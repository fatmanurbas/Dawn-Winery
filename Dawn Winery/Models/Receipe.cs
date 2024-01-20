using System.ComponentModel.DataAnnotations;

namespace Dawn_Winery.Models
{
    public class Receipe
    {
        [Display(Name = "Receipe Name")]
        [Key] public string? Rname { get; set; }
        public bool Type { get; set; }
        public string? Grape1 { get; set; }
        public float G1Kilo { get; set; }
        public string? Grape2 { get; set; }
        public float? G2Kilo { get; set; }
        public string? Grape3 { get; set; }
        public float? G3Kilo { get; set; }
        public string? Grape4 { get; set; }
        public float? G4Kilo { get; set; }
        public string? Grape5 { get; set; }
        public float? G5Kilo { get; set; }
        public string? Grape6 { get; set; }
        public float? G6Kilo { get; set; }
        public int SO2 { get; set; }
    }
}
