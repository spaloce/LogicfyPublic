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
        Task<KisimDto?> GetByIdAsync(int id);

        // OLUŞTUR (UniteId + DTO)
        Task<KisimDto> CreateAsync(int uniteId, KisimCreateDto dto);

        // GÜNCELLE
        Task<KisimDto?> UpdateAsync(int id, KisimCreateDto dto);

        // SİL
        Task<bool> DeleteAsync(int id);
    }
}
