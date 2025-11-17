using AutoMapper;
using Logicfy.Data.UnitOfWork;
using Logicfy.Dtos.Unite;
using Logicfy.Models;
using Logicfy.Services.Interfaces;

namespace Logicfy.Services
{
    public class UniteService : IUniteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UniteService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // Tüm üniteleri getir
        public async Task<List<UniteDto>> GetAllAsync()
        {
            var entities = await _unitOfWork.Repository<Unite>().GetAllAsync();
            return _mapper.Map<List<UniteDto>>(entities);
        }

        // Belirli bir programlama diline ait üniteler
        public async Task<List<UniteDto>> GetByLanguageIdAsync(int languageId)
        {
            var entities = await _unitOfWork.Repository<Unite>()
                .FindAsync(x => x.ProgramlamaDiliId == languageId);

            return _mapper.Map<List<UniteDto>>(entities);
        }

        // Id ile getirme
        public async Task<UniteDto> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.Repository<Unite>().GetByIdAsync(id);
            return _mapper.Map<UniteDto>(entity);
        }

        // Yeni ünite oluştur
        public async Task<UniteDto> CreateAsync(UniteCreateDto dto)
        {
            var entity = _mapper.Map<Unite>(dto);

            await _unitOfWork.Repository<Unite>().AddAsync(entity);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<UniteDto>(entity);
        }

        // Silme işlemi
        public async Task<bool> DeleteAsync(int id)
        {
            var repo = _unitOfWork.Repository<Unite>();
            var entity = await repo.GetByIdAsync(id);

            if (entity == null)
                return false;

            repo.Remove(entity);
            await _unitOfWork.SaveAsync();
            return true;
        }
    }
}
