using Logicfy.Controllers;
using Logicfy.Dtos.ProgramlamaDili;
using Logicfy.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Logicfy.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class ProgramlamaDiliController : BaseController
    {
        private readonly IProgramlamaDiliService _service;

        public ProgramlamaDiliController(IProgramlamaDiliService service)
        {
            _service = service;
        }

        // ---------------------------------------------------------
        // TÜM DİLLER
        // ---------------------------------------------------------
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAllAsync();
            return Ok(data);
        }

        // ---------------------------------------------------------
        // TEK DİL
        // ---------------------------------------------------------
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var data = await _service.GetByIdAsync(id);
            if (data == null)
                return Ok("Programlama dili bulunamadı.");

            return Ok(data);
        }

        // ---------------------------------------------------------
        // OLUŞTUR
        // ---------------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProgramlamaDiliCreateDto dto)
        {
            var data = await _service.CreateAsync(dto);
            return Ok(data);
        }

        // ---------------------------------------------------------
        // GÜNCELLE
        // ---------------------------------------------------------
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProgramlamaDiliCreateDto dto)
        {
            var data = await _service.UpdateAsync(id, dto);

            if (data == null)
                return Ok("Programlama dili bulunamadı.");

            return Ok(data);
        }

        // ---------------------------------------------------------
        // SİL
        // ---------------------------------------------------------
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.DeleteAsync(id);

            if (!ok)
                return Ok("Programlama dili bulunamadı.");

            return Ok("Silindi.");
        }
    }
}
