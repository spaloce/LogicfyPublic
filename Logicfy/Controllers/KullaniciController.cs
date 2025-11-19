using Logicfy.Data.UnitOfWork;
using Logicfy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Logicfy.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class KullaniciController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public KullaniciController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // -----------------------------------------------------
        //  GET: api/kullanici
        //  Kullanıcı listesi (paging + arama)
        // -----------------------------------------------------
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string? search = null)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 20;

            var repo = _unitOfWork.Repository<Kullanici>();

            var query = repo.Query();

            // Arama: Ad, Soyad, Email üzerinde basit filtre
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim().ToLower();
                query = query.Where(x =>
                    x.AdSoyad.ToLower().Contains(search) ||
                    x.Email.ToLower().Contains(search));
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(x => x.AdSoyad)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new KullaniciListDto
                {
                    Id = x.Id,
                    Ad = x.AdSoyad,
                    Email = x.Email,
                    XP = x.XP,
                    Seviye = x.Seviye,
                    OlusturmaTarihi = x.KayitTarihi
                })
                .ToListAsync();

            var result = new
            {
                page,
                pageSize,
                totalCount,
                totalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                items
            };

            return Ok(result);
        }

        // -----------------------------------------------------
        //  GET: api/kullanici/{id}
        //  Tek kullanıcı detayı
        // -----------------------------------------------------
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(string id)
        {
            var repo = _unitOfWork.Repository<Kullanici>();

            var user = await repo.Query()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (user == null)
                return Ok("Kullanıcı bulunamadı.");

            var dto = new KullaniciDetailDto
            {
                Id = user.Id,
                Ad = user.AdSoyad,
                Email = user.Email,
                XP = user.XP,
                Seviye = user.Seviye,
                OlusturmaTarihi = user.KayitTarihi,
                // ihtiyaca göre extra alanlar:
                // SonGirisTarihi = user.LastLoginAt,
                // AktifMi = user.IsActive
            };

            return Ok(dto);
        }
    }

    // ---------------------------------------------------------
    //  DTO'lar (entity'i direkt dışarı vermeyelim)
    // ---------------------------------------------------------
    public class KullaniciListDto
    {
        public string Id { get; set; }
        public string Ad { get; set; } = string.Empty;
        public string Soyad { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int XP { get; set; }
        public int Seviye { get; set; }
        public DateTime? OlusturmaTarihi { get; set; }
    }

    public class KullaniciDetailDto : KullaniciListDto
    {
        // İleriye dönük ek alanları burada tutabilirsin
        // public DateTime? SonGirisTarihi { get; set; }
        // public bool AktifMi { get; set; }
    }
}
