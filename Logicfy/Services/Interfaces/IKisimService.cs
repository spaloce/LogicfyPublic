using Logicfy.Dtos;
using Logicfy.Dtos.Kisim;

namespace Logicfy.Services.Interfaces
{
    public interface IKisimService
    {
        Task<List<KisimDto>> GetAllAsync();
        Task<List<KisimDto>> GetByUniteIdAsync(int uniteId);
        Task<KisimDto> GetByIdAsync(int id);

        Task<KisimDto> CreateAsync(KisimCreateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
