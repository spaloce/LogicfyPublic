using Logicfy.Models;

namespace Logicfy.Services.Interfaces
{
    public interface IKullaniciProgressService
    {
        Task SoruCevaplaAsync(int kullaniciId, int soruId, bool dogruMu, string cevapJson, int sureMs);

        Task<KullaniciDersIlerleme?> GetDersProgressAsync(int kullaniciId, int dersId);
        Task<KullaniciKisimProgress?> GetKisimProgressAsync(int kullaniciId, int kisimId);
        Task<KullaniciUnitProgress?> GetUniteProgressAsync(int kullaniciId, int uniteId);
    }
}
