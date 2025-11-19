using Logicfy.Data.UnitOfWork;
using Logicfy.Models;
using Logicfy.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Logicfy.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : BaseController
    {
        private readonly IProgramlamaDiliService _dilService;
        private readonly IUniteService _uniteService;
        private readonly IKisimService _kisimService;
        private readonly IDersService _dersService;
        private readonly ISoruService _soruService;

        public DashboardController(
            IProgramlamaDiliService dilService,
            IUniteService uniteService,
            IKisimService kisimService,
            IDersService dersService,
            ISoruService soruService)
        {
            _dilService = dilService;
            _uniteService = uniteService;
            _kisimService = kisimService;
            _dersService = dersService;
            _soruService = soruService;
        }

        // ---------------------------------------------------------
        //   GET: api/dashboard
        // ---------------------------------------------------------
        [HttpGet]
        public async Task<IActionResult> GetDashboard()
        {
            try
            {
                // SERVİSLER SIRAYLA ÇAĞRILIYOR → DbContext çakışması olmaz
                var diller = await _dilService.GetAllAsync();
                var uniteler = await _uniteService.GetAllAsync();
                var kisimlar = await _kisimService.GetAllAsync();
                var dersler = await _dersService.GetAllAsync();
                var sorular = await _soruService.GetAllAsync();

                var result = new
                {
                    stats = new
                    {
                        toplamDil = diller.Count,
                        toplamUnite = uniteler.Count,
                        toplamKisim = kisimlar.Count,
                        toplamDers = dersler.Count,
                        toplamSoru = sorular.Count
                    },
                    recent = new
                    {
                        sonDil = diller.OrderByDescending(x => x.Id).FirstOrDefault(),
                        sonUnite = uniteler.OrderByDescending(x => x.Id).FirstOrDefault(),
                        sonDers = dersler.OrderByDescending(x => x.Id).FirstOrDefault(),
                        sonSoru = sorular.OrderByDescending(x => x.Id).FirstOrDefault()
                    }
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
