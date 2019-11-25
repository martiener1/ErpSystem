using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Shared.Data;
using Shared.Models;

namespace ProductAPI.DataAccess
{
    public class DataAccessImplMySql : DataAccess
    {
        string dbConnectionString;

        public DataAccessImplMySql(string dbConnectionString)
        {
            this.dbConnectionString = dbConnectionString;
        }

        public DataAccessImplMySql()
        {
            this.dbConnectionString = DatabaseConnectionString.GetAzureConnectionString("product");
        }

        private MySqlConnection NewConnection()
        {
            DBConnection dbCon = DBConnection.Instance();
            dbCon.Connect(dbConnectionString);
            return dbCon.Connection;
        }

        private void CloseConnection()
        {
            //DBConnection.Instance().Close();
        }

        public async Task<Product> AddNewProduct(int storeId, Product product)
        {
            /* THIS METHOD WILL NOT BE USED IN THE POC
             * 
             * if product has a group
             * check if group is already in database
             * if not, create new productgroup record
             * check if group has a category
             * check if category is already in database
             * if not, create new productcategory record
             * add product to database
             * select last added record in database
             * return that record
             */ 
            throw new NotImplementedException();
        }

        public async Task<Product> AlterProduct(int storeId, Product product)
        {
            // THIS METHOD WILL NOT BE USED IN THE POC
            throw new NotImplementedException();
        }

        public async Task DeleteProduct(int storeId, int id)
        {
            // THIS METHOD WILL NOT BE USED IN THE POC
            throw new NotImplementedException();
        }

        public async Task<Product[]> GetAllProducts(int storeId)
        {
            // THIS METHOD WILL NOT BE USED IN THE POC
            throw new NotImplementedException();
        }

        public async Task<Product> GetProductByEAN(int selectStoreId, string selectEuropeanArticleNumber)
        {
            string query = "Select * from product where storeid = @0 and europeanArticleNumber = @1;";
            object[] row = await QueryExecutor.SelectSingle(NewConnection(), query, MySqlDbType.Int32, selectStoreId, MySqlDbType.VarChar, selectEuropeanArticleNumber);
            CloseConnection();
            if (row[0] == null) return null; // empty record
            int id = (int)row[0];
            int storeId = (int)row[1];
            int groupId = (int)row[2];
            ProductGroup productGroup = await GetProductGroup(selectStoreId, groupId);
            string name = (row[3] == DBNull.Value) ? null : (string)row[3];
            string brand = (row[4] == DBNull.Value) ? null : (string)row[4];
            double? buyingPrice = (row[5] == null) ? null : (double?)row[5];
            double? sellingPrice = (row[6] == null) ? null : (double?)row[6];
            string supplier = (row[7] == DBNull.Value) ? null : (string)row[7];
            string productNumber = (row[8] == DBNull.Value) ? null : (string)row[8];
            string ean = (row[9] == DBNull.Value) ? null : (string)row[9];

            Product product = new Product(id, storeId, name, brand, buyingPrice, sellingPrice, supplier, productNumber, ean, productGroup);
            return product;
        }

        public async Task<Product> GetProductById(int selectStoreId, long selectId)
        {
            string query = "Select * from product where storeid = @0 and id = @1;";
            object[] row = await QueryExecutor.SelectSingle(NewConnection(), query, MySqlDbType.Int32, selectStoreId, MySqlDbType.Int32, selectId);
            CloseConnection();
            if (row[0] == null) return null; // empty record
            int id = (int)row[0];
            int storeId = (int)row[1];
            int groupId = (int)row[2];
            ProductGroup productGroup = await GetProductGroup(selectStoreId, groupId);
            string name = (row[3] == DBNull.Value) ? null : (string)row[3];
            string brand = (row[4] == DBNull.Value) ? null : (string)row[4];
            double? buyingPrice = (row[5] == null) ? null : (double?)row[5];
            double? sellingPrice = (row[6] == null) ? null : (double?)row[6];
            string supplier = (row[7] == DBNull.Value) ? null : (string)row[7];
            string productNumber = (row[8] == DBNull.Value) ? null : (string)row[8];
            string ean = (row[9] == DBNull.Value) ? null : (string)row[9];

            Product product = new Product(id, storeId, name, brand, buyingPrice, sellingPrice, supplier, productNumber, ean, productGroup);
            return product;
        }

        public async Task<Product> GetProductByProductNumber(int selectStoreId, string selectProductNumber)
        {
            string query = "Select * from product where storeid = @0 and productNumber = @1;";
            object[] row = await QueryExecutor.SelectSingle(NewConnection(), query, MySqlDbType.Int32, selectStoreId, MySqlDbType.VarChar, selectProductNumber);
            CloseConnection();
            if (row[0] == null) return null; // empty record
            int id = (int)row[0];
            int storeId = (int)row[1];
            int groupId = (int)row[2];
            ProductGroup productGroup = await GetProductGroup(selectStoreId, groupId);
            string name = (row[3] == DBNull.Value) ? null : (string)row[3];
            string brand = (row[4] == DBNull.Value) ? null : (string)row[4];
            double? buyingPrice = (row[5] == null) ? null : (double?)row[5];
            double? sellingPrice = (row[6] == null) ? null : (double?)row[6];
            string supplier = (row[7] == DBNull.Value) ? null : (string)row[7];
            string productNumber = (row[8] == DBNull.Value) ? null : (string)row[8];
            string ean = (row[9] == DBNull.Value) ? null : (string)row[9];

            Product product = new Product(id, storeId, name, brand, buyingPrice, sellingPrice, supplier, productNumber, ean, productGroup);
            return product;
        }

        private async Task<ProductGroup> GetProductGroup(int selectStoreId, int selectGroupId)
        {
            string query = "select `id`, `categoryId`, `name` from `group` where storeId = @0 and id = @1;";
            object[] row = await QueryExecutor.SelectSingle(NewConnection(), query, MySqlDbType.Int32, selectStoreId, MySqlDbType.Int32, selectGroupId);
            CloseConnection();
            if (row[0] == null) return null; // empty record
            int id = (int)row[0];
            int categoryId = (int)row[1];
            ProductCategory productCategory = await GetProductCategory(selectStoreId, categoryId);
            string name = (string)row[2];

            return new ProductGroup(id, name, productCategory);
        }

        private async Task<ProductCategory> GetProductCategory(int selectStoreId, int selectCategoryId)
        {
            string query = "select id, `name` from category where storeId = @0 and id = @1;";
            object[] row = await QueryExecutor.SelectSingle(NewConnection(), query, MySqlDbType.Int32, selectStoreId, MySqlDbType.Int32, selectCategoryId);
            CloseConnection();
            if (row[0] == null) return null; // empty record
            int id = (int)row[0];
            string name = (string)row[1];

            return new ProductCategory(id, name);
        }
    }
}
