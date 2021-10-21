using BlazorStore.Data.Models;
using BlazorStore.Merchant;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace BlazorStore.Data.Services
{
    public class PaymentService
    {
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PaymentService(ApplicationDbContext db, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<OrderModel> GetSinglePaymentAsync(int? id)
        {
            if (id is null)
                return null;
            var paymentFromDB = await _db.Orders.Include(x => x.Customer)
                                                    .Include(x => x.OrderDetails).ThenInclude(x => x.Products)
                                                    .Include(x => x.Payment)
                                                    .FirstOrDefaultAsync(x => x.AppointmentId == id);
            return paymentFromDB;
        }

        public async Task<OrderModel> SavePaymentDetailsAsync(PayPalResponse response)
        {
            int appointmentId;
            var isSuccess = Int32.TryParse(response.transactions[0].description, out appointmentId);
            if (!isSuccess)
                return null;
            OrderModel orderFromDB = await _db.Orders.Include(x => x.Appointment)
                                                   .Include(x => x.Customer)
                                                   .Include(x => x.OrderDetails)
                                                   .FirstOrDefaultAsync(x => x.AppointmentId == appointmentId);
            if (orderFromDB is null)
                return null;
            Payment details = new();

            details.PayPalPaymentId = response.id;
            details.PayerFirstName = response.payer.payer_info.first_name;
            details.PayerLastName = response.payer.payer_info.last_name;
            details.PayPalEmail = response.payer.payer_info.email;
            details.TotalPrice = double.Parse(response.transactions[0].amount.total, CultureInfo.InvariantCulture);
            details.PaymentStatus = PaymentStatusEnum.Success;

            await _db.PaymentDetails.AddAsync(details);
            await _db.SaveChangesAsync();

            orderFromDB.PaymentId = details.Id;
            await _db.SaveChangesAsync();

            return orderFromDB;
        }
    }
}