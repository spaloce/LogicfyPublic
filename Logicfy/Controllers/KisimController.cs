using Logicfy.Controllers;
using Logicfy.Data.UnitOfWork;
using Logicfy.Dtos.Kisim;
using Logicfy.Models;
using Logicfy.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Logicfy.Api.Controllers
{
    [Route("api/[controller]")]
    public class KisimController : BaseController
    {
        private readonly IKisimService _service;
        private readonly IUnitOfWork _unitOfWork;

        public KisimController(IKisimService service, IUnitOfWork unitOfWork)
        {
            _service = service;
            _unitOfWork = unitOfWork;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var repo = _unitOfWork.Repository<Kisim>();

            var list = await repo.Query()
                .OrderBy(x => x.Sira)
                .ToListAsync();

            return Ok(new
            {
                status = true,
                message = "Kısımlar yüklendi",
                data = list
            });
        }

        // ---------------------------------------------------------
        // 1) ÜNİTEYE AİT TÜM KISIMLAR
        // ---------------------------------------------------------
        //[HttpGet("unite/{uniteId}")]
        //public async Task<IActionResult> GetByUnite(int uniteId)
        //{
        //    var data = await _service.GetByUniteIdAsync(uniteId);
        //    return Success(data);
        //}

        // ---------------------------------------------------------
        // 2) TEK KISIM GETİR
        // ---------------------------------------------------------
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var data = await _service.GetByIdAsync(id);

            if (data == null)
                return Fail("Kısım bulunamadı.");

            return Success(data);
        }

        // ---------------------------------------------------------
        // 3) KISIM OLUŞTUR
        // ---------------------------------------------------------
        [HttpPost("unite/{uniteId}")]
        public async Task<IActionResult> Create(int uniteId, [FromBody] KisimCreateDto dto)
        {
            var data = await _service.CreateAsync(uniteId, dto);
            return Success(data);
        }

        // ---------------------------------------------------------
        // 4) KISIM GÜNCELLE
        // ---------------------------------------------------------
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] KisimCreateDto dto)
        {
            var data = await _service.UpdateAsync(id, dto);

            if (data == null)
                return Fail("Kısım bulunamadı.");

            return Success(data);
        }

        // ---------------------------------------------------------
        // 5) KISIM SİL
        // ---------------------------------------------------------
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.DeleteAsync(id);

            if (!ok)
                return Fail("Kısım bulunamadı.");

            return Success("Silindi.");
        }
        // -------------------------------------------------------
        // GET api/kisim/unite/{uniteId}
        // -------------------------------------------------------
        [HttpGet("unite/{uniteId:int}")]
        public async Task<IActionResult> GetByUnite(int uniteId)
        {
            var kisimRepo = _unitOfWork.Repository<Kisim>();

            var kisimlar = await kisimRepo.Query()
                .Where(x => x.UniteId == uniteId)
                .OrderBy(x => x.Sira)
                .ToListAsync();

            return Ok(new
            {
                status = true,
                message = "Kısımlar yüklendi",
                data = kisimlar
            });
        }
    }
}

