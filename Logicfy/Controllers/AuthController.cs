using Logicfy.Controllers;
using Logicfy.Dtos.Auth;
using Logicfy.Models;
using Logicfy.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Logicfy.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly IKullaniciService _kullaniciService;
        private readonly JwtTokenHelper _jwtTokenHelper;
        private readonly SignInManager<Kullanici> _signInManager;

        public AuthController(
            IKullaniciService kullaniciService,
            JwtTokenHelper jwtTokenHelper,
            SignInManager<Kullanici> signInManager)
        {
            _kullaniciService = kullaniciService;
            _jwtTokenHelper = jwtTokenHelper;
            _signInManager = signInManager;
        }

        // -----------------------------------------
        // REGISTER
        // -----------------------------------------
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] KullaniciRegisterDto dto)
        {
            var user = await _kullaniciService.RegisterAsync(dto);

            return Ok(new
            {
                Ok = true,
                user = new
                {
                    user.Id,
                    user.Email,
                    user.AdSoyad
                }
            });
        }

        // -----------------------------------------
        // LOGIN (Cookie + JWT)
        // -----------------------------------------
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] KullaniciLoginDto dto)
        {
            var user = await _kullaniciService.LoginAsync(dto);

            // Cookie login
            await _signInManager.SignInAsync(user, isPersistent: true);

            // JWT
            var token = _jwtTokenHelper.GenerateToken(
                user.Id,
                user.Email,
                user.AdSoyad,
                user.Rol
            );

            return Ok(new
            {
                Ok = true,
                token,
                user = new
                {
                    user.Id,
                    user.Email,
                    user.AdSoyad,
                    user.Rol,
                    user.XP,
                    user.Seviye
                }
            });
        }

        // -----------------------------------------
        // ME (JWT ile kullanıcı bilgisi)
        // -----------------------------------------
        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            var userId = GetUserId(); // string

            var user = await _kullaniciService.GetByIdAsync(userId);

            return Ok(new
            {
                Ok = true,
                user = new
                {
                    user.Id,
                    user.Email,
                    user.AdSoyad,
                    user.Rol,
                    user.XP,
                    user.Seviye,
                    user.Streak
                }
            });
        }

        // -----------------------------------------
        // LOGOUT
        // -----------------------------------------
        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return Ok(new { Ok = true });
        }
    }
}
