namespace Logicfy.Models
{
    public class Unite : BaseEntity
    {
        public int ProgramlamaDiliId { get; set; }
        public string Baslik { get; set; }
        public int Sira { get; set; }
        public int DersSayisiCache { get; set; }

        public ProgramlamaDili ProgramlamaDili { get; set; }
        public ICollection<Kisim> Kismlar { get; set; }
    }

}
