using BlazorStore.Data.Models;
using BlazorStore.Utility;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorStore.Data.Services
{
    public class CategoryService
    {
        private readonly ApplicationDbContext _db;

        public CategoryService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Category> GetSingleCategoryAsync(int id)
        {
            Category categoryFromDB = await _db.Categories.FindAsync(id);
            return categoryFromDB;

            // 2
            //return await _db.Categories.FindAsync(id);
        }

        public List<Category> GetSortedCategoriesAsync(int pageIndex, out int totalCount)
        {
            totalCount = _db.Categories.Count();

            var categoriesFromDB = Task.Run(async () => await _db.Categories.Skip((pageIndex - 1) * SD.PageSize)
                                                               .Take(SD.PageSize)
                                                               .ToListAsync()).Result;

            return categoriesFromDB;
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            List<Category> allCategories = await _db.Categories.ToListAsync();
            return allCategories;
        }

        public async Task<bool> CreateCategoryAsync(Category newCategory)
        {
            if (newCategory == null)
                return false;
            await _db.Categories.AddAsync(newCategory);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateCategoryAsync(Category categoryForUpdate)
        {
            if (categoryForUpdate == null)
                return false;
            Category categoryFromDB = await _db.Categories.FindAsync(categoryForUpdate.Id);
            if (categoryFromDB == null)
                return false;
            categoryFromDB.Name = categoryForUpdate.Name;
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteCategoryAsync(Category categoryForDeletion)
        {
            if (categoryForDeletion == null)
                return false;
            Category categoryFromDB = await _db.Categories.FindAsync(categoryForDeletion.Id);
            if (categoryFromDB == null)
                return false;
            _db.Categories.Remove(categoryFromDB);
            await _db.SaveChangesAsync();

            return true;
        }
    }
}