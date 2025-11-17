using Logicfy.Dtos;
using Logicfy.Dtos.Soru;

namespace Logicfy.Services.Interfaces
{
    public interface ISoruService
    {
        Task<List<SoruDto>> GetByDersIdAsync(int dersId);
        Task<SoruDto> GetByIdAsync(int id);

        Task<SoruDto> CreateTip1Async(int dersId, SoruTip1CreateDto dto);
        Task<SoruDto> CreateTip2Async(int dersId, SoruTip2CreateDto dto);
        Task<SoruDto> CreateTip3Async(int dersId, SoruTip3CreateDto dto);
        Task<SoruDto> CreateTip4Async(int dersId, SoruTip4CreateDto dto);

        Task<bool> DeleteAsync(int id);
    }
}
