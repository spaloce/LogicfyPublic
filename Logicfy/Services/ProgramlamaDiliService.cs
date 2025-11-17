using AutoMapper;
using Logicfy.Data.UnitOfWork;
using Logicfy.Dtos.ProgramlamaDili;
using Logicfy.Models;
using Logicfy.Services.Interfaces;

namespace Logicfy.Services
{
    public class ProgramlamaDiliService : IProgramlamaDiliService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProgramlamaDiliService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<ProgramlamaDiliDto>> GetAllAsync()
        {
            var entities = await _unitOfWork.Repository<ProgramlamaDili>().GetAllAsync();
            return _mapper.Map<List<ProgramlamaDiliDto>>(entities);
        }

        public async Task<ProgramlamaDiliDto> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.Repository<ProgramlamaDili>().GetByIdAsync(id);
            return _mapper.Map<ProgramlamaDiliDto>(entity);
        }

        public async Task<ProgramlamaDiliDto> CreateAsync(ProgramlamaDiliCreateDto dto)
        {
            var entity = _mapper.Map<ProgramlamaDili>(dto);

            await _unitOfWork.Repository<ProgramlamaDili>().AddAsync(entity);
            await _unitOfWork.SaveAsync(); // Save işlemi burada

            return _mapper.Map<ProgramlamaDiliDto>(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var repo = _unitOfWork.Repository<ProgramlamaDili>();
            var entity = await repo.GetByIdAsync(id);

            if (entity == null)
                return false;

            repo.Remove(entity);
            await _unitOfWork.SaveAsync();

            return true;
        }
    }
}
