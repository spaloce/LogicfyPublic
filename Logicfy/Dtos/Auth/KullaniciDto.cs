namespace Logicfy.Dtos.Auth
{
    public class KullaniciDto
    {
        public int Id { get; set; }
        public string AdSoyad { get; set; }
        public string Email { get; set; }
        public string Rol { get; set; }

        public int XP { get; set; }
        public int Seviye { get; set; }
        public int Streak { get; set; }
    }


}
