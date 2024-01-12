using System.ComponentModel.DataAnnotations;

namespace Dawn_Winery.Models
{
    public class RawMaterial
    {
        [Key] public string? Hid { get; set; }
        public string? Hname { get; set; }
        public bool Type { get; set; }
        public int Alcohol { get; set; }
        public int Sweet { get; set; }
        public int Acidity { get; set; }
        public int Body { get; set; }
        public int Tannin { get; set; }
        public int Stock { get; set; }
    }
}
