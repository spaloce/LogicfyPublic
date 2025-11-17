using AutoMapper;
using Logicfy.Data.UnitOfWork;
using Logicfy.Dtos;
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

        // Tüm dersler
        public async Task<List<DersDto>> GetAllAsync()
        {
            var entities = await _unitOfWork.Repository<Ders>()
                .Query()
                .Include(x => x.Sorular)
                .ToListAsync();

            return _mapper.Map<List<DersDto>>(entities);
        }

        // Belirli bir kısma ait dersler
        public async Task<List<DersDto>> GetByKisimIdAsync(int kisimId)
        {
            var entities = await _unitOfWork.Repository<Ders>()
                .Query()
                .Where(x => x.KisimId == kisimId)
                .Include(x => x.Sorular)
                .ToListAsync();

            return _mapper.Map<List<DersDto>>(entities);
        }

        // Id ile getirme
        public async Task<DersDto> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.Repository<Ders>()
                .Query()
                .Include(x => x.Sorular)
                .FirstOrDefaultAsync(x => x.Id == id);

            return _mapper.Map<DersDto>(entity);
        }

        // Yeni ders ekleme
        public async Task<DersDto> CreateAsync(DersCreateDto dto)
        {
            var entity = _mapper.Map<Ders>(dto);

            await _unitOfWork.Repository<Ders>().AddAsync(entity);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<DersDto>(entity);
        }

        // Silme
        public async Task<bool> DeleteAsync(int id)
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
