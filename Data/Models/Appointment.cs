
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorStore.Data.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter a name")]
        public string CustomerName { get; set; }
        [Required(ErrorMessage = "Please enter a surname")]
        public string CustomerSurname { get; set; }
        [Required(ErrorMessage = "Please enter a phone number")]
        public string CustomerPhoneNumber { get; set; }
        [Required(ErrorMessage = "Please enter an email address")]
        public string CustomerEmail { get; set; }
        [Required(ErrorMessage = "Please enter the first address line")]
        [Display(Name = "Address 1")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Please enter  a city name")]
        public string City { get; set; }
        public string Zip { get; set; }
        [Required(ErrorMessage = "Please enter a country name")]
        public string Country { get; set; }
        public DateTime AppointmentDay { get; set; }
        [NotMapped]
        public DateTime AppointmentTime { get; set; }
        public bool IsConfirmed { get; set; }
        public string SalesPersonID { get; set; }
        [ForeignKey(nameof(SalesPersonID))]
        public virtual ApplicationUser SalesPerson { get; set; }

    }
}
