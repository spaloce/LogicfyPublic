namespace Logicfy.Dtos.Progress
{
    public class KullaniciUnitProgressDto
    {
        public int UniteId { get; set; }
        public string UniteBaslik { get; set; }

        public int TamamlananDersSayisi { get; set; }
        public int ToplamDersSayisi { get; set; }

        public int IlerlemeOrani { get; set; }
        public bool TamamlandiMi { get; set; }
    }

}
