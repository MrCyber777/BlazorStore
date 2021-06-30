

using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorStore.Data.Models
{
    public class ApplicationUser:IdentityUser
    {
        [MaxLength(30)]
        [Display(Name = "Sales person")]
        public string Name { get; set; }
        [NotMapped]
        public bool IsSuperAdmin { get; set; }
        [NotMapped]
        public bool IsUser { get; set; }
    }
}
