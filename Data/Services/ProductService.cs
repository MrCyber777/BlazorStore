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
        private readonly ProductService _productService;

        public ProductService(ApplicationDbContext db, ProductService productService)
        {
            _productService = productService;
            _db = db;
        }

        public async Task<Product> GetSingleProductAsync(int id)
        {
            Product productFromDB = await _db.Products.Include(x => x.Category)
                                                       .Include(x => x.SpecialTag)
                                                      .FirstOrDefaultAsync(x => x.Id == id);
            return productFromDB;
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            List<Product> productsFromDB = await _db.Products.Include(x => x.Category)
                                                             .Include(x => x.SpecialTag)
                                                              .ToListAsync();
            return productsFromDB;
        }

        public List<Product> GetSortedProductsAsync(int pageIndex, out int totalCount)
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
        /// <summary>
        /// Update single product
        /// </summary>
        /// <param name="productForUpdate">Product Object</param>
        /// <returns>If update success, return true, else return false</returns>
        public async Task<bool> UpdateProductAsync(Product productForUpdate)
        {
            if (productForUpdate == null)
                return false;
            Product productFromDB = await _db.Products.FirstOrDefaultAsync(x => x.Id == productForUpdate.Id);
            if (productFromDB == null)
                return false;
            if (productForUpdate.Image == null)
                productForUpdate.Image = productFromDB.Image;
            _db.Products.Update(productForUpdate);
            await _db.SaveChangesAsync();

            return true;
        }
        public async Task<List<Product>> ShoppingCart(List<int> listOfShoppingCart, List<Product> listOfProducts)
        {
            double totalPrice = 0;                    
            if (listOfShoppingCart is not null && listOfShoppingCart.Any())
            {
                for (int i = 0; i < listOfShoppingCart.Count; i++)
                {
                    var currentProduct = await _productService.GetSingleProductAsync(listOfShoppingCart[i]);
                    if (listOfProducts.Exists(x => x.Id.Equals(currentProduct.Id)))
                        listOfProducts.FirstOrDefault(x => x.Id == currentProduct.Id).Quantity++;
                    else
                    {
                        currentProduct.Quantity = 1;
                        listOfProducts.Add(currentProduct);
                    }

                }
                // 4. Получить конечную сумму всех продуктов и сохранить значение в переменную
                if (listOfProducts is not null && listOfProducts.Any())
                {
                    listOfProducts.ForEach(x =>
                    {
                        totalPrice += x.Price * x.Quantity;
                    });
                }
            }
            return listOfProducts;
         
        }
        /// <summary>
        /// Deletion  of a single product 
        /// </summary>
        /// <param name="productForDeletion">Product object</param>
        /// <returns>If success true, else false</returns>
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