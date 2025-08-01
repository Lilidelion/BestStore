using System.ComponentModel.DataAnnotations;

namespace BestStore.Models
{
    public class ProductDTO
    {
        [Required,MaxLength(100)]
        public string Name { get; set; } = "";
        [Required,MaxLength(100)]
        public string Brand { get; set; } = "";
        [Required,MaxLength(100)]
        public string Category { get; set; } = "";
        [Required]
        public string Price { get; set; } = "";
        [Required]
        public string Description { get; set; } = "";
        public IFormFile? ImageFile { get; set; }
        [Required]
        public string Weight { get; set; } = "";
    }
    }

