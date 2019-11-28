using MySql.Data.MySqlClient;
using Shared.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Testing.IntegrationTesting.LoginAPI
{
    class DatabaseProductDummyFiller
    {
        string connectionString;

        public DatabaseProductDummyFiller()
        {
            connectionString = DatabaseConnectionString.GetAzureConnectionString("producttest");
        }

        public static async Task EmptyAndFillDatabases()
        {
            DatabaseProductDummyFiller dbFiller = new DatabaseProductDummyFiller();
            await dbFiller.ClearDatabaseTestRecords();
            await dbFiller.FillDatabaseTablesWithDummyData();
        }

        private async Task ClearDatabaseTestRecords()
        {

            string queryTruncateProducts = "truncate `product`;";
            string queryTruncateGroups = "truncate `group`;";
            string queryTruncateCategories = "truncate `category`;";
            
            await QueryExecutor.Truncate(connectionString, queryTruncateProducts);
            await QueryExecutor.Truncate(connectionString, queryTruncateGroups);
            await QueryExecutor.Truncate(connectionString, queryTruncateCategories);
        }

        private async Task FillDatabaseTablesWithDummyData()
        {
            string queryInsertCategory = "INSERT INTO `category` (`id`, `storeId`, `name`) VALUES ('1', '1', 'category1');";
            string queryInstertGroup1 = "INSERT INTO `group` (`id`, `storeId`, `categoryId`, `name`) VALUES ('1', '1', '1', 'group1');";
            string queryInstertGroup2 = "INSERT INTO `group` (`id`, `storeId`, `categoryId`, `name`) VALUES ('2', '1', '1', 'group2');";
            string queryInsertProduct1 = "INSERT INTO `product` (`id`, `storeId`, `groupId`, `name`, `brand`, `buyingPrice`, `sellingPrice`, `supplier`, `productNumber`, `europeanArticleNumber`) VALUES ('1', '1', '1', 'name1', 'brand1', '1.00', '2.50', 'supplier1', '123456', '8718123456721');";
            string queryInsertProduct2 = "INSERT INTO `product` (`id`, `storeId`, `groupId`, `name`, `brand`, `buyingPrice`, `sellingPrice`, `supplier`, `productNumber`, `europeanArticleNumber`) VALUES ('2', '1', '2', 'name2', 'brand2', '2.00', '4.50', 'supplier2', '654321', '1234554321');";
            
            await QueryExecutor.Insert(connectionString, queryInsertCategory);
            await QueryExecutor.Insert(connectionString, queryInstertGroup1);
            await QueryExecutor.Insert(connectionString, queryInstertGroup2);
            await QueryExecutor.Insert(connectionString, queryInsertProduct1);
            await QueryExecutor.Insert(connectionString, queryInsertProduct2);
        }
    }
}
