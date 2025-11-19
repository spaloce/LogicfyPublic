using AutoMapper;
using Logicfy.Data.UnitOfWork;
using Logicfy.Dtos.Kisim;
using Logicfy.Models;
using Logicfy.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

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

        // ---------------------------------------------------------
        // TÜM KISIMLAR
        // ---------------------------------------------------------
        public async Task<List<KisimDto>> GetAllAsync()
        {
            var entities = await _unitOfWork.Repository<Kisim>()
                .Query()
                .OrderBy(x => x.Sira)
                .ToListAsync();

            return _mapper.Map<List<KisimDto>>(entities);
        }

        // ---------------------------------------------------------
        // ÜNİTEYE AİT TÜM KISIMLAR
        // ---------------------------------------------------------
        public async Task<List<KisimDto>> GetByUniteIdAsync(int uniteId)
        {
            var entities = await _unitOfWork.Repository<Kisim>()
                .Query()
                .Where(x => x.UniteId == uniteId)
                .OrderBy(x => x.Sira)
                .ToListAsync();

            return _mapper.Map<List<KisimDto>>(entities);
        }

        // ---------------------------------------------------------
        // TEK KISIM
        // ---------------------------------------------------------
        public async Task<KisimDto?> GetByIdAsync(string id)
        {
            var entity = await _unitOfWork.Repository<Kisim>().GetByIdAsync(id);

            if (entity == null)
                return null;

            return _mapper.Map<KisimDto>(entity);
        }

        // ---------------------------------------------------------
        // KISIM OLUŞTUR
        // ---------------------------------------------------------
        public async Task<KisimDto> CreateAsync(int uniteId, KisimCreateDto dto)
        {
            var entity = new Kisim
            {
                UniteId = uniteId,
                Baslik = dto.Baslik,
                Sira = dto.Sira
            };

            await _unitOfWork.Repository<Kisim>().AddAsync(entity);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<KisimDto>(entity);
        }

        // ---------------------------------------------------------
        // KISIM GÜNCELLE
        // ---------------------------------------------------------
        public async Task<KisimDto?> UpdateAsync(string id, KisimCreateDto dto)
        {
            var repo = _unitOfWork.Repository<Kisim>();
            var entity = await repo.GetByIdAsync(id);

            if (entity == null)
                return null;

            entity.Baslik = dto.Baslik;
            entity.Sira = dto.Sira;

            repo.Update(entity);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<KisimDto>(entity);
        }

        // ---------------------------------------------------------
        // KISIM SİL
        // ---------------------------------------------------------
        public async Task<bool> DeleteAsync(string id)
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
