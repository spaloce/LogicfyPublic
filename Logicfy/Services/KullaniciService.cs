using Logicfy.Dtos.Auth;
using Logicfy.Models;
using Logicfy.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Logicfy.Services
{
    public class KullaniciService : IKullaniciService
    {
        private readonly UserManager<Kullanici> _userManager;

        public KullaniciService(UserManager<Kullanici> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Kullanici> RegisterAsync(KullaniciRegisterDto dto)
        {
            var user = new Kullanici
            {
                UserName = dto.Email,
                Email = dto.Email,
                AdSoyad = dto.AdSoyad,
                KayitTarihi = DateTime.Now,
                Rol = "User"
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                throw new Exception(string.Join(" | ", result.Errors.Select(x => x.Description)));

            return user;
        }

        public async Task<Kullanici> LoginAsync(KullaniciLoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null)
                throw new Exception("Kullanıcı bulunamadı.");

            var valid = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!valid)
                throw new Exception("Şifre hatalı.");

            return user;
        }

        public async Task<Kullanici> GetByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }
    }
}
