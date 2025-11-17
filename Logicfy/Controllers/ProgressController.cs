using Logicfy.Controllers;
using Logicfy.Dtos;
using Logicfy.Dtos.Progress;
using Logicfy.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Logicfy.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class ProgressController : BaseController
    {
        private readonly IKullaniciProgressService _progressService;

        public ProgressController(IKullaniciProgressService progressService)
        {
            _progressService = progressService;
        }

        // ---------------------------------------------------------
        // 1) SORU ÇÖZÜMÜ
        // ---------------------------------------------------------
        [HttpPost("soru-cevapla")]
        public async Task<IActionResult> SoruCevapla([FromBody] SoruCevaplaDto dto)
        {
            var userId = GetUserId();

            await _progressService.SoruCevaplaAsync(
                userId,
                dto.SoruId,
                dto.DogruMu,
                dto.CevapJson,
                dto.SureMs
            );

            return Ok(new { success = true, message = "Cevap işlendi." });
        }

        // ---------------------------------------------------------
        // 2) DERS PROGRESS
        // ---------------------------------------------------------
        [HttpGet("ders/{dersId}")]
        public async Task<IActionResult> DersProgress(int dersId)
        {
            var userId = GetUserId();
            var data = await _progressService.GetDersProgressAsync(userId, dersId);

            return Ok(new { success = true, data });
        }

        // ---------------------------------------------------------
        // 3) KISIM PROGRESS
        // ---------------------------------------------------------
        [HttpGet("kisim/{kisimId}")]
        public async Task<IActionResult> KisimProgress(int kisimId)
        {
            var userId = GetUserId();
            var data = await _progressService.GetKisimProgressAsync(userId, kisimId);

            return Ok(new { success = true, data });
        }

        // ---------------------------------------------------------
        // 4) UNITE PROGRESS
        // ---------------------------------------------------------
        [HttpGet("unite/{uniteId}")]
        public async Task<IActionResult> UniteProgress(int uniteId)
        {
            var userId = GetUserId();
            var data = await _progressService.GetUniteProgressAsync(userId, uniteId);

            return Ok(new { success = true, data });
        }
    }
}
