using Logicfy.Controllers;
using Logicfy.Data.UnitOfWork;
using Logicfy.Dtos.Unite;
using Logicfy.Models;
using Logicfy.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Logicfy.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class UniteController : BaseController
    {
        private readonly IUniteService _service;
        private readonly IUnitOfWork _unitOfWork;

        public UniteController(IUniteService service, IUnitOfWork unitOfWork)
        {
            _service = service;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var repo = _unitOfWork.Repository<Unite>();

            var list = await repo.Query()
                .OrderBy(x => x.Sira)
                .ToListAsync();

            return Ok(new
            {
                status = true,
                message = "Üniteler yüklendi",
                data = list
            });
        }


        // ---------------------------------------------------------
        // BİR DİLE AİT ÜNİTELER
        // ---------------------------------------------------------
        [HttpGet("dil/{dilId}")]
        public async Task<IActionResult> GetAllByDil(int dilId)
        {
            var data = await _service.GetByDilIdAsync(dilId);
            return Ok(data);
        }

        // ---------------------------------------------------------
        // TEK ÜNİTE GETİR
        // ---------------------------------------------------------
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var data = await _service.GetByIdAsync(id);
            if (data == null)
                return Ok("Ünite bulunamadı.");

            return Ok(data);
        }

        // ---------------------------------------------------------
        // ÜNİTE OLUŞTUR
        // ---------------------------------------------------------
        [HttpPost("dil/{dilId}")]
        public async Task<IActionResult> Create(int dilId, [FromBody] UniteCreateDto dto)
        {
            var data = await _service.CreateAsync(dilId, dto);
            return Ok(data);
        }

        // ---------------------------------------------------------
        // ÜNİTE GÜNCELLE
        // ---------------------------------------------------------
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UniteCreateDto dto)
        {
            var data = await _service.UpdateAsync(id, dto);

            if (data == null)
                return Ok("Ünite bulunamadı.");

            return Ok(data);
        }

        // ---------------------------------------------------------
        // ÜNİTE SİL
        // ---------------------------------------------------------
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.DeleteAsync(id);

            if (!ok)
                return Ok("Ünite bulunamadı.");

            return Ok("Silindi.");
        }
    }
}
