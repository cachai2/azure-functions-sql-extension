﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SqlExtensionSamples
{

    public class GetProducts
    {

        
        [FunctionName("GetProducts")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "getproducts/{cost}")]
            HttpRequest req,
            [Sql("select * from Products where cost = @Cost",
                CommandType = System.Data.CommandType.Text,
                Parameters = "@Cost={cost}",
                ConnectionStringSetting = "SQLServerAuthentication")]
            string products)
        {
            return (ActionResult)new OkObjectResult(products);
        }

        /**
        [FunctionName("GetProducts")]
        public static IActionResult Run(
             [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "getproducts/{cost}")]
             HttpRequest req,
             [Sql("select * from Products where cost = @Cost",
                 Type = System.Data.CommandType.Text,
                 Parameters = "@Cost={cost}",
                 ConnectionStringSetting = "SQLServerAuthentication")]
             string products)
        {
            return (ActionResult)new OkObjectResult(products);
        } **/


        /**
        [FunctionName("GetProducts")]
        public static IActionResult Run(
             [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "getproducts/{cost}")]
             HttpRequest req,
             [Sql(Procedure = "SelectProductsCost",
                 Parameters = "@Cost: {cost}",
                 Authentication = "%SQLServerAuthentication%",
                 ConnectionString = "Data Source=sotevo.database.windows.net;Database=TestDB;")]
             IEnumerable<Product> products)
        {
            return (ActionResult)new OkObjectResult(products);
        } **/

        /**
        [FunctionName("GetProducts")]
        public static async Task<IActionResult> Run(
             [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "getproducts/{cost}")]
             HttpRequest req,
             [Sql("select * from Products where cost = @Cost",
                 Type = System.Data.CommandType.Text,
                 Parameters = "@Cost={cost}",
                 ConnectionStringSetting = "SQLServerAuthentication")]
             IAsyncEnumerable<Product> products)
        {
            var enumerator = products.GetAsyncEnumerator();
            var list = new List<Product>();
            while (await enumerator.MoveNextAsync())
            {
                list.Add(enumerator.Current);
            }
            await enumerator.DisposeAsync();
            return (ActionResult)new OkObjectResult(list);
        } **/

        /**
        [FunctionName("GetProducts")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "getproducts")]
            HttpRequest req)
        {
            SqlConnection connection = BuildSqlConnection();
            string result = string.Empty;
            using (connection)
            {
                try
                {
                    string query = "select * from dbo.Products where cost = " + req.Query["cost"] + " FOR JSON AUTO";
                    SqlCommand command = new SqlCommand(query, connection);
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result += reader[0];
                        }
                    }
                }
                catch (Exception e)
                {
                    throw new InvalidOperationException("Exception in executing query: " + e.Message);
                }
            }
            var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(result);
            return (ActionResult)new OkObjectResult(products);
        } **/

        public static SqlConnection BuildSqlConnection()
        {
            return null;
        }



        /**
        [FunctionName("GetProduct")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "products/{id}")]
            HttpRequest req,
            ILogger logger,
            [Sql(SQLQuery = "select * from dbo.Products",
                Authentication = "%SQLServerAuthentication%",
                ConnectionString = "Data Source=sotevo.database.windows.net;Database=TestDB;")]
            SqlCommand command)
        {
            string result = string.Empty;
            using (SqlConnection connection = command.Connection)
            {
                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result += reader[0];
                        }
                    }
                }
                catch (Exception e)
                {
                    throw new InvalidOperationException("Exception in executing query: " + e.Message);
                }

            }
            return (ActionResult)new OkObjectResult(result);
        } **/


        public class Product
        {
            public int ProductID { get; set; }

            public string Name { get; set; }

            public int Cost { get; set; }
        }
    }
}