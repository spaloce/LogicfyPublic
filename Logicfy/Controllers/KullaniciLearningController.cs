using Logicfy.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Logicfy.Api.Controllers
{
    [Authorize]
    [Route("api/kullanici/learning")]
    [ApiController]
    public class KullaniciLearningController : ControllerBase
    {
        private readonly IKullaniciLearningPathService _service;

        public KullaniciLearningController(IKullaniciLearningPathService service)
        {
            _service = service;
        }

        private string GetUserId()
        {
            var claim = User.FindFirst("kullaniciId");
            if (claim == null)
                throw new Exception("Token içinde kullaniciId bulunamadı.");

            return claim.Value;
        }

        [HttpGet("dil/{dilId}")]
        public async Task<IActionResult> GetDilOverview(int dilId)
        {
            string kullaniciId = GetUserId();
            return Ok(await _service.GetDilOverviewAsync(kullaniciId, dilId));
        }

        [HttpGet("unite/{uniteId}")]
        public async Task<IActionResult> GetUniteOverview(int uniteId)
        {
            string kullaniciId = GetUserId();
            return Ok(await _service.GetUniteOverviewAsync(kullaniciId, uniteId));
        }

        [HttpGet("kisim/{kisimId}")]
        public async Task<IActionResult> GetKisimOverview(int kisimId)
        {
            string kullaniciId = GetUserId();
            return Ok(await _service.GetKisimOverviewAsync(kullaniciId, kisimId));
        }

        [HttpGet("ders/{dersId}")]
        public async Task<IActionResult> GetDersOverview(int dersId)
        {
            string kullaniciId = GetUserId();
            return Ok(await _service.GetDersOverviewAsync(kullaniciId, dersId));
        }
    }
}
