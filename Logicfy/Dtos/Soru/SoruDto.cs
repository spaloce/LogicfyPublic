namespace Logicfy.Dtos.Soru
{

    public class SoruDto
    {
        public int Id { get; set; }
        public int DersId { get; set; }
        public int SoruTipi { get; set; }
        public string SoruMetni { get; set; }

        // TIP 1
        public List<SoruSecenekDto> Secenekler { get; set; }
        public string DogruCevapMetni { get; set; }

        // TIP 2
        public Tip2Dto Tip2 { get; set; }

        // TIP 3
        public List<Tip3CozumDto> Tip3 { get; set; }

        // TIP 4
        public Tip4Dto Tip4 { get; set; }
    }
}



