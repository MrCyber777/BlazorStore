using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorStore.Data.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter a name")]
        [MaxLength(50), MinLength(2)]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Please enter a surname")]
        [MaxLength(50), MinLength(2)]
        public string CustomerSurname { get; set; }

        [Required(ErrorMessage = "Please enter a phone number")]
        [Phone]
        public string CustomerPhoneNumber { get; set; }

        [Required(ErrorMessage = "Please enter an email address")]
        [EmailAddress]
        public string CustomerEmail { get; set; }

        [Required(ErrorMessage = "Please enter the first address line")]
        [Display(Name = "Address 1"), MaxLength(255), MinLength(3)]
        public string Address { get; set; }

        [Required(ErrorMessage = "Please enter  a city name"), MaxLength(50), MinLength(2)]
        public string City { get; set; }

        public int Zip { get; set; }

        [Required(ErrorMessage = "Please enter a country name"), MaxLength(50), MinLength(2)]
        public string Country { get; set; }

        public DateTime AppointmentDay { get; set; } = DateTime.Now;

        [NotMapped]
        public DateTime AppointmentTime { get; set; }

        public bool IsConfirmed { get; set; }
    }
}