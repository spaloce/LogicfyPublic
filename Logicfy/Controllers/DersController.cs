using Logicfy.Controllers;
using Logicfy.Data.UnitOfWork;
using Logicfy.Dtos.Ders;
using Logicfy.Models;
using Logicfy.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Logicfy.Api.Controllers
{
    [Route("api/[controller]")]
    public class DersController : BaseController
    {
        private readonly IDersService _service;
        private readonly IUnitOfWork _unitOfWork;
        public DersController(IDersService service, IUnitOfWork unitOfWork)
        {
            _service = service;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var repo = _unitOfWork.Repository<Ders>();

            var list = await repo.Query()
                .OrderBy(x => x.Sira)
                .ToListAsync();

            return Ok(new
            {
                status = true,
                message = "Dersler yüklendi",
                data = list
            });
        }


        // ---------------------------------------------------------
        // 1) KISIMA AİT TÜM DERSLER
        // ---------------------------------------------------------
        //[HttpGet("kisim/{kisimId}")]
        //public async Task<IActionResult> GetByKisim(int kisimId)
        //{
        //    var data = await _service.GetByKisimIdAsync(kisimId);
        //    return Success(data);
        //}

        // ---------------------------------------------------------
        // 2) TEK DERS GETİR
        // ---------------------------------------------------------
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var data = await _service.GetByIdAsync(id);

            if (data == null)
                return Fail("Ders bulunamadı.");

            return Success(data);
        }

        // ---------------------------------------------------------
        // 3) DERS OLUŞTUR
        // ---------------------------------------------------------
        [HttpPost("kisim/{kisimId}")]
        public async Task<IActionResult> Create(int kisimId, [FromBody] DersCreateDto dto)
        {
            var data = await _service.CreateAsync(kisimId, dto);
            return Success(data);
        }

        // ---------------------------------------------------------
        // 4) DERS GÜNCELLE
        // ---------------------------------------------------------
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] DersCreateDto dto)
        {
            var data = await _service.UpdateAsync(id, dto);

            if (data == null)
                return Fail("Ders bulunamadı.");

            return Success(data);
        }

        // ---------------------------------------------------------
        // 5) DERS SİL
        // ---------------------------------------------------------
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.DeleteAsync(id);

            if (!ok)
                return Fail("Ders bulunamadı.");

            return Success("Silindi.");
        }

        // -------------------------------------------------------
        // GET api/ders/kisim/{kisimId}
        // -------------------------------------------------------
        [HttpGet("kisim/{kisimId:int}")]
        public async Task<IActionResult> GetByKisim(int kisimId)
        {
            var dersRepo = _unitOfWork.Repository<Ders>();

            var dersler = await dersRepo.Query()
                .Where(x => x.KisimId == kisimId)
                .OrderBy(x => x.Sira)
                .ToListAsync();

            return Ok(new
            {
                status = true,
                message = "Dersler yüklendi",
                data = dersler
            });
        }
    }
}
