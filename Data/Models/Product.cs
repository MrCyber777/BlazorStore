
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorStore.Data.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50),MinLength(2)]        
        public string Name { get; set; }
        [Required]
        [Range(1,1000000)]
        public double Price { get; set; }
        public int Quantity { get; set; } = 1;
        public byte[] Image { get; set; }
        public string ShadeColor { get; set; }

        public int SpecialTagId { get; set; }
        [ForeignKey(nameof(SpecialTagId))]
        public virtual SpecialTag SpecialTag { get; set; }
        public int CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public virtual Category Category { get; set; }
    }
}
