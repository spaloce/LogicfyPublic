using System.ComponentModel;

namespace Logicfy.Models
{
    public class ProgramlamaDili : BaseEntity
    {
        public string Ad { get; set; }
        public string Kod { get; set; }   // "javascript"
        public string IkonUrl { get; set; }
        public bool AktifMi { get; set; } = true;

        public ICollection<Unite> Uniteler { get; set; }
    }

}
