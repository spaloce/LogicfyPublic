using Logicfy.Dtos.ProgramlamaDili;
using Logicfy.Dtos;

namespace Logicfy.Services.Interfaces
{
    public interface IProgramlamaDiliService
    {
        Task<List<ProgramlamaDiliDto>> GetAllAsync();
        Task<ProgramlamaDiliDto> GetByIdAsync(int id);

        Task<ProgramlamaDiliDto> CreateAsync(ProgramlamaDiliCreateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
