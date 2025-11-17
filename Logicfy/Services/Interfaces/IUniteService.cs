using Logicfy.Dtos.Unite;

namespace Logicfy.Services.Interfaces
{
    public interface IUniteService
    {
        Task<List<UniteDto>> GetAllAsync();
        Task<List<UniteDto>> GetByLanguageIdAsync(int languageId);
        Task<UniteDto> GetByIdAsync(int id);

        Task<UniteDto> CreateAsync(UniteCreateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
