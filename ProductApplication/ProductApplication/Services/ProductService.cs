using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductAPI.DataAccess;
using Shared.Models;

namespace ProductAPI.Services
{
    public static class ProductService
    {

        private static DataAccess.DataAccess dataAccess = new DataAccessImplMySql();
        
        public async static Task<Product[]> GetAllProducts(int storeId)
        {
            return await dataAccess.GetAllProducts(storeId);
        }

        public static async Task<Product> GetProductById(int storeId, long productId)
        {
            return await dataAccess.GetProductById(storeId, productId);
        }

        public static async Task<Product> GetProductByProductNumber(int storeId, string productNumber)
        {
            return await dataAccess.GetProductByProductNumber(storeId, productNumber);
        }

        public static async Task<Product> GetProductByEAN(int storeId, string EAN)
        {
            return await dataAccess.GetProductByEAN(storeId, EAN);
        }

        public static async Task<Product> AddNewProduct(int storeId, Product product)
        {
            return await dataAccess.AddNewProduct(storeId, product);
        }

        public static async Task<Product> AlterProduct(int storeId, Product product)
        {
            return await dataAccess.AlterProduct(storeId, product);
        }

        public static async Task DeleteProduct(int storeId, int productId)
        {
            await dataAccess.DeleteProduct(storeId, productId);
        }
    }
}
