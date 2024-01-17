using System.ComponentModel.DataAnnotations;

namespace Dawn_Winery.Models
{
    public class EndProduct
    {
        [Key] public string? Mname { get; set; }
        public int Year { get; set; }
        public int Aging { get; set; }
        public int Quality { get; set; }
        public bool Type { get; set; }

        public int Milil { get; set; }
        public int Bottle { get; set; }
        public int Stock { get; set; }
    }
}
