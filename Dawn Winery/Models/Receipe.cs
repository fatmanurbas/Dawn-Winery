using System.ComponentModel.DataAnnotations;

namespace Dawn_Winery.Models
{
    public class Receipe
    {
        [Key] public string? Rname { get; set; }
        public bool Type { get; set; }
        public string? Grape1 { get; set; }
        public int G1Kilo { get; set; }
        public string? Grape2 { get; set; }
        public int? G2Kilo { get; set; }
        public string? Grape3 { get; set; }
        public int? G3Kilo { get; set; }
        public string? Grape4 { get; set; }
        public int? G4Kilo { get; set; }
        public string? Grape5 { get; set; }
        public int? G5Kilo { get; set; }
        public string? Grape6 { get; set; }
        public int? G6Kilo { get; set; }
        public int SO2 { get; set; }
    }
}
