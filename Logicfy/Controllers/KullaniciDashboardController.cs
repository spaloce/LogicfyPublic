using Logicfy.Controllers;
using Logicfy.Data.UnitOfWork;
using Logicfy.Models;
using Logicfy.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Logicfy.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class KullaniciDashboardController : BaseController
    {
        private readonly IKullaniciService _kullaniciService;
        private readonly IKullaniciProgressService _progressService;
        private readonly IUnitOfWork _unitOfWork;

        public KullaniciDashboardController(
            IKullaniciService kullaniciService,
            IKullaniciProgressService progressService,
            IUnitOfWork unitOfWork)
        {
            _kullaniciService = kullaniciService;
            _progressService = progressService;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("{kullaniciId}")]
        public async Task<IActionResult> GetDashboard(string kullaniciId)
        {
            try
            {
                var userRepo = _unitOfWork.Repository<Kullanici>();
                var soruRepo = _unitOfWork.Repository<Soru>();
                var cevapRepo = _unitOfWork.Repository<KullaniciSoruCevap>();
                var dersRepo = _unitOfWork.Repository<Ders>();
                var kisimRepo = _unitOfWork.Repository<Kisim>();
                var uniteRepo = _unitOfWork.Repository<Unite>();

                // --------------------------
                // KULLANICI BİLGİLERİ
                // --------------------------
                var user = await userRepo.Query()
                    .FirstOrDefaultAsync(x => x.Id == kullaniciId);

                if (user == null)
                    return Ok("Kullanıcı bulunamadı.");

                // --------------------------
                // SON ÇÖZÜLEN SORULAR
                // --------------------------
                var sonCevaplar = await cevapRepo.Query()
                    .Where(x => x.KullaniciId == kullaniciId)
                    .Include(x => x.Soru)
                        .ThenInclude(s => s.Ders)
                            .ThenInclude(d => d.Kisim)
                                .ThenInclude(k => k.Unite)
                    .OrderByDescending(x => x.Id)
                    .Take(10)
                    .ToListAsync();

                var sonAktiviteler = sonCevaplar.Select(x => new
                {
                    soruId = x.SoruId,
                    soru = x.Soru?.SoruMetni,
                    ders = x.Soru?.Ders?.Baslik,
                    kisim = x.Soru?.Ders?.Kisim?.Baslik,
                    unite = x.Soru?.Ders?.Kisim?.Unite?.Baslik,
                    dogruMu = x.DogruMu,
                    sureMs = x.SureMs,
                    tarih = x.CreatedAt
                });

                // --------------------------
                // DERS PROGRESS
                // --------------------------
                var dersler = await dersRepo.Query().ToListAsync();

                var dersProgress = new List<object>();

                foreach (var ders in dersler)
                {
                    var dp = await _progressService.GetDersProgressAsync(kullaniciId, ders.Id);

                    dersProgress.Add(new
                    {
                        dersId = ders.Id,
                        dersAdi = ders.Baslik,
                        toplamSoru = dp?.ToplamSoruSayisi ?? 0,
                        tamamlananSoru = dp?.TamamlananSoruSayisi ?? 0,
                        ilerleme = dp?.IlerlemeOrani ?? 0,
                        tamamlandiMi = dp?.TamamlandiMi ?? false
                    });
                }

                // --------------------------
                // KISIM PROGRESS
                // --------------------------
                var kisimlar = await kisimRepo.Query().ToListAsync();
                var kisimProgress = new List<object>();

                foreach (var k in kisimlar)
                {
                    var kp = await _progressService.GetKisimProgressAsync(kullaniciId, k.Id);

                    kisimProgress.Add(new
                    {
                        kisimId = k.Id,
                        kisimAdi = k.Baslik,
                        toplamDers = kp?.ToplamDersSayisi ?? 0,
                        tamamlananDers = kp?.TamamlananDersSayisi ?? 0,
                        ilerleme = kp?.IlerlemeOrani ?? 0
                    });
                }

                // --------------------------
                // ÜNİTE PROGRESS
                // --------------------------
                var uniteler = await uniteRepo.Query().ToListAsync();
                var uniteProgress = new List<object>();

                foreach (var u in uniteler)
                {
                    var up = await _progressService.GetUniteProgressAsync(kullaniciId, u.Id);

                    uniteProgress.Add(new
                    {
                        uniteId = u.Id,
                        uniteAdi = u.Baslik,
                        toplamDers = up?.ToplamDersSayisi ?? 0,
                        tamamlananDers = up?.TamamlananDersSayisi ?? 0,
                        ilerleme = up?.IlerlemeOrani ?? 0
                    });
                }

                // --------------------------
                // TÜM DASHBOARD SONUCU
                // --------------------------
                var result = new
                {
                    kullanici = new
                    {
                        user.Id,
                        user.Email,
                        user.KayitTarihi,
                        user.AdSoyad,
                        user.XP,
                        user.Seviye
                    },
                    sonAktiviteler,
                    dersProgress,
                    kisimProgress,
                    uniteProgress
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok("Dashboard yüklenirken hata: " + ex.Message);
            }
        }
    }
}
