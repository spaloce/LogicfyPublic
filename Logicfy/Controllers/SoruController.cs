using Logicfy.Controllers;
using Logicfy.Dtos.Soru;
using Logicfy.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Logicfy.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class SoruController : BaseController
    {
        private readonly ISoruService _service;

        public SoruController(ISoruService service)
        {
            _service = service;
        }

        // ---------------------------------------------------------
        // 1) TÜM SORULAR (Admin)
        // ---------------------------------------------------------
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAllAsync();
            return Ok(data);
        }

        // ---------------------------------------------------------
        // 2) DERSİN TÜM SORULARI
        // ---------------------------------------------------------
        [HttpGet("ders/{dersId}")]
        public async Task<IActionResult> GetByDers(int dersId)
        {
            var data = await _service.GetByDersIdAsync(dersId);
            return Ok(data);
        }

        // ---------------------------------------------------------
        // 3) TEK SORU
        // ---------------------------------------------------------
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var data = await _service.GetByIdAsync(id);
            if (data == null)
                return Ok("Soru bulunamadı.");

            return Ok(data);
        }

        // ---------------------------------------------------------
        // 4) SİL
        // ---------------------------------------------------------
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.DeleteAsync(id);
            if (!ok)
                return Ok("Soru bulunamadı.");

            return Ok("Silindi.");
        }

        // ---------------------------------------------------------
        // 5) TIP 1 - OLUŞTUR
        // ---------------------------------------------------------
        [HttpPost("tip1/{dersId}")]
        public async Task<IActionResult> CreateTip1(int dersId, [FromBody] SoruTip1CreateDto dto)
        {
            var result = await _service.CreateTip1Async(dersId, dto);
            return Ok(result);
        }

        // ---------------------------------------------------------
        // 6) TIP 1 - GÜNCELLE
        // ---------------------------------------------------------
        [HttpPut("tip1/{id}")]
        public async Task<IActionResult> UpdateTip1(int id, [FromBody] SoruTip1CreateDto dto)
        {
            var result = await _service.UpdateTip1Async(id, dto);

            if (result == null)
                return Ok("Soru bulunamadı.");

            return Ok(result);
        }

        // ---------------------------------------------------------
        // 7) TIP 2 - OLUŞTUR
        // ---------------------------------------------------------
        [HttpPost("tip2/{dersId}")]
        public async Task<IActionResult> CreateTip2(int dersId, [FromBody] SoruTip2CreateDto dto)
        {
            var result = await _service.CreateTip2Async(dersId, dto);
            return Ok(result);
        }

        // ---------------------------------------------------------
        // 8) TIP 2 - GÜNCELLE
        // ---------------------------------------------------------
        [HttpPut("tip2/{id}")]
        public async Task<IActionResult> UpdateTip2(int id, [FromBody] SoruTip2CreateDto dto)
        {
            var result = await _service.UpdateTip2Async(id, dto);

            if (result == null)
                return Ok("Soru bulunamadı.");

            return Ok(result);
        }

        // ---------------------------------------------------------
        // 9) TIP 3 - OLUŞTUR
        // ---------------------------------------------------------
        [HttpPost("tip3/{dersId}")]
        public async Task<IActionResult> CreateTip3(int dersId, [FromBody] SoruTip3CreateDto dto)
        {
            var result = await _service.CreateTip3Async(dersId, dto);
            return Ok(result);
        }

        // ---------------------------------------------------------
        // 10) TIP 3 - GÜNCELLE
        // ---------------------------------------------------------
        [HttpPut("tip3/{id}")]
        public async Task<IActionResult> UpdateTip3(int id, [FromBody] SoruTip3CreateDto dto)
        {
            var result = await _service.UpdateTip3Async(id, dto);

            if (result == null)
                return Ok("Soru bulunamadı.");

            return Ok(result);
        }

        // ---------------------------------------------------------
        // 11) TIP 4 - OLUŞTUR
        // ---------------------------------------------------------
        [HttpPost("tip4/{dersId}")]
        public async Task<IActionResult> CreateTip4(int dersId, [FromBody] SoruTip4CreateDto dto)
        {
            var result = await _service.CreateTip4Async(dersId, dto);
            return Ok(result);
        }

        // ---------------------------------------------------------
        // 12) TIP 4 - GÜNCELLE
        // ---------------------------------------------------------
        [HttpPut("tip4/{id}")]
        public async Task<IActionResult> UpdateTip4(int id, [FromBody] SoruTip4CreateDto dto)
        {
            var result = await _service.UpdateTip4Async(id, dto);

            if (result == null)
                return Ok("Soru bulunamadı.");

            return Ok(result);
        }
    }
}
