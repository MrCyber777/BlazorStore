using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorStore.Data.Models
{
    public enum PaymentStatusEnum
    {
        Success,
        Fail
    }
    public class Payment
    {
        [Key]
        public int Id { get; set; }
        public string PayPalPaymentId { get; set; }
        public double TotalPrice { get; set; }
        public string PayPalEmail { get; set; }
        public string PayerFirstName { get; set; }
        public string PayerLastName { get; set; }
        public PaymentStatusEnum? PaymentStatus { get; set; }

    }
}
