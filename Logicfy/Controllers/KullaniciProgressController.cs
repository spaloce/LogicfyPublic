using Logicfy.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Logicfy.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class KullaniciProgressController : ControllerBase
    {
        private readonly IKullaniciProgressService _progressService;

        public KullaniciProgressController(IKullaniciProgressService progressService)
        {
            _progressService = progressService;
        }

        // -------------------------------------------------------
        //   POST: api/kullaniciprogress/soru-cevapla
        // -------------------------------------------------------
        [HttpPost("soru-cevapla")]
        public async Task<IActionResult> SoruCevapla([FromBody] SoruCevaplaRequest req)
        {
            await _progressService.SoruCevaplaAsync(
                req.KullaniciId,
                req.SoruId,
                req.DogruMu,
                req.CevapJson,
                req.SureMs
            );

            return Ok(new { message = "Soru işleme alındı." });
        }

        // -------------------------------------------------------
        //   GET: api/kullaniciprogress/ders/{kullaniciId}/{dersId}
        // -------------------------------------------------------
        [HttpGet("ders/{kullaniciId:int}/{dersId:int}")]
        public async Task<IActionResult> GetDersProgress(string kullaniciId, int dersId)
        {
            var result = await _progressService.GetDersProgressAsync(kullaniciId, dersId);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        // -------------------------------------------------------
        //   GET: api/kullaniciprogress/kisim/{kullaniciId}/{kisimId}
        // -------------------------------------------------------
        [HttpGet("kisim/{kullaniciId:int}/{kisimId:int}")]
        public async Task<IActionResult> GetKisimProgress(string kullaniciId, int kisimId)
        {
            var result = await _progressService.GetKisimProgressAsync(kullaniciId, kisimId);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        // -------------------------------------------------------
        //   GET: api/kullaniciprogress/unite/{kullaniciId}/{uniteId}
        // -------------------------------------------------------
        [HttpGet("unite/{kullaniciId:int}/{uniteId:int}")]
        public async Task<IActionResult> GetUniteProgress(string kullaniciId, int uniteId)
        {
            var result = await _progressService.GetUniteProgressAsync(kullaniciId, uniteId);
            if (result == null)
                return NotFound();

            return Ok(result);
        }
    }

    // -----------------------------------------------------------
    //   REQUEST DTO
    // -----------------------------------------------------------
    public class SoruCevaplaRequest
    {
        public string KullaniciId { get; set; }
        public int SoruId { get; set; }
        public bool DogruMu { get; set; }
        public string CevapJson { get; set; } = "";
        public int SureMs { get; set; }
    }
}
