
using System.ComponentModel.DataAnnotations;


namespace BlazorStore.Data.Models
{
    public class RegisterInputModel
    {
       
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

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


            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }       
    }
}
