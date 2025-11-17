namespace Logicfy.Dtos.Progress
{
    public class KullaniciKisimProgressDto
    {
        public int KisimId { get; set; }
        public string KisimBaslik { get; set; }

        public int TamamlananDersSayisi { get; set; }
        public int ToplamDersSayisi { get; set; }

        public int IlerlemeOrani { get; set; }
        public bool TamamlandiMi { get; set; }
    }

}
