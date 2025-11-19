namespace Logicfy.Dtos.Auth
{
    public class KullaniciRegisterDto
    {
        public string AdSoyad { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Rol { get; set; } = "User";
    }

}
