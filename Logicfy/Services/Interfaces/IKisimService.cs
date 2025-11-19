using Logicfy.Dtos.Kisim;

namespace Logicfy.Services.Interfaces
{
    public interface IKisimService
    {
        // TÜM KISIMLAR
        Task<List<KisimDto>> GetAllAsync();

        // ÜNİTEYE AİT KISIMLAR
        Task<List<KisimDto>> GetByUniteIdAsync(int uniteId);

        // TEK KISIM
        Task<KisimDto?> GetByIdAsync(string id);

        // OLUŞTUR (UniteId + DTO)
        Task<KisimDto> CreateAsync(int uniteId, KisimCreateDto dto);

        // GÜNCELLE
        Task<KisimDto?> UpdateAsync(string id, KisimCreateDto dto);

        // SİL
        Task<bool> DeleteAsync(string id);
    }
}
