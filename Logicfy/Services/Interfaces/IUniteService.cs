using Logicfy.Dtos.Unite;

namespace Logicfy.Services.Interfaces
{
    public interface IUniteService
    {
        // TÜM ÜNİTELER
        Task<List<UniteDto>> GetAllAsync();

        // BİR DİLE AİT ÜNİTELER
        Task<List<UniteDto>> GetByDilIdAsync(int dilId);

        // TEK ÜNİTE
        Task<UniteDto?> GetByIdAsync(int id);

        // OLUŞTUR (DilId gerekli)
        Task<UniteDto> CreateAsync(int dilId, UniteCreateDto dto);

        // GÜNCELLE
        Task<UniteDto?> UpdateAsync(int id, UniteCreateDto dto);

        // SİL
        Task<bool> DeleteAsync(int id);
    }
}
