
using System.ComponentModel.DataAnnotations;

namespace BlazorStore.Data.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(30),MinLength(2)]
        [Display(Name="Category Name")]
        public string Name { get; set; }
    }
}
