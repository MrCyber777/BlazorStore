﻿
using BlazorStore.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;


namespace BlazorStore.Data.Services
{
    public class AppointmentService
    {
       private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
       public AppointmentService(ApplicationDbContext db,IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Appointment> GetSingleAppointmentAsync(int id)
        {
            Appointment appointmentFromDB = await _db.Appointments.FindAsync(id);
            return appointmentFromDB;
        }
        public async Task<List<Appointment>> GetAllAppointmentsAsync()
        {
            List<Appointment> appointmentsFromDB = await _db.Appointments.ToListAsync();
            return appointmentsFromDB;
        }
        public async Task<int>CreateAppointmentAsync(Appointment newAppointment,List<Product>listOfProducts)
        {
            if (newAppointment == null)
                return 0;
            await _db.Appointments.AddAsync(newAppointment);
            await _db.SaveChangesAsync();

            // Получаем ИД пользователя
            var currentUser = _httpContextAccessor.HttpContext.User;// Получаем объект ClaimsPrincipal
            var claimsIdentity = (ClaimsIdentity)_httpContextAccessor.HttpContext.User.Identity;// Получаем информацию о пользователе (Identity)
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);// Получаем свойство NameIdentifier ( id ) 
            var userId = claim?.Value;

            int orderId = 0;
            OrderModel order = new();
            order.AppointmentId = newAppointment.Id;
            order.UserId = userId;
            order.CreatedAt = System.DateTime.Now;
            await _db.Orders.AddAsync(order);
            await _db.SaveChangesAsync();

            orderId = order.OrderId;

            OrderDetails details = new();
            foreach(var item in listOfProducts)
            {
                details.OrderId = orderId;
                details.UserId = userId;
                details.ProductId = item.Id;
                details.Quantity = item.Quantity;
                await _db.OrderDetails.AddAsync(details);
            }
            await _db.SaveChangesAsync();

            return newAppointment.Id;
        }
        public async Task<int> UpdateAppointmentAsync(Appointment appointmentForUpdate)
        {
            if (appointmentForUpdate == null)
                return 0;
            Appointment appointmentFromDB = await _db.Appointments.FirstOrDefaultAsync(x=>x.Id==appointmentForUpdate.Id);
            if (appointmentFromDB == null)
                return 0;
            _db.Update(appointmentForUpdate);
            await _db.SaveChangesAsync();

            return appointmentForUpdate.Id;
        }
        //public async Task<bool>ConfirmAppointmentAsync(Appointment appointmentForConfirmation)
        //{
            
        //}
        public async Task<bool>DeleteAppointmentAsync(Appointment appointmentForDeletion)
        {
            if (appointmentForDeletion == null)
                return false;
            Appointment appointmentFromDB = await _db.Appointments.FirstOrDefaultAsync(x => x.Id == appointmentForDeletion.Id);
            if (appointmentFromDB == null)
                return false;
            _db.Appointments.Remove(appointmentFromDB);
            await _db.SaveChangesAsync();

            return true;
        }
    }
}
