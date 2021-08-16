using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorStore.Data.Models
{
    public class OrderModel
    {
        [Key]
        public int OrderId { get; set; }
        public string UserId { get; set; }
        public int AppointmentId { get; set; }
        public DateTime CreatedAt { get; set; }
        [ForeignKey(nameof(UserId))]
        public  virtual ApplicationUser Customer { get; set; }
        [ForeignKey(nameof(AppointmentId))]
        public  virtual Appointment Appointment { get; set; }
    }
}
