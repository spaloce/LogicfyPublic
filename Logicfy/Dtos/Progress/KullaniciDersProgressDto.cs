namespace Logicfy.Dtos.Progress
{
    public class KullaniciDersProgressDto
    {
        public int DersId { get; set; }
        public string DersBaslik { get; set; }

        public int CozulenSoru { get; set; }
        public int ToplamSoru { get; set; }
        public double IlerlemeYuzdesi { get; set; }
        public DateTime SonIslem { get; set; }
    }


}
