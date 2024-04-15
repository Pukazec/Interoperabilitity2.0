using System.ComponentModel.DataAnnotations.Schema;

namespace Dtos
{
    [Table(nameof(Cat))]
    public class Cat
    {
        public int Id { get; set; }

        public double Age { get; set; }

        public string Color { get; set; } = string.Empty;

        public string CatName { get; set; } = string.Empty;

        public string? Summary { get; set; }
    }
}
