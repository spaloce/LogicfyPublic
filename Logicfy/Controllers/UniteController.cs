using Logicfy.Controllers;
using Logicfy.Dtos.Unite;
using Logicfy.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Logicfy.Api.Controllers
{
    [Route("api/[controller]")]
    public class UniteController : BaseController
    {
        private readonly IUniteService _service;

        public UniteController(IUniteService service)
        {
            _service = service;
        }

        // ---------------------------------------------------------
        // BİR DİLE AİT ÜNİTELER
        // ---------------------------------------------------------
        [HttpGet("dil/{dilId}")]
        public async Task<IActionResult> GetAllByDil(int dilId)
        {
            var data = await _service.GetByDilIdAsync(dilId);
            return Success(data);
        }

        // ---------------------------------------------------------
        // TEK ÜNİTE GETİR
        // ---------------------------------------------------------
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var data = await _service.GetByIdAsync(id);
            if (data == null)
                return Fail("Ünite bulunamadı.");

            return Success(data);
        }

        // ---------------------------------------------------------
        // ÜNİTE OLUŞTUR
        // ---------------------------------------------------------
        [HttpPost("dil/{dilId}")]
        public async Task<IActionResult> Create(int dilId, [FromBody] UniteCreateDto dto)
        {
            var data = await _service.CreateAsync(dilId, dto);
            return Success(data);
        }

        // ---------------------------------------------------------
        // ÜNİTE GÜNCELLE
        // ---------------------------------------------------------
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UniteCreateDto dto)
        {
            var data = await _service.UpdateAsync(id, dto);

            if (data == null)
                return Fail("Ünite bulunamadı.");

            return Success(data);
        }

        // ---------------------------------------------------------
        // ÜNİTE SİL
        // ---------------------------------------------------------
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.DeleteAsync(id);

            if (!ok)
                return Fail("Ünite bulunamadı.");

            return Success("Silindi.");
        }
    }
}
