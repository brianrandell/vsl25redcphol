using System.ComponentModel.DataAnnotations;

namespace AdvancedCopilotAPI.Models
{
    public class Product
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;
        
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }
        
        [Range(0, int.MaxValue)]
        public int Stock { get; set; }
        
        [StringLength(50)]
        public string Category { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        public bool IsActive { get; set; } = true;
    }
}