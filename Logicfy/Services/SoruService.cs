using AutoMapper;
using Logicfy.Data.UnitOfWork;
using Logicfy.Dtos;
using Logicfy.Dtos.Soru;
using Logicfy.Models;
using Logicfy.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

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

        // -------------------------------------------------------------------
        //  SORU LİSTELEME
        // -------------------------------------------------------------------

        public async Task<List<SoruDto>> GetByDersIdAsync(int dersId)
        {
            var sorular = await _unitOfWork.Repository<Soru>()
                .Query()
                .Where(x => x.DersId == dersId)
                .Include(x => x.Secenekler)
                .Include(x => x.DogruCevap)
                .ToListAsync();

            var list = new List<SoruDto>();

            foreach (var soru in sorular)
            {
                var dto = _mapper.Map<SoruDto>(soru);

                // TIP 2
                if (soru.SoruTipi == 2)
                {
                    var kelime = await _unitOfWork.Repository<SoruKelimeBlok>()
                        .Query()
                        .FirstOrDefaultAsync(x => x.SoruId == soru.Id);

                    if (kelime != null)
                    {
                        dto.Tip2 = new Tip2Dto
                        {
                            Id = kelime.Id,
                            DogruKod = kelime.DogruKod,
                            Kelimeler = System.Text.Json.JsonSerializer.Deserialize<List<string>>(kelime.KelimelerJson)
                        };
                    }
                }

                // TIP 3
                if (soru.SoruTipi == 3)
                {
                    var cozumler = await _unitOfWork.Repository<SoruFonksiyonCozum>()
                        .Query()
                        .Where(x => x.SoruId == soru.Id)
                        .ToListAsync();

                    dto.Tip3 = cozumler.Select(c => new Tip3CozumDto
                    {
                        Id = c.Id,
                        CozumKod = c.CozumKod
                    }).ToList();
                }

                // TIP 4
                if (soru.SoruTipi == 4)
                {
                    var preview = await _unitOfWork.Repository<SoruCanliPreview>()
                        .Query()
                        .FirstOrDefaultAsync(x => x.SoruId == soru.Id);

                    if (preview != null)
                    {
                        dto.Tip4 = new Tip4Dto
                        {
                            Id = preview.Id,
                            DogruHtml = preview.DogruHtml,
                            DogruCss = preview.DogruCss,
                            GerekenEtiketler = System.Text.Json.JsonSerializer.Deserialize<List<string>>(preview.GerekenEtiketlerJson),
                            GerekenStiller = System.Text.Json.JsonSerializer.Deserialize<List<string>>(preview.GerekenStillerJson)
                        };
                    }
                }

                list.Add(dto);
            }

            return list;
        }


        public async Task<SoruDto> GetByIdAsync(int id)
        {
            var soru = await _unitOfWork.Repository<Soru>()
                .Query()
                .Include(x => x.Secenekler)
                .Include(x => x.DogruCevap)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (soru == null)
                return null;

            var dto = _mapper.Map<SoruDto>(soru);

            // TIP 2 – Kelime Blok
            if (soru.SoruTipi == 2)
            {
                var kelime = await _unitOfWork.Repository<SoruKelimeBlok>()
                    .Query()
                    .FirstOrDefaultAsync(x => x.SoruId == soru.Id);

                if (kelime != null)
                {
                    dto.Tip2 = new Tip2Dto
                    {
                        Id = kelime.Id,
                        DogruKod = kelime.DogruKod,
                        Kelimeler = System.Text.Json.JsonSerializer.Deserialize<List<string>>(kelime.KelimelerJson)
                    };
                }
            }

            // TIP 3 – Fonksiyon Çözümleri
            if (soru.SoruTipi == 3)
            {
                var cozumler = await _unitOfWork.Repository<SoruFonksiyonCozum>()
                    .Query()
                    .Where(x => x.SoruId == soru.Id)
                    .ToListAsync();

                dto.Tip3 = cozumler
                    .Select(x => new Tip3CozumDto
                    {
                        Id = x.Id,
                        CozumKod = x.CozumKod
                    })
                    .ToList();
            }

            // TIP 4 – HTML/CSS Preview
            if (soru.SoruTipi == 4)
            {
                var preview = await _unitOfWork.Repository<SoruCanliPreview>()
                    .Query()
                    .FirstOrDefaultAsync(x => x.SoruId == soru.Id);

                if (preview != null)
                {
                    dto.Tip4 = new Tip4Dto
                    {
                        Id = preview.Id,
                        DogruHtml = preview.DogruHtml,
                        DogruCss = preview.DogruCss,
                        GerekenEtiketler = System.Text.Json.JsonSerializer.Deserialize<List<string>>(preview.GerekenEtiketlerJson),
                        GerekenStiller = System.Text.Json.JsonSerializer.Deserialize<List<string>>(preview.GerekenStillerJson)
                    };
                }
            }

            return dto;
        }


        // -------------------------------------------------------------------
        //  TIP 1: ÇOKTAN SEÇMELİ SORU (4 seçenek)
        // -------------------------------------------------------------------

        public async Task<SoruDto> CreateTip1Async(int dersId, SoruTip1CreateDto dto)
        {
            var soruRepo = _unitOfWork.Repository<Soru>();
            var secenekRepo = _unitOfWork.Repository<SoruSecenek>();

            // 1) Önce soruyu ekle
            var soru = new Soru
            {
                DersId = dersId,
                SoruMetni = dto.SoruMetni,
                SoruTipi = 1
            };

            await soruRepo.AddAsync(soru);
            await _unitOfWork.SaveAsync(); // ID oluşsun

            // 2) Seçenekleri ekle
            var secenekEntities = new List<SoruSecenek>();

            for (int i = 0; i < dto.Secenekler.Count; i++)
            {
                secenekEntities.Add(new SoruSecenek
                {
                    SoruId = soru.Id,
                    SecenekMetni = dto.Secenekler[i]
                });
            }

            await secenekRepo.AddRangeAsync(secenekEntities);
            await _unitOfWork.SaveAsync();

            // 3) Doğru cevabı işaretle
            var dogru = secenekEntities[dto.DogruIndex];
            soru.DogruCevapId = dogru.Id;

            soruRepo.Update(soru);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<SoruDto>(soru);
        }

        // -------------------------------------------------------------------
        //  TIP 2: KELİME BLOK TAMAMLAMA SORUSU
        // -------------------------------------------------------------------

        public async Task<SoruDto> CreateTip2Async(int dersId, SoruTip2CreateDto dto)
        {
            var soruRepo = _unitOfWork.Repository<Soru>();

            var soru = new Soru
            {
                DersId = dersId,
                SoruMetni = dto.SoruMetni,
                SoruTipi = 2
            };

            await soruRepo.AddAsync(soru);
            await _unitOfWork.SaveAsync(); // ID oluşsun

            // Tip2 özel tablo
            var kelimeRepo = _unitOfWork.Repository<SoruKelimeBlok>();
            var kelime = new SoruKelimeBlok
            {
                SoruId = soru.Id,
                DogruKod = dto.DogruKod,
                KelimelerJson = System.Text.Json.JsonSerializer.Serialize(dto.Kelimeler)
            };

            await kelimeRepo.AddAsync(kelime);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<SoruDto>(soru);
        }

        // -------------------------------------------------------------------
        //  TIP 3: FONKSİYON TAMAMLAMA SORUSU
        // -------------------------------------------------------------------

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

            var cozumRepo = _unitOfWork.Repository<SoruFonksiyonCozum>();

            foreach (var coz in dto.DogruCozumler)
            {
                await cozumRepo.AddAsync(new SoruFonksiyonCozum
                {
                    SoruId = soru.Id,
                    CozumKod = coz
                });
            }

            await _unitOfWork.SaveAsync();

            return _mapper.Map<SoruDto>(soru);
        }

        // -------------------------------------------------------------------
        //  TIP 4: HTML/CSS CANLI ÖNİZLEME
        // -------------------------------------------------------------------

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

            var previewRepo = _unitOfWork.Repository<SoruCanliPreview>();

            var preview = new SoruCanliPreview
            {
                SoruId = soru.Id,
                DogruHtml = dto.DogruHtml,
                DogruCss = dto.DogruCss,
                GerekenEtiketlerJson = System.Text.Json.JsonSerializer.Serialize(dto.GerekenEtiketler),
                GerekenStillerJson = System.Text.Json.JsonSerializer.Serialize(dto.GerekenStiller)
            };

            await previewRepo.AddAsync(preview);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<SoruDto>(soru);
        }

        // -------------------------------------------------------------------
        //  SORU SİLME
        // -------------------------------------------------------------------

        public async Task<bool> DeleteAsync(int id)
        {
            var soruRepo = _unitOfWork.Repository<Soru>();
            var soru = await soruRepo.GetByIdAsync(id);

            if (soru == null)
                return false;

            soruRepo.Remove(soru);
            await _unitOfWork.SaveAsync();

            return true;
        }
    }
}
