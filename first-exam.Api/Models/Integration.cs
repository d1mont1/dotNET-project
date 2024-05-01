using System.ComponentModel.DataAnnotations;

namespace first_exam.Api.Models
{
    public partial class Integration
    {
        [Key]
        public int id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string PathImage { get; set; } = null!;
        public Integration() { }
    }
}
