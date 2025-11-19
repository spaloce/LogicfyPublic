using Logicfy.Dtos.Ders;

namespace Logicfy.Services.Interfaces
{
    public interface IDersService
    {
        // TÜM DERSLER
        Task<List<DersDto>> GetAllAsync();

        // BİR KISIMA AİT DERSLER
        Task<List<DersDto>> GetByKisimIdAsync(int kisimId);

        // TEK DERS
        Task<DersDto?> GetByIdAsync(string id);

        // OLUŞTUR (KisimId + DTO)
        Task<DersDto> CreateAsync(int kisimId, DersCreateDto dto);

        // GÜNCELLE
        Task<DersDto?> UpdateAsync(string id, DersCreateDto dto);

        // SİL
        Task<bool> DeleteAsync(string id);
    }
}
