using Logicfy.Controllers;
using Logicfy.Dtos;
using Logicfy.Dtos.Kullanici;
using Logicfy.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Logicfy.Api.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : BaseController
    {
        private readonly IKullaniciService _kullaniciService;
        private readonly JwtTokenHelper _jwtTokenHelper;

        public AuthController(
            IKullaniciService kullaniciService,
            JwtTokenHelper jwtTokenHelper)
        {
            _kullaniciService = kullaniciService;
            _jwtTokenHelper = jwtTokenHelper;
        }

        // ---------------------------------------------------
        //  REGISTER
        // ---------------------------------------------------
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] KullaniciRegisterDto dto)
        {
            var user = await _kullaniciService.RegisterAsync(dto);

            return Ok(new
            {
                success = true,
                message = "Kayıt başarılı.",
                data = user
            });
        }

        // ---------------------------------------------------
        //  LOGIN
        // ---------------------------------------------------
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] KullaniciLoginDto dto)
        {
            var result = await _kullaniciService.LoginAsync(dto);

            // Token üret
            var token = _jwtTokenHelper.GenerateToken(
                result.Kullanici.Id,
                result.Kullanici.Email,
                result.Kullanici.AdSoyad
            );

            return Ok(new
            {
                success = true,
                token,
                user = result.Kullanici
            });
        }

        // ---------------------------------------------------
        //  ME (JWT'den Kullanıcıyı Çek)
        // ---------------------------------------------------
        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            var userId = GetUserId();

            var user = await _kullaniciService.GetByIdAsync(userId);

            return Ok(new
            {
                success = true,
                user
            });
        }
    }
}
