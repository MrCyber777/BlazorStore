
using BlazorStore.Data.Models;
using BlazorStore.Utility;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorStore.Data.Services
{
    public class ProductService
    {
        private readonly ApplicationDbContext _db;
        public ProductService(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<Product> GetSingleProductAsync(int id)
        {
            Product productFromDB = await _db.Products.Include(x => x.Category)
                                                       .Include(x => x.SpecialTag)
                                                      .FirstOrDefaultAsync(x=>x.Id==id);
            return productFromDB;
        }
        public async Task<List<Product>> GetAllProductsAsync()
        {
            List<Product> productsFromDB = await _db.Products.Include(x => x.Category)
                                                             .Include(x => x.SpecialTag)
                                                              .ToListAsync();
            return productsFromDB;
        }

        public List<Product> GetSortedProductsAsync(int pageIndex,out int totalCount)
        {                                        
           //List<Product> productsFromDB =  _db.Products
           //                                               .Include(x => x.Category)
           //                                                .Include(x => x.SpecialTag)                                                          
           //                                                .Skip((pageIndex - 1) * SD.PageSize)
           //                                                .Take(SD.PageSize)
           //                                               .ToList();
            totalCount = _db.Products.Count();
            var productsFromDB = Task.Run(async () => await _db.Products
                                                               .Skip((pageIndex - 1) * SD.PageSize)
                                                               .Take(SD.PageSize)
                                                               .Include(x => x.Category)
                                                               .Include(x => x.SpecialTag).ToListAsync()).Result;
            return productsFromDB;
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            List<Category> categoriesFromDB = await _db.Categories.ToListAsync();
            return categoriesFromDB;
            //return await _db.Categories.ToListAsync();
        }
        public async Task<List<SpecialTag>> GetAllSpecialTagsAsync()
        {
            List<SpecialTag> specialTagsFromDB = await _db.SpecialTags.ToListAsync();
            return specialTagsFromDB;
        }
        public async Task<bool> CreateProductAsync(Product newProduct)
        {
            if (newProduct == null)
                return false;
            await _db.Products.AddAsync(newProduct);
            await _db.SaveChangesAsync();

            return true;
        }
        public async Task<bool> UpdateProductAsync(Product productForUpdate)
        {
            if (productForUpdate == null)
                return false;
            Product productFromDB = await _db.Products.FirstOrDefaultAsync(x=>x.Id==productForUpdate.Id);
            if (productFromDB == null)
                return false;
            if (productForUpdate.Image == null)
                productForUpdate.Image = productFromDB.Image;
            _db.Products.Update(productForUpdate);
            await _db.SaveChangesAsync();

            return true;

        }
        public async Task<bool> DeleteProductAsync(Product productForDeletion)
        {
            if (productForDeletion == null)
                return false;
            Product productFromDB = await _db.Products.FirstOrDefaultAsync(x => x.Id == productForDeletion.Id);
            if (productFromDB == null)
                return false;
            _db.Products.Remove(productFromDB);
            await _db.SaveChangesAsync();

            return true;
        }
    }
}
