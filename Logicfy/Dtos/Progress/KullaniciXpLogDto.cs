namespace Logicfy.Dtos.Progress
{
    public class KullaniciXpLogDto
    {
        public string Kaynak { get; set; }   // "Soru Çözme", "Ders Tamamlama", "Streak"
        public int Xp { get; set; }
        public DateTime Tarih { get; set; }
    }

}
