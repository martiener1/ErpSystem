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
        private DatabaseAccess dbAccess;

        public DatabaseProductDummyFiller()
        {
            string connectionString = DatabaseConnectionString.GetAzureConnectionString("producttest");
            dbAccess = new DatabaseAccess(connectionString);
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


            MySqlConnection mySqlConnection = dbAccess.NewConnection();
            await QueryExecutor.Truncate(mySqlConnection, queryTruncateProducts);
            await QueryExecutor.Truncate(mySqlConnection, queryTruncateGroups);
            await QueryExecutor.Truncate(mySqlConnection, queryTruncateCategories);
            dbAccess.CloseConnection();
        }

        private async Task FillDatabaseTablesWithDummyData()
        {
            string queryInsertCategory = "INSERT INTO `category` (`id`, `storeId`, `name`) VALUES ('1', '1', 'category1');";
            string queryInstertGroup1 = "INSERT INTO `group` (`id`, `storeId`, `categoryId`, `name`) VALUES ('1', '1', '1', 'group1');";
            string queryInstertGroup2 = "INSERT INTO `group` (`id`, `storeId`, `categoryId`, `name`) VALUES ('2', '1', '1', 'group2');";
            string queryInsertProduct1 = "INSERT INTO `product` (`id`, `storeId`, `groupId`, `name`, `brand`, `buyingPrice`, `sellingPrice`, `supplier`, `productNumber`, `europeanArticleNumber`) VALUES ('1', '1', '1', 'name1', 'brand1', '1.00', '2.50', 'supplier1', '123456', '8718123456721');";
            string queryInsertProduct2 = "INSERT INTO `product` (`id`, `storeId`, `groupId`, `name`, `brand`, `buyingPrice`, `sellingPrice`, `supplier`, `productNumber`, `europeanArticleNumber`) VALUES ('2', '1', '2', 'name2', 'brand2', '2.00', '4.50', 'supplier2', '654321', '1234554321');";
            
            MySqlConnection mySqlConnection = dbAccess.NewConnection();
            await QueryExecutor.Insert(mySqlConnection, queryInsertCategory);
            await QueryExecutor.Insert(mySqlConnection, queryInstertGroup1);
            await QueryExecutor.Insert(mySqlConnection, queryInstertGroup2);
            await QueryExecutor.Insert(mySqlConnection, queryInsertProduct1);
            await QueryExecutor.Insert(mySqlConnection, queryInsertProduct2);
            dbAccess.CloseConnection();
        }
    }
}
