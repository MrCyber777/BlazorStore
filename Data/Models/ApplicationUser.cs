using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BlazorStore.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage = "Please enter a name")]
        [MaxLength(50), MinLength(2)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter a surname")]
        [MaxLength(50), MinLength(2)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter the first address line")]
        [Display(Name = "Address 1"), MaxLength(255), MinLength(3)]
        public string Address { get; set; }

        [Required(ErrorMessage = "Please enter  a city name"), MaxLength(50), MinLength(2)]
        public string City { get; set; }

        public int Zip { get; set; }

        [Required(ErrorMessage = "Please enter a country name"), MaxLength(50), MinLength(2)]
        public string Country { get; set; }
        public string IP { get; set; }
        public string Mac { get; set; }
    }
}