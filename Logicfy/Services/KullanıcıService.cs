using AutoMapper;
using Logicfy.Data.UnitOfWork;
using Logicfy.Dtos;
using Logicfy.Dtos.Kullanici;
using Logicfy.Models;
using Logicfy.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Logicfy.Services
{
    public class KullaniciService : IKullaniciService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public KullaniciService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration config)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _config = config;
        }

        // ---------------------------------------------------------------
        // REGISTER
        // ---------------------------------------------------------------
        public async Task<KullaniciDto> RegisterAsync(KullaniciRegisterDto dto)
        {
            var repo = _unitOfWork.Repository<Kullanici>();

            var email = dto.Email.Trim().ToLower();

            // Email zaten var mı?
            bool exists = await repo
                .Query()
                .AnyAsync(x => x.Email.ToLower() == email);

            if (exists)
                throw new Exception("Bu e-posta ile kayıtlı bir kullanıcı zaten var.");

            var entity = new Kullanici
            {
                AdSoyad = dto.AdSoyad,
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Sifre),
                XP = 0,
                Seviye = 1,
                Streak = 0,
                KayitTarihi = DateTime.UtcNow,
                SonGirisTarihi = null
            };

            await repo.AddAsync(entity);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<KullaniciDto>(entity);
        }

        // ---------------------------------------------------------------
        // LOGIN
        // ---------------------------------------------------------------
        public async Task<KullaniciLoginResultDto> LoginAsync(KullaniciLoginDto dto)
        {
            var repo = _unitOfWork.Repository<Kullanici>();
            var email = dto.Email.Trim().ToLower();

            var user = await repo
                .Query()
                .FirstOrDefaultAsync(x => x.Email.ToLower() == email);

            if (user == null)
                throw new Exception("Kullanıcı bulunamadı.");

            bool sifreDogru = BCrypt.Net.BCrypt.Verify(dto.Sifre, user.PasswordHash);
            if (!sifreDogru)
                throw new Exception("Şifre hatalı.");

            // Günlük seri (streak) ve SonGirisTarihi güncelle
            await UpdateDailyStreakAsync(user);

            user.SonGirisTarihi = DateTime.UtcNow;
            _unitOfWork.Repository<Kullanici>().Update(user);

            await _unitOfWork.SaveAsync();

            // JWT token üret
            var token = GenerateJwtToken(user);

            return new KullaniciLoginResultDto
            {
                Token = token,
                Kullanici = _mapper.Map<KullaniciDto>(user)
            };
        }

        // ---------------------------------------------------------------
        // GET BY ID
        // ---------------------------------------------------------------
        public async Task<KullaniciDto> GetByIdAsync(int id)
        {
            var user = await _unitOfWork.Repository<Kullanici>().GetByIdAsync(id);
            return _mapper.Map<KullaniciDto>(user);
        }

        // ---------------------------------------------------------------
        // GÜNLÜK SERİ (STREAK) GÜNCELLEME
        // ---------------------------------------------------------------
        private async Task UpdateDailyStreakAsync(Kullanici user)
        {
            var seriRepo = _unitOfWork.Repository<KullaniciGunlukSeri>();

            var seri = await seriRepo.Query()
                .FirstOrDefaultAsync(x => x.KullaniciId == user.Id);

            var today = DateTime.UtcNow.Date;

            if (seri == null)
            {
                // İlk kez giriş yapıyor
                seri = new KullaniciGunlukSeri
                {
                    KullaniciId = user.Id,
                    SeriSayisi = 1,
                    SonGiris = DateTime.UtcNow
                };

                await seriRepo.AddAsync(seri);
                user.Streak = 1;
            }
            else
            {
                var last = seri.SonGiris.Date;

                if (today == last)
                {
                    // Aynı gün tekrar giriş -> streak değişmez
                }
                else if (today == last.AddDays(1))
                {
                    // Seri devam ediyor
                    seri.SeriSayisi += 1;
                    seri.SonGiris = DateTime.UtcNow;
                    seriRepo.Update(seri);

                    user.Streak = seri.SeriSayisi;
                }
                else
                {
                    // Seri bozulmuş, sıfırdan başla
                    seri.SeriSayisi = 1;
                    seri.SonGiris = DateTime.UtcNow;
                    seriRepo.Update(seri);

                    user.Streak = 1;
                }
            }
        }

        // ---------------------------------------------------------------
        // JWT TOKEN ÜRETİMİ
        // ---------------------------------------------------------------
        private string GenerateJwtToken(Kullanici user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("id", user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("adSoyad", user.AdSoyad ?? string.Empty)
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(int.Parse(_config["Jwt:ExpiresMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
