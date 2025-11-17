using AutoMapper;
using Logicfy.Data.UnitOfWork;
using Logicfy.Dtos;
using Logicfy.Dtos.Kullanici;
using Logicfy.Models;
using Logicfy.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Logicfy.Services
{
    public class KullaniciService : IKullaniciService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public KullaniciService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // ---------------------------------------------------------
        //   REGISTER
        // ---------------------------------------------------------
        public async Task<KullaniciDto> RegisterAsync(KullaniciRegisterDto dto)
        {
            var repo = _unitOfWork.Repository<Kullanici>();

            var email = dto.Email.Trim().ToLower();

            bool exists = await repo.Query().AnyAsync(x => x.Email == email);
            if (exists)
                throw new Exception("Bu e-posta zaten kayıtlı.");

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

        // ---------------------------------------------------------
        //   LOGIN
        // ---------------------------------------------------------
        public async Task<KullaniciLoginResultDto> LoginAsync(KullaniciLoginDto dto)
        {
            var repo = _unitOfWork.Repository<Kullanici>();
            var email = dto.Email.Trim().ToLower();

            var user = await repo.Query().FirstOrDefaultAsync(x => x.Email == email);
            if (user == null)
                throw new Exception("Kullanıcı bulunamadı.");

            bool validPassword = BCrypt.Net.BCrypt.Verify(dto.Sifre, user.PasswordHash);
            if (!validPassword)
                throw new Exception("Şifre hatalı.");

            // günlük streak güncelle
            await UpdateDailyStreak(user);

            user.SonGirisTarihi = DateTime.UtcNow;
            repo.Update(user);
            await _unitOfWork.SaveAsync();

            return new KullaniciLoginResultDto
            {
                Kullanici = _mapper.Map<KullaniciDto>(user)
            };
        }

        // ---------------------------------------------------------
        //   GET BY ID
        // ---------------------------------------------------------
        public async Task<KullaniciDto> GetByIdAsync(int id)
        {
            var user = await _unitOfWork.Repository<Kullanici>().GetByIdAsync(id);
            return _mapper.Map<KullaniciDto>(user);
        }

        // ---------------------------------------------------------
        //   STREAK GÜNCELLEME
        // ---------------------------------------------------------
        private async Task UpdateDailyStreak(Kullanici user)
        {
            var seriRepo = _unitOfWork.Repository<KullaniciGunlukSeri>();
            var seri = await seriRepo.Query().FirstOrDefaultAsync(x => x.KullaniciId == user.Id);

            var today = DateTime.UtcNow.Date;

            if (seri == null)
            {
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
                    // aynı gün giriş, streak değişmez
                }
                else if (today == last.AddDays(1))
                {
                    seri.SeriSayisi += 1;
                    seri.SonGiris = DateTime.UtcNow;

                    seriRepo.Update(seri);
                    user.Streak = seri.SeriSayisi;
                }
                else
                {
                    seri.SeriSayisi = 1;
                    seri.SonGiris = DateTime.UtcNow;

                    seriRepo.Update(seri);
                    user.Streak = 1;
                }
            }
        }
    }
}
