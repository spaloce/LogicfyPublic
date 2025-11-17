using Logicfy.Dtos;
using Logicfy.Dtos.Soru;

namespace Logicfy.Services.Interfaces
{
    public interface ISoruService
    {
        // ---------------------------------------------------------
        // LİSTELEME
        // ---------------------------------------------------------
        Task<List<SoruDto>> GetAllAsync();                   // Admin için
        Task<List<SoruDto>> GetByDersIdAsync(int dersId);   // Müfredat için
        Task<SoruDto?> GetByIdAsync(int id);

        // ---------------------------------------------------------
        // OLUŞTURMA (Tip Bazlı)
        // ---------------------------------------------------------
        Task<SoruDto> CreateTip1Async(int dersId, SoruTip1CreateDto dto);
        Task<SoruDto> CreateTip2Async(int dersId, SoruTip2CreateDto dto);
        Task<SoruDto> CreateTip3Async(int dersId, SoruTip3CreateDto dto);
        Task<SoruDto> CreateTip4Async(int dersId, SoruTip4CreateDto dto);

        // ---------------------------------------------------------
        // GÜNCELLEME (Tip Bazlı)
        // ---------------------------------------------------------
        Task<SoruDto?> UpdateTip1Async(int id, SoruTip1CreateDto dto);
        Task<SoruDto?> UpdateTip2Async(int id, SoruTip2CreateDto dto);
        Task<SoruDto?> UpdateTip3Async(int id, SoruTip3CreateDto dto);
        Task<SoruDto?> UpdateTip4Async(int id, SoruTip4CreateDto dto);

        // ---------------------------------------------------------
        // SİLME
        // ---------------------------------------------------------
        Task<bool> DeleteAsync(int id);
    }
}
