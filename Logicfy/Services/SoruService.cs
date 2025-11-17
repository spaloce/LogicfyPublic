using AutoMapper;
using Logicfy.Data.UnitOfWork;
using Logicfy.Dtos.Soru;
using Logicfy.Models;
using Logicfy.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Logicfy.Services
{
    public class SoruService : ISoruService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SoruService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // ---------------------------------------------------------
        // TÜM SORULAR (Admin)
        // ---------------------------------------------------------
        public async Task<List<SoruDto>> GetAllAsync()
        {
            var list = await _unitOfWork.Repository<Soru>()
                .Query()
                .Include(x => x.DogruCevap)
                .Include(x => x.Secenekler)
                .OrderBy(x => x.Id)
                .ToListAsync();

            return _mapper.Map<List<SoruDto>>(list);
        }

        // ---------------------------------------------------------
        // DERSİN TÜM SORULARI
        // ---------------------------------------------------------
        public async Task<List<SoruDto>> GetByDersIdAsync(int dersId)
        {
            var list = await _unitOfWork.Repository<Soru>()
                .Query()
                .Where(x => x.DersId == dersId)
                .Include(x => x.DogruCevap)
                .Include(x => x.Secenekler)
                .ToListAsync();

            return _mapper.Map<List<SoruDto>>(list);
        }

        // ---------------------------------------------------------
        // TEK SORU
        // ---------------------------------------------------------
        public async Task<SoruDto?> GetByIdAsync(int id)
        {
            var entity = await _unitOfWork.Repository<Soru>()
                .Query()
                .Include(x => x.Secenekler)
                .Include(x => x.DogruCevap)
                .FirstOrDefaultAsync(x => x.Id == id);

            return _mapper.Map<SoruDto>(entity);
        }

        // ---------------------------------------------------------
        // TIP 1 - ÇOKTAN SEÇMELİ OLUŞTUR
        // ---------------------------------------------------------
        public async Task<SoruDto> CreateTip1Async(int dersId, SoruTip1CreateDto dto)
        {
            var soruRepo = _unitOfWork.Repository<Soru>();
            var secenekRepo = _unitOfWork.Repository<SoruSecenek>();

            var soru = new Soru
            {
                DersId = dersId,
                SoruMetni = dto.SoruMetni,
                SoruTipi = 1
            };

            await soruRepo.AddAsync(soru);
            await _unitOfWork.SaveAsync();

            var secenekler = new List<SoruSecenek>();

            for (int i = 0; i < dto.Secenekler.Count; i++)
            {
                secenekler.Add(new SoruSecenek
                {
                    SoruId = soru.Id,
                    SecenekMetni = dto.Secenekler[i]
                });
            }

            await secenekRepo.AddRangeAsync(secenekler);
            await _unitOfWork.SaveAsync();

            soru.DogruCevapId = secenekler[dto.DogruIndex].Id;
            soruRepo.Update(soru);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<SoruDto>(soru);
        }

        // ---------------------------------------------------------
        // TIP 1 - GÜNCELLE
        // ---------------------------------------------------------
        public async Task<SoruDto?> UpdateTip1Async(int id, SoruTip1CreateDto dto)
        {
            var soruRepo = _unitOfWork.Repository<Soru>();
            var secenekRepo = _unitOfWork.Repository<SoruSecenek>();

            var soru = await soruRepo.Query()
                .Include(x => x.Secenekler)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (soru == null)
                return null;

            soru.SoruMetni = dto.SoruMetni;

            // önce eski seçenekleri sil
            foreach (var s in soru.Secenekler)
                secenekRepo.Remove(s);

            await _unitOfWork.SaveAsync();

            // yeni seçenekleri ekle
            var yeni = new List<SoruSecenek>();
            foreach (var sec in dto.Secenekler)
            {
                yeni.Add(new SoruSecenek
                {
                    SoruId = soru.Id,
                    SecenekMetni = sec
                });
            }

            await secenekRepo.AddRangeAsync(yeni);
            await _unitOfWork.SaveAsync();

            // doğru cevabı güncelle
            soru.DogruCevapId = yeni[dto.DogruIndex].Id;

            soruRepo.Update(soru);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<SoruDto>(soru);
        }

        // ---------------------------------------------------------
        // TIP 2 - OLUŞTUR
        // ---------------------------------------------------------
        public async Task<SoruDto> CreateTip2Async(int dersId, SoruTip2CreateDto dto)
        {
            var soru = new Soru
            {
                DersId = dersId,
                SoruMetni = dto.SoruMetni,
                SoruTipi = 2
            };

            await _unitOfWork.Repository<Soru>().AddAsync(soru);
            await _unitOfWork.SaveAsync();

            var kelime = new SoruKelimeBlok
            {
                SoruId = soru.Id,
                DogruKod = dto.DogruKod,
                KelimelerJson = JsonSerializer.Serialize(dto.Kelimeler)
            };

            await _unitOfWork.Repository<SoruKelimeBlok>().AddAsync(kelime);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<SoruDto>(soru);
        }

        // ---------------------------------------------------------
        // TIP 2 - GÜNCELLE
        // ---------------------------------------------------------
        public async Task<SoruDto?> UpdateTip2Async(int id, SoruTip2CreateDto dto)
        {
            var soru = await _unitOfWork.Repository<Soru>().GetByIdAsync(id);
            if (soru == null)
                return null;

            var kelimeRepo = _unitOfWork.Repository<SoruKelimeBlok>();
            var blok = await kelimeRepo.Query()
                .FirstOrDefaultAsync(x => x.SoruId == soru.Id);

            soru.SoruMetni = dto.SoruMetni;

            blok.DogruKod = dto.DogruKod;
            blok.KelimelerJson = JsonSerializer.Serialize(dto.Kelimeler);

            kelimeRepo.Update(blok);
            _unitOfWork.Repository<Soru>().Update(soru);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<SoruDto>(soru);
        }

        // ---------------------------------------------------------
        // TIP 3 - OLUŞTUR
        // ---------------------------------------------------------
        public async Task<SoruDto> CreateTip3Async(int dersId, SoruTip3CreateDto dto)
        {
            var soru = new Soru
            {
                DersId = dersId,
                SoruMetni = dto.SoruMetni,
                SoruTipi = 3
            };

            await _unitOfWork.Repository<Soru>().AddAsync(soru);
            await _unitOfWork.SaveAsync();

            var repo = _unitOfWork.Repository<SoruFonksiyonCozum>();

            foreach (var c in dto.DogruCozumler)
            {
                await repo.AddAsync(new SoruFonksiyonCozum
                {
                    SoruId = soru.Id,
                    CozumKod = c
                });
            }

            await _unitOfWork.SaveAsync();
            return _mapper.Map<SoruDto>(soru);
        }

        // ---------------------------------------------------------
        // TIP 3 - GÜNCELLE
        // ---------------------------------------------------------
        public async Task<SoruDto?> UpdateTip3Async(int id, SoruTip3CreateDto dto)
        {
            var soru = await _unitOfWork.Repository<Soru>().GetByIdAsync(id);
            if (soru == null)
                return null;

            soru.SoruMetni = dto.SoruMetni;

            var repo = _unitOfWork.Repository<SoruFonksiyonCozum>();
            var cozums = await repo.Query()
                .Where(x => x.SoruId == soru.Id)
                .ToListAsync();

            // eski çözümleri sil
            foreach (var c in cozums)
                repo.Remove(c);

            await _unitOfWork.SaveAsync();

            // yeni çözümler ekle
            foreach (var c in dto.DogruCozumler)
            {
                await repo.AddAsync(new SoruFonksiyonCozum
                {
                    SoruId = soru.Id,
                    CozumKod = c
                });
            }

            _unitOfWork.Repository<Soru>().Update(soru);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<SoruDto>(soru);
        }

        // ---------------------------------------------------------
        // TIP 4 - OLUŞTUR
        // ---------------------------------------------------------
        public async Task<SoruDto> CreateTip4Async(int dersId, SoruTip4CreateDto dto)
        {
            var soru = new Soru
            {
                DersId = dersId,
                SoruMetni = dto.SoruMetni,
                SoruTipi = 4
            };

            await _unitOfWork.Repository<Soru>().AddAsync(soru);
            await _unitOfWork.SaveAsync();

            var preview = new SoruCanliPreview
            {
                SoruId = soru.Id,
                DogruHtml = dto.DogruHtml,
                DogruCss = dto.DogruCss,
                GerekenEtiketlerJson = JsonSerializer.Serialize(dto.GerekenEtiketler),
                GerekenStillerJson = JsonSerializer.Serialize(dto.GerekenStiller)
            };

            await _unitOfWork.Repository<SoruCanliPreview>().AddAsync(preview);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<SoruDto>(soru);
        }

        // ---------------------------------------------------------
        // TIP 4 - GÜNCELLE
        // ---------------------------------------------------------
        public async Task<SoruDto?> UpdateTip4Async(int id, SoruTip4CreateDto dto)
        {
            var soru = await _unitOfWork.Repository<Soru>().GetByIdAsync(id);
            if (soru == null)
                return null;

            soru.SoruMetni = dto.SoruMetni;

            var repo = _unitOfWork.Repository<SoruCanliPreview>();
            var preview = await repo.Query()
                .FirstOrDefaultAsync(x => x.SoruId == soru.Id);

            preview.DogruHtml = dto.DogruHtml;
            preview.DogruCss = dto.DogruCss;
            preview.GerekenEtiketlerJson = JsonSerializer.Serialize(dto.GerekenEtiketler);
            preview.GerekenStillerJson = JsonSerializer.Serialize(dto.GerekenStiller);

            repo.Update(preview);
            _unitOfWork.Repository<Soru>().Update(soru);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<SoruDto>(soru);
        }

        // ---------------------------------------------------------
        // SİL
        // ---------------------------------------------------------
        public async Task<bool> DeleteAsync(int id)
        {
            var repo = _unitOfWork.Repository<Soru>();
            var entity = await repo.GetByIdAsync(id);

            if (entity == null)
                return false;

            repo.Remove(entity);
            await _unitOfWork.SaveAsync();

            return true;
        }
    }
}
