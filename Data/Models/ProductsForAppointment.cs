﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorStore.Data.Models
{
    public class ProductsForAppointment
    {
        public int Id { get; set; }
        public int AppointmentId { get; set; }
        public int ProductId { get; set; }
        public string UserId { get; set; }
        public DateTime CreateAt { get; set; }

    }
}
