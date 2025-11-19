using Logicfy.Models;

namespace Logicfy.Services.Interfaces
{
    public interface IKullaniciProgressService
    {
        Task SoruCevaplaAsync(string kullaniciId, int soruId, bool dogruMu, string cevapJson, int sureMs);

        Task<KullaniciDersIlerleme?> GetDersProgressAsync(string kullaniciId, int dersId);
        Task<KullaniciKisimProgress?> GetKisimProgressAsync(string kullaniciId, int kisimId);
        Task<KullaniciUnitProgress?> GetUniteProgressAsync(string kullaniciId, int uniteId);
    }
}
