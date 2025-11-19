using AutoMapper;
using Logicfy.Data.UnitOfWork;
using Logicfy.Dtos.LearningPath;
using Logicfy.Models;
using Logicfy.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Logicfy.Services
{
    public class KullaniciLearningPathService : IKullaniciLearningPathService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IKullaniciProgressService _progressService;

        public KullaniciLearningPathService(IUnitOfWork uow, IKullaniciProgressService progressService)
        {
            _unitOfWork = uow;
            _progressService = progressService;
        }

        // ---------------------------------------------------------
        //  DİL OVERVIEW
        // ---------------------------------------------------------
        public async Task<KullaniciDilOverviewDto> GetDilOverviewAsync(string kullaniciId, int dilId)
        {
            var dilRepo = _unitOfWork.Repository<ProgramlamaDili>();
            var uniteRepo = _unitOfWork.Repository<Unite>();
            var userRepo = _unitOfWork.Repository<Kullanici>();

            var dil = await dilRepo.GetByIdAsync(dilId);
            if (dil == null)
                throw new Exception("Dil bulunamadı.");

            var user = await userRepo.GetByIdAsync(kullaniciId);

            var uniteler = await uniteRepo.Query()
                .Where(x => x.ProgramlamaDiliId == dilId)
                .OrderBy(x => x.Sira)
                .ToListAsync();

            var unitelerDto = new List<KullaniciUniteOverviewDto>();

            foreach (var u in uniteler)
            {
                var up = await _progressService.GetUniteProgressAsync(kullaniciId, u.Id);

                unitelerDto.Add(new KullaniciUniteOverviewDto
                {
                    UniteId = u.Id,
                    Baslik = u.Baslik,
                    IlerlemeOrani = up?.IlerlemeOrani ?? 0,
                    KilitliMi = false, // İstersen kilit sistemini buradan kurarsın
                    Kisimlar = new List<KullaniciKisimOverviewDto>()
                });
            }

            return new KullaniciDilOverviewDto
            {
                DilId = dil.Id,
                DilAdi = dil.Ad,
                KullaniciXp = user.XP,
                KullaniciSeviye = user.Seviye,
                Streak = user.Streak,
                Uniteler = unitelerDto
            };
        }

        // ---------------------------------------------------------
        //  ÜNİTE OVERVIEW
        // ---------------------------------------------------------
        public async Task<KullaniciUniteOverviewDto> GetUniteOverviewAsync(string kullaniciId, int uniteId)
        {
            var uniteRepo = _unitOfWork.Repository<Unite>();
            var kisimRepo = _unitOfWork.Repository<Kisim>();

            var unite = await uniteRepo.GetByIdAsync(uniteId);
            if (unite == null)
                throw new Exception("Unite bulunamadı.");

            var kisimlar = await kisimRepo.Query()
                .Where(x => x.UniteId == uniteId)
                .OrderBy(x => x.Sira)
                .ToListAsync();

            var kisimDtos = new List<KullaniciKisimOverviewDto>();

            foreach (var k in kisimlar)
            {
                var kp = await _progressService.GetKisimProgressAsync(kullaniciId, k.Id);

                kisimDtos.Add(new KullaniciKisimOverviewDto
                {
                    KisimId = k.Id,
                    Baslik = k.Baslik,
                    IlerlemeOrani = kp?.IlerlemeOrani ?? 0,
                    TamamlandiMi = kp?.IlerlemeOrani == 100,
                    Dersler = new List<KullaniciDersOverviewDto>()
                });
            }

            return new KullaniciUniteOverviewDto
            {
                UniteId = unite.Id,
                Baslik = unite.Baslik,
                IlerlemeOrani = kisimDtos.Average(x => x.IlerlemeOrani),
                KilitliMi = false,
                Kisimlar = kisimDtos
            };
        }

        // ---------------------------------------------------------
        //  KISIM OVERVIEW
        // ---------------------------------------------------------
        public async Task<KullaniciKisimOverviewDto> GetKisimOverviewAsync(string kullaniciId, int kisimId)
        {
            var kisimRepo = _unitOfWork.Repository<Kisim>();
            var dersRepo = _unitOfWork.Repository<Ders>();

            var kisim = await kisimRepo.GetByIdAsync(kisimId);
            if (kisim == null)
                throw new Exception("Kisim bulunamadı.");

            var dersler = await dersRepo.Query()
                .Where(x => x.KisimId == kisimId)
                .OrderBy(x => x.Sira)
                .ToListAsync();

            var dersDtoList = new List<KullaniciDersOverviewDto>();

            foreach (var d in dersler)
            {
                var dp = await _progressService.GetDersProgressAsync(kullaniciId, d.Id);

                dersDtoList.Add(new KullaniciDersOverviewDto
                {
                    DersId = d.Id,
                    Baslik = d.Baslik,
                    ToplamSoru = dp?.ToplamSoruSayisi ?? 0,
                    CozulenSoru = dp?.TamamlananSoruSayisi ?? 0,
                    IlerlemeOrani = dp?.IlerlemeOrani ?? 0
                });
            }

            return new KullaniciKisimOverviewDto
            {
                KisimId = kisim.Id,
                Baslik = kisim.Baslik,
                IlerlemeOrani = dersDtoList.Average(x => x.IlerlemeOrani),
                TamamlandiMi = dersDtoList.All(x => x.IlerlemeOrani == 100),
                Dersler = dersDtoList
            };
        }

        // ---------------------------------------------------------
        //  DERS OVERVIEW
        // ---------------------------------------------------------
        public async Task<KullaniciDersOverviewDto> GetDersOverviewAsync(string kullaniciId, int dersId)
        {
            var dersRepo = _unitOfWork.Repository<Ders>();
            var ders = await dersRepo.GetByIdAsync(dersId);

            var dp = await _progressService.GetDersProgressAsync(kullaniciId, dersId);

            return new KullaniciDersOverviewDto
            {
                DersId = ders.Id,
                Baslik = ders.Baslik,
                CozulenSoru = dp?.TamamlananSoruSayisi ?? 0,
                ToplamSoru = dp?.ToplamSoruSayisi ?? 0,
                IlerlemeOrani = dp?.IlerlemeOrani ?? 0
            };
        }

        // ---------------------------------------------------------
        //  AKTİF DERS GET/SET
        // ---------------------------------------------------------
        public async Task SetAktifDersAsync(string kullaniciId, int dersId)
        {
            var repo = _unitOfWork.Repository<KullaniciDersKaydi>();

            var kayit = await repo.Query()
                .FirstOrDefaultAsync(x => x.KullaniciId == kullaniciId);

            if (kayit == null)
            {
                await repo.AddAsync(new KullaniciDersKaydi
                {
                    KullaniciId = kullaniciId,
                    DersId = dersId,
                    AktifMi = true
                });
            }
            else
            {
                kayit.DersId = dersId;
                kayit.AktifMi = true;
                repo.Update(kayit);
            }

            await _unitOfWork.SaveAsync();
        }

        public async Task<int?> GetAktifDersAsync(string kullaniciId)
        {
            var repo = _unitOfWork.Repository<KullaniciDersKaydi>();

            var kayit = await repo.Query()
                .FirstOrDefaultAsync(x => x.KullaniciId == kullaniciId && x.AktifMi);

            return kayit?.DersId;
        }
    }
}
