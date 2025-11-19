using Logicfy.Dtos.LearningPath;

namespace Logicfy.Services.Interfaces
{
    public interface IKullaniciLearningPathService
    {
        Task<KullaniciDilOverviewDto> GetDilOverviewAsync(string kullaniciId, int dilId);
        Task<KullaniciUniteOverviewDto> GetUniteOverviewAsync(string kullaniciId, int uniteId);
        Task<KullaniciKisimOverviewDto> GetKisimOverviewAsync(string kullaniciId, int kisimId);
        Task<KullaniciDersOverviewDto> GetDersOverviewAsync(string kullaniciId, int dersId);

        Task SetAktifDersAsync(string kullaniciId, int dersId);
        Task<int?> GetAktifDersAsync(string kullaniciId);
    }
}
