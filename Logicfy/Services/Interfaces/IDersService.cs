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
        Task<DersDto?> GetByIdAsync(int id);

        // OLUŞTUR (KisimId + DTO)
        Task<DersDto> CreateAsync(int kisimId, DersCreateDto dto);

        // GÜNCELLE
        Task<DersDto?> UpdateAsync(int id, DersCreateDto dto);

        // SİL
        Task<bool> DeleteAsync(int id);
    }
}
