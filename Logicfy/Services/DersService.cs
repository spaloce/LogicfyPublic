using AutoMapper;
using Logicfy.Data.UnitOfWork;
using Logicfy.Dtos.Ders;
using Logicfy.Models;
using Logicfy.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Logicfy.Services
{
    public class DersService : IDersService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DersService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // ---------------------------------------------------------
        // TÜM DERSLER
        // ---------------------------------------------------------
        public async Task<List<DersDto>> GetAllAsync()
        {
            var list = await _unitOfWork.Repository<Ders>()
                .Query()
                .OrderBy(x => x.Sira)
                .ToListAsync();

            return _mapper.Map<List<DersDto>>(list);
        }

        // ---------------------------------------------------------
        // KISIMA AİT DERSLER
        // ---------------------------------------------------------
        public async Task<List<DersDto>> GetByKisimIdAsync(int kisimId)
        {
            var list = await _unitOfWork.Repository<Ders>()
                .Query()
                .Where(x => x.KisimId == kisimId)
                .OrderBy(x => x.Sira)
                .ToListAsync();

            return _mapper.Map<List<DersDto>>(list);
        }

        // ---------------------------------------------------------
        // TEK DERS
        // ---------------------------------------------------------
        public async Task<DersDto?> GetByIdAsync(string id)
        {
            var entity = await _unitOfWork.Repository<Ders>().GetByIdAsync(id);

            if (entity == null)
                return null;

            return _mapper.Map<DersDto>(entity);
        }

        // ---------------------------------------------------------
        // DERS OLUŞTUR
        // ---------------------------------------------------------
        public async Task<DersDto> CreateAsync(int kisimId, DersCreateDto dto)
        {
            var entity = new Ders
            {
                KisimId = kisimId,
                Baslik = dto.Baslik,
                Sira = dto.Sira,
                TahminiSure = dto.TahminiSure,
                SoruSayisiCache = 0,
                ZorlukSeviyesi = dto.ZorlukSeviyesi
            };

            await _unitOfWork.Repository<Ders>().AddAsync(entity);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<DersDto>(entity);
        }

        // ---------------------------------------------------------
        // DERS GÜNCELLE
        // ---------------------------------------------------------
        public async Task<DersDto?> UpdateAsync(string id, DersCreateDto dto)
        {
            var repo = _unitOfWork.Repository<Ders>();
            var entity = await repo.GetByIdAsync(id);

            if (entity == null)
                return null;

            entity.Baslik = dto.Baslik;
            entity.Sira = dto.Sira;
            entity.TahminiSure = dto.TahminiSure;
            entity.ZorlukSeviyesi = dto.ZorlukSeviyesi;

            repo.Update(entity);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<DersDto>(entity);
        }

        // ---------------------------------------------------------
        // DERS SİL
        // ---------------------------------------------------------
        public async Task<bool> DeleteAsync(string id)
        {
            var repo = _unitOfWork.Repository<Ders>();
            var entity = await repo.GetByIdAsync(id);

            if (entity == null)
                return false;

            repo.Remove(entity);
            await _unitOfWork.SaveAsync();
            return true;
        }
    }
}
