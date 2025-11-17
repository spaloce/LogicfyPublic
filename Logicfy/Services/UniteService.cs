using AutoMapper;
using Logicfy.Data.UnitOfWork;
using Logicfy.Dtos.Unite;
using Logicfy.Models;
using Logicfy.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

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

        // ---------------------------------------------------------
        // TÜM ÜNİTELER
        // ---------------------------------------------------------
        public async Task<List<UniteDto>> GetAllAsync()
        {
            var entities = await _unitOfWork.Repository<Unite>()
                .Query()
                .OrderBy(x => x.Sira)
                .ToListAsync();

            return _mapper.Map<List<UniteDto>>(entities);
        }

        // ---------------------------------------------------------
        // BİR DİLE AİT ÜNİTELER
        // ---------------------------------------------------------
        public async Task<List<UniteDto>> GetByDilIdAsync(int dilId)
        {
            var entities = await _unitOfWork.Repository<Unite>()
                .Query()
                .Where(x => x.ProgramlamaDiliId == dilId)
                .OrderBy(x => x.Sira)
                .ToListAsync();

            return _mapper.Map<List<UniteDto>>(entities);
        }

        // ---------------------------------------------------------
        // TEK ÜNİTE GETİR
        // ---------------------------------------------------------
        public async Task<UniteDto?> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.Repository<Unite>().GetByIdAsync(id);

            if (entity == null)
                return null;

            return _mapper.Map<UniteDto>(entity);
        }

        // ---------------------------------------------------------
        // ÜNİTE OLUŞTUR (DilId + DTO)
        // ---------------------------------------------------------
        public async Task<UniteDto> CreateAsync(int dilId, UniteCreateDto dto)
        {
            var entity = new Unite
            {
                ProgramlamaDiliId = dilId,
                Baslik = dto.Baslik,
                Sira = dto.Sira
            };

            await _unitOfWork.Repository<Unite>().AddAsync(entity);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<UniteDto>(entity);
        }

        // ---------------------------------------------------------
        // ÜNİTE GÜNCELLE
        // ---------------------------------------------------------
        public async Task<UniteDto?> UpdateAsync(int id, UniteCreateDto dto)
        {
            var repo = _unitOfWork.Repository<Unite>();
            var entity = await repo.GetByIdAsync(id);

            if (entity == null)
                return null;

            entity.Baslik = dto.Baslik;
            entity.Sira = dto.Sira;

            repo.Update(entity);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<UniteDto>(entity);
        }

        // ---------------------------------------------------------
        // ÜNİTE SİL
        // ---------------------------------------------------------
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
