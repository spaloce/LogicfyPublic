namespace Logicfy.Dtos.Ders
{
    public class DersDto
    {
        public int Id { get; set; }
        public string Baslik { get; set; }
        public int Sira { get; set; }
        public int TahminiSure { get; set; }
        public int ZorlukSeviyesi { get; set; }
        public int SoruSayisi { get; set; }  // ← eksik olan buydu

    }

}
