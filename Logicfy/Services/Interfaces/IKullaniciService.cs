using Logicfy.Dtos;
using Logicfy.Dtos.Auth;
using Logicfy.Models;

namespace Logicfy.Services.Interfaces
{
    public interface IKullaniciService
    {
        Task<Kullanici> RegisterAsync(KullaniciRegisterDto dto);
        Task<Kullanici> LoginAsync(KullaniciLoginDto dto);
        Task<Kullanici> GetByIdAsync(string id);
    }
}
