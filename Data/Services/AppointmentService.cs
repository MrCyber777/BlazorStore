
using BlazorStore.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorStore.Data.Services
{
    public class AppointmentService
    {
       private readonly ApplicationDbContext _db;
       public AppointmentService(ApplicationDbContext db)
        {
            _db = db;
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
        public async Task<int>CreateAppointmentAsync(Appointment newAppointment)
        {
            if (newAppointment == null)
                return 0;
            await _db.Appointments.AddAsync(newAppointment);
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
