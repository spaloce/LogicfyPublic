using Logicfy.Dtos;
using Logicfy.Dtos.Ders;

namespace Logicfy.Services.Interfaces
{
    public interface IDersService
    {
        Task<List<DersDto>> GetAllAsync();
        Task<List<DersDto>> GetByKisimIdAsync(int kisimId);
        Task<DersDto> GetByIdAsync(int id);

        Task<DersDto> CreateAsync(DersCreateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
