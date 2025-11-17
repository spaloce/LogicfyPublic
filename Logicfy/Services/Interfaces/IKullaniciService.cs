using Logicfy.Dtos;
using Logicfy.Dtos.Kullanici;

namespace Logicfy.Services.Interfaces
{
    public interface IKullaniciService
    {
        Task<KullaniciDto> RegisterAsync(KullaniciRegisterDto dto);
        Task<KullaniciLoginResultDto> LoginAsync(KullaniciLoginDto dto);
        Task<KullaniciDto> GetByIdAsync(int id);
    }
}
