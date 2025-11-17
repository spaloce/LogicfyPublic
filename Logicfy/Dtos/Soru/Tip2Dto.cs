namespace Logicfy.Dtos
{
    public class Tip2Dto
    {
        public int Id { get; set; }
        public string DogruKod { get; set; }

        // Kelimeler JSON olarak saklandığı için frontend'e array gönderiyoruz
        public List<string> Kelimeler { get; set; }
    }
}
