using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorStore.Data.Models
{
    public class OrderDetails
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product Products { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser User { get; set; }

        public List<OrderModel> Orders { get; set; } = new();
    }
}