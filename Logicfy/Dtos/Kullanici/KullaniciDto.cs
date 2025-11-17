namespace Logicfy.Dtos.Kullanici
{
    public class KullaniciDto
    {
        public int Id { get; set; }
        public string AdSoyad { get; set; }
        public string Email { get; set; }

        public int XP { get; set; }
        public int Seviye { get; set; }
        public int Streak { get; set; }
    }


}
