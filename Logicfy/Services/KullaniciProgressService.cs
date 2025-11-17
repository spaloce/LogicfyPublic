using AutoMapper;
using Logicfy.Data.UnitOfWork;
using Logicfy.Models;
using Logicfy.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Logicfy.Services
{
    public class KullaniciProgressService : IKullaniciProgressService
    {
        private readonly IUnitOfWork _unitOfWork;

        public KullaniciProgressService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // ---------------------------------------------------------
        //   SORU ÇÖZÜMÜ
        // ---------------------------------------------------------
        public async Task SoruCevaplaAsync(int kullaniciId, int soruId, bool dogruMu, string cevapJson, int sureMs)
        {
            var soruRepo = _unitOfWork.Repository<Soru>();
            var cevapRepo = _unitOfWork.Repository<KullaniciSoruCevap>();
            var userRepo = _unitOfWork.Repository<Kullanici>();

            var soru = await soruRepo.Query()
                .Include(x => x.Ders)
                    .ThenInclude(d => d.Kisim)
                        .ThenInclude(k => k.Unite)
                .FirstOrDefaultAsync(x => x.Id == soruId);

            if (soru == null)
                throw new Exception("Soru bulunamadı.");

            var user = await userRepo.GetByIdAsync(kullaniciId);

            // -----------------------------
            // Soru cevabını kaydet
            // -----------------------------
            await cevapRepo.AddAsync(new KullaniciSoruCevap
            {
                KullaniciId = kullaniciId,
                SoruId = soruId,
                DogruMu = dogruMu,
                CevapJson = cevapJson,
                SureMs = sureMs
            });

            // -----------------------------
            // XP Güncelle
            // -----------------------------
            int xp = dogruMu ? 10 : 2;
            user.XP += xp;
            user.Seviye = (user.XP / 100) + 1;
            userRepo.Update(user);

            await _unitOfWork.SaveAsync();

            // ------------------------------------------------------
            // PROGRESS GÜNCELLEME: DERS - KISIM - ÜNİTE
            // ------------------------------------------------------
            await UpdateDersProgress(kullaniciId, soru.DersId);
            await UpdateKisimProgress(kullaniciId, soru.Ders.KisimId);
            await UpdateUniteProgress(kullaniciId, soru.Ders.Kisim.UniteId);
        }

        // ---------------------------------------------------------
        //   DERS PROGRESS
        // ---------------------------------------------------------
        private async Task UpdateDersProgress(int kullaniciId, int dersId)
        {
            var soruRepo = _unitOfWork.Repository<Soru>();
            var cevapRepo = _unitOfWork.Repository<KullaniciSoruCevap>();
            var progressRepo = _unitOfWork.Repository<KullaniciDersIlerleme>();

            int toplamSoru = await soruRepo.Query().CountAsync(x => x.DersId == dersId);

            int tamamlanan = await cevapRepo.Query()
                .Include(x => x.Soru)
                .CountAsync(x => x.KullaniciId == kullaniciId && x.Soru.DersId == dersId);

            int yuzde = toplamSoru == 0 ? 0 : (int)((tamamlanan * 100.0) / toplamSoru);

            var ilerleme = await progressRepo.Query()
                .FirstOrDefaultAsync(x => x.KullaniciId == kullaniciId && x.DersId == dersId);

            if (ilerleme == null)
            {
                await progressRepo.AddAsync(new KullaniciDersIlerleme
                {
                    KullaniciId = kullaniciId,
                    DersId = dersId,
                    ToplamSoruSayisi = toplamSoru,
                    TamamlananSoruSayisi = tamamlanan,
                    IlerlemeOrani = yuzde,
                    TamamlandiMi = yuzde == 100
                });
            }
            else
            {
                ilerleme.ToplamSoruSayisi = toplamSoru;
                ilerleme.TamamlananSoruSayisi = tamamlanan;
                ilerleme.IlerlemeOrani = yuzde;
                ilerleme.TamamlandiMi = yuzde == 100;

                progressRepo.Update(ilerleme);
            }

            await _unitOfWork.SaveAsync();
        }

        // ---------------------------------------------------------
        //   KISIM PROGRESS
        // ---------------------------------------------------------
        private async Task UpdateKisimProgress(int kullaniciId, int kisimId)
        {
            var dersRepo = _unitOfWork.Repository<Ders>();
            var dersProgressRepo = _unitOfWork.Repository<KullaniciDersIlerleme>();
            var kisimProgressRepo = _unitOfWork.Repository<KullaniciKisimProgress>();

            // Bu kısımdaki tüm dersler
            var dersler = await dersRepo.Query()
                .Where(x => x.KisimId == kisimId)
                .ToListAsync();

            int toplamDers = dersler.Count;

            int tamamlananDers = 0;

            foreach (var ders in dersler)
            {
                var ilerleme = await dersProgressRepo.Query()
                    .FirstOrDefaultAsync(x => x.KullaniciId == kullaniciId && x.DersId == ders.Id);

                if (ilerleme != null && ilerleme.TamamlandiMi)
                    tamamlananDers++;
            }

            int yuzde = toplamDers == 0 ? 0 : (int)((tamamlananDers * 100.0) / toplamDers);

            var kp = await kisimProgressRepo.Query()
                .FirstOrDefaultAsync(x => x.KullaniciId == kullaniciId && x.KisimId == kisimId);

            if (kp == null)
            {
                await kisimProgressRepo.AddAsync(new KullaniciKisimProgress
                {
                    KullaniciId = kullaniciId,
                    KisimId = kisimId,
                    ToplamDersSayisi = toplamDers,
                    TamamlananDersSayisi = tamamlananDers,
                    IlerlemeOrani = yuzde
                });
            }
            else
            {
                kp.ToplamDersSayisi = toplamDers;
                kp.TamamlananDersSayisi = tamamlananDers;
                kp.IlerlemeOrani = yuzde;
                kisimProgressRepo.Update(kp);
            }

            await _unitOfWork.SaveAsync();
        }

        // ---------------------------------------------------------
        //   ÜNİTE PROGRESS (Kısım Üzerinden)
        // ---------------------------------------------------------
        private async Task UpdateUniteProgress(int kullaniciId, int uniteId)
        {
            var kisimRepo = _unitOfWork.Repository<Kisim>();
            var kisimProgressRepo = _unitOfWork.Repository<KullaniciKisimProgress>();
            var uniteProgressRepo = _unitOfWork.Repository<KullaniciUnitProgress>();

            // Ünitedeki tüm kısımlar
            var kisimlar = await kisimRepo.Query()
                .Where(x => x.UniteId == uniteId)
                .ToListAsync();

            int toplamDers = 0;
            int tamamlananDers = 0;

            foreach (var kisim in kisimlar)
            {
                // Her kısım için progress yoksa 0 kabul edilir
                var kp = await kisimProgressRepo.Query()
                    .FirstOrDefaultAsync(x => x.KullaniciId == kullaniciId && x.KisimId == kisim.Id);

                if (kp != null)
                {
                    toplamDers += kp.ToplamDersSayisi;
                    tamamlananDers += kp.TamamlananDersSayisi;
                }
            }

            int yuzde = toplamDers == 0 ? 0 : (int)((tamamlananDers * 100.0) / toplamDers);

            var up = await uniteProgressRepo.Query()
                .FirstOrDefaultAsync(x => x.KullaniciId == kullaniciId && x.UniteId == uniteId);

            if (up == null)
            {
                await uniteProgressRepo.AddAsync(new KullaniciUnitProgress
                {
                    KullaniciId = kullaniciId,
                    UniteId = uniteId,
                    ToplamDersSayisi = toplamDers,
                    TamamlananDersSayisi = tamamlananDers,
                    IlerlemeOrani = yuzde
                });
            }
            else
            {
                up.ToplamDersSayisi = toplamDers;
                up.TamamlananDersSayisi = tamamlananDers;
                up.IlerlemeOrani = yuzde;

                uniteProgressRepo.Update(up);
            }

            await _unitOfWork.SaveAsync();
        }

        // ---------------------------------------------------------
        //  DERS PROGRESS GET
        // ---------------------------------------------------------
        public async Task<KullaniciDersIlerleme?> GetDersProgressAsync(int kullaniciId, int dersId)
        {
            return await _unitOfWork.Repository<KullaniciDersIlerleme>()
                .Query()
                .Include(x => x.Ders)
                .FirstOrDefaultAsync(x => x.KullaniciId == kullaniciId && x.DersId == dersId);
        }

        // ---------------------------------------------------------
        //  KISIM PROGRESS GET
        // ---------------------------------------------------------
        public async Task<KullaniciKisimProgress?> GetKisimProgressAsync(int kullaniciId, int kisimId)
        {
            return await _unitOfWork.Repository<KullaniciKisimProgress>()
                .Query()
                .Include(x => x.Kisim)
                .FirstOrDefaultAsync(x => x.KullaniciId == kullaniciId && x.KisimId == kisimId);
        }

        // ---------------------------------------------------------
        //  UNITE PROGRESS GET
        // ---------------------------------------------------------
        public async Task<KullaniciUnitProgress?> GetUniteProgressAsync(int kullaniciId, int uniteId)
        {
            return await _unitOfWork.Repository<KullaniciUnitProgress>()
                .Query()
                .Include(x => x.Unite)
                .FirstOrDefaultAsync(x => x.KullaniciId == kullaniciId && x.UniteId == uniteId);
        }
    }
}
