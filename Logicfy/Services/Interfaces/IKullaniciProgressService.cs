using System.Threading.Tasks;

namespace Logicfy.Services.Interfaces
{
    public interface IKullaniciProgressService
    {
        Task SoruCevaplaAsync(int kullaniciId, int soruId, bool dogruMu, string cevapJson, int sureMs);
    }
}
