using AutoMapper;
using Logicfy.Dtos;
using Logicfy.Dtos.Ders;
using Logicfy.Dtos.Kisim;
using Logicfy.Dtos.Kullanici;
using Logicfy.Dtos.ProgramlamaDili;
using Logicfy.Dtos.Progress;
using Logicfy.Dtos.Soru;
using Logicfy.Dtos.Unite;
using Logicfy.Models;

public class LogicfyMappingProfile : Profile
{
    public LogicfyMappingProfile()
    {
        // ------------------------------------------------------------
        // PROGRAMLAMA DİLLERİ
        // ------------------------------------------------------------
        CreateMap<ProgramlamaDili, ProgramlamaDiliDto>();
        CreateMap<ProgramlamaDiliCreateDto, ProgramlamaDili>();


        // ------------------------------------------------------------
        // UNITE
        // ------------------------------------------------------------
        CreateMap<Unite, UniteDto>();
        CreateMap<UniteCreateDto, Unite>();


        // ------------------------------------------------------------
        // KISIM
        // ------------------------------------------------------------
        CreateMap<Kisim, KisimDto>();
        CreateMap<KisimCreateDto, Kisim>();


        // ------------------------------------------------------------
        // DERS
        // ------------------------------------------------------------
        CreateMap<Ders, DersDto>()
            .ForMember(dest => dest.SoruSayisi,
                opt => opt.MapFrom(src => src.Sorular != null ? src.Sorular.Count : 0));

        CreateMap<DersCreateDto, Ders>();


        // ------------------------------------------------------------
        // SORU (Unified DTO)
        // ------------------------------------------------------------
        CreateMap<Soru, SoruDto>()
            .ForMember(dest => dest.Secenekler,
                opt => opt.MapFrom(src => src.Secenekler))
            .ForMember(dest => dest.DogruCevapMetni,
                opt => opt.MapFrom(src =>
                    src.DogruCevap != null ? src.DogruCevap.SecenekMetni : null))
            .ForMember(dest => dest.Tip2, opt => opt.Ignore())
            .ForMember(dest => dest.Tip3, opt => opt.Ignore())
            .ForMember(dest => dest.Tip4, opt => opt.Ignore());


        // ------------------------------------------------------------
        // SORU SEÇENEK DTO
        // ------------------------------------------------------------
        CreateMap<SoruSecenek, SoruSecenekDto>();


        // ------------------------------------------------------------
        // TIP 1 CREATE DTO -> SORU
        // ------------------------------------------------------------
        CreateMap<SoruTip1CreateDto, Soru>()
            .ForMember(dest => dest.SoruTipi, opt => opt.MapFrom(_ => 1))
            .ForMember(dest => dest.DersId, opt => opt.Ignore())
            .ForMember(dest => dest.DogruCevapId, opt => opt.Ignore())
            .ForMember(dest => dest.Secenekler, opt => opt.Ignore());


        // ------------------------------------------------------------
        // TIP 2-3-4 CREATE DTO -> SORU (Temel veri)
        // ------------------------------------------------------------

        CreateMap<SoruTip2CreateDto, Soru>()
            .ForMember(dest => dest.SoruTipi, opt => opt.MapFrom(_ => 2))
            .ForMember(dest => dest.DersId, opt => opt.Ignore());

        CreateMap<SoruTip3CreateDto, Soru>()
            .ForMember(dest => dest.SoruTipi, opt => opt.MapFrom(_ => 3))
            .ForMember(dest => dest.DersId, opt => opt.Ignore());

        CreateMap<SoruTip4CreateDto, Soru>()
            .ForMember(dest => dest.SoruTipi, opt => opt.MapFrom(_ => 4))
            .ForMember(dest => dest.DersId, opt => opt.Ignore());


        // ------------------------------------------------------------
        // TIP 2-3-4 -> DTO ÇIKTISINDA ÖZEL ALAN DESTEKLERİ
        // (Servis içi zenginleştirme için bırakılmıştır)
        // ------------------------------------------------------------
        CreateMap<SoruKelimeBlok, Tip2Dto>();
        CreateMap<SoruFonksiyonCozum, Tip3CozumDto>();
        CreateMap<SoruCanliPreview, Tip4Dto>();


        // ------------------------------------------------------------
        // KULLANICI
        // ------------------------------------------------------------
        CreateMap<Kullanici, KullaniciDto>();
        CreateMap<KullaniciRegisterDto, Kullanici>();
        CreateMap<Kullanici, KullaniciDetayDto>()
            .ForMember(dest => dest.SonDersler, opt => opt.Ignore());


        // ------------------------------------------------------------
        // PROGRESS
        // ------------------------------------------------------------
        CreateMap<KullaniciDersIlerleme, KullaniciDersProgressDto>()
            .ForMember(dest => dest.DersBaslik,
                opt => opt.MapFrom(src => src.Ders.Baslik));

        CreateMap<KullaniciUnitProgress, KullaniciUnitProgressDto>()
            .ForMember(dest => dest.UniteBaslik,
                opt => opt.MapFrom(src => src.Unite.Baslik));

        CreateMap<KullaniciKisimProgress, KullaniciKisimProgressDto>()
            .ForMember(dest => dest.KisimBaslik,
                opt => opt.MapFrom(src => src.Kisim.Baslik));

        CreateMap<KullaniciSoruCevap, KullaniciSoruCevapDto>();
        CreateMap<KullaniciXpLog, KullaniciXpLogDto>();
    }
}
