namespace Logicfy.Dtos.Soru
{
    public class SoruTip4CreateDto
    {
        public string SoruMetni { get; set; }
        public string DogruHtml { get; set; }
        public string DogruCss { get; set; }
        public List<string> GerekenEtiketler { get; set; }
        public List<string> GerekenStiller { get; set; }
    }

}
