namespace Logicfy.Dtos.Soru
{
    public class SoruBatchItemDto
    {
        public string Language { get; set; }
        public string Unite { get; set; }
        public string Kisim { get; set; }
        public String Lesson { get; set; }
        public int QuestionType { get; set; }
        public string SoruMetni { get; set; }
        public string EkVeriJson { get; set; }
        public string DogruCevap { get; set; }
    }
    public class SoruBatchCreateDto
    {
        public List<SoruBatchItemDto> Sorular { get; set; }
    }

}
