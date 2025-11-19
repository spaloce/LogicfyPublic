using Logicfy.Dtos.Progress;

namespace Logicfy.Dtos.Auth
{
    public class KullaniciDetayDto
    {
        public int Id { get; set; }
        public string AdSoyad { get; set; }
        public string Email { get; set; }
        public int ToplamXp { get; set; }
        public int Seri { get; set; }

        // Kullanıcının son ilerlemeleri
        public List<KullaniciDersProgressDto> SonDersler { get; set; }
    }

}
