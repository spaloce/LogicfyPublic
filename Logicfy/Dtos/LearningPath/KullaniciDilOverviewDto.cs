namespace Logicfy.Dtos.LearningPath
{
    public class KullaniciDilOverviewDto
    {
        public int DilId { get; set; }
        public string DilAdi { get; set; }

        public int KullaniciXp { get; set; }
        public int KullaniciSeviye { get; set; }
        public int Streak { get; set; }

        public List<KullaniciUniteOverviewDto> Uniteler { get; set; }
    }

    public class KullaniciUniteOverviewDto
    {
        public int UniteId { get; set; }
        public string Baslik { get; set; }

        public double IlerlemeOrani { get; set; }
        public bool KilitliMi { get; set; }

        public List<KullaniciKisimOverviewDto> Kisimlar { get; set; }
    }

    public class KullaniciKisimOverviewDto
    {
        public int KisimId { get; set; }
        public string Baslik { get; set; }

        public double IlerlemeOrani { get; set; }
        public bool TamamlandiMi { get; set; }

        public List<KullaniciDersOverviewDto> Dersler { get; set; }
    }

    public class KullaniciDersOverviewDto
    {
        public int DersId { get; set; }
        public string Baslik { get; set; }

        public int CozulenSoru { get; set; }
        public int ToplamSoru { get; set; }
        public double IlerlemeOrani { get; set; }
    }
}
