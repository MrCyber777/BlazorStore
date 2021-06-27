
using BlazorStore.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorStore.Data.Services
{
    public class SpecialTagService
    {
        private readonly ApplicationDbContext _db;
        public SpecialTagService(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<SpecialTag> GetSingleSpecialTagAsync(int id)
        {
            SpecialTag specialTagFromDB = await _db.SpecialTags.FindAsync(id);
            return specialTagFromDB;
        }
        public async Task<List<SpecialTag>> GetAllSpecialTagsAsync()
        {
            List<SpecialTag> allSpecialTags = await _db.SpecialTags.ToListAsync();
            return allSpecialTags;
        }
        public async Task<bool> CreateSpecialTagAsync(SpecialTag newSpecialTag)
        {
            if (newSpecialTag == null)
                return false;
            await _db.SpecialTags.AddAsync(newSpecialTag);
            await _db.SaveChangesAsync();

            return true;
        }
        public async Task<bool> UpdateSpecialTagAsync(SpecialTag specialTagForUpdate)
        {
            if (specialTagForUpdate == null)
                return false;
            SpecialTag specialTagFromDB = await _db.SpecialTags.FindAsync(specialTagForUpdate.Id);
            if (specialTagFromDB == null)
                return false;
            specialTagFromDB.Name = specialTagForUpdate.Name;
            await _db.SaveChangesAsync();

            return true;
        }
        public async Task<bool> DeleteSpecialTagAsync(SpecialTag specialTagForDeletion)
        {
            if (specialTagForDeletion == null)
                return false;
            SpecialTag specialTagFromDB = await _db.SpecialTags.FindAsync(specialTagForDeletion.Id);
            if (specialTagFromDB == null)
                return false;
            _db.SpecialTags.Remove(specialTagFromDB);
            await _db.SaveChangesAsync();

            return true;
        }     
    }
}
