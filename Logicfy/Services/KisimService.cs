using AutoMapper;
using Logicfy.Data.UnitOfWork;
using Logicfy.Dtos;
using Logicfy.Dtos.Kisim;
using Logicfy.Models;
using Logicfy.Services.Interfaces;

namespace Logicfy.Services
{
    public class KisimService : IKisimService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public KisimService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // Tüm kısımlar
        public async Task<List<KisimDto>> GetAllAsync()
        {
            var entities = await _unitOfWork.Repository<Kisim>().GetAllAsync();
            return _mapper.Map<List<KisimDto>>(entities);
        }

        // Belirli bir üniteye ait kısımlar
        public async Task<List<KisimDto>> GetByUniteIdAsync(int uniteId)
        {
            var entities = await _unitOfWork.Repository<Kisim>()
                .FindAsync(x => x.UniteId == uniteId);

            return _mapper.Map<List<KisimDto>>(entities);
        }

        // Id ile getirme
        public async Task<KisimDto> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.Repository<Kisim>().GetByIdAsync(id);
            return _mapper.Map<KisimDto>(entity);
        }

        // Yeni kısım oluştur
        public async Task<KisimDto> CreateAsync(KisimCreateDto dto)
        {
            var entity = _mapper.Map<Kisim>(dto);

            await _unitOfWork.Repository<Kisim>().AddAsync(entity);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<KisimDto>(entity);
        }

        // Silme
        public async Task<bool> DeleteAsync(int id)
        {
            var repo = _unitOfWork.Repository<Kisim>();
            var entity = await repo.GetByIdAsync(id);

            if (entity == null)
                return false;

            repo.Remove(entity);
            await _unitOfWork.SaveAsync();

            return true;
        }
    }
}
