using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shared.Models;

namespace ProductAPI.DataAccess
{
    public interface DataAccess
    {

        Task<Product[]> GetAllProducts(int storeId);

        Task<Product> GetProductById(int storeId, long id);

        Task<Product> GetProductByProductNumber(int storeId, string productNumber);

        Task<Product> GetProductByEAN(int storeId, string EAN);

        Task<Product> AddNewProduct(int storeId, Product product);

        Task<Product> AlterProduct(int storeId, Product product);

        Task DeleteProduct(int storeId, int id);
    }
}
