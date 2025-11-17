namespace Logicfy.Dtos
{
    public class Tip4Dto
    {
        public int Id { get; set; }
        public string DogruHtml { get; set; }
        public string DogruCss { get; set; }

        public List<string> GerekenEtiketler { get; set; }
        public List<string> GerekenStiller { get; set; }
    }
}
