using System.ComponentModel.DataAnnotations;
using System.IO;

namespace first_exam.Models
{
    public class Integration
    {
        [Key]
        public int id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PathImage { get; set; }
        public Integration() { }
    }
}
