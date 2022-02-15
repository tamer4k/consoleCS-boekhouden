using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace Boekhouden
{
    internal class Program
    {
        public void Database()
        {
            
            var datasource = @"(localdb)\MSSQLLocalDB";//your server
            var database = "snelstart"; //your database name
            var username = ""; //username of server to connect
            var password = ""; //password
            string connString = @"Data Source=" + datasource + ";Initial Catalog="
            + database + ";Persist Security Info=True;User ID=" + username + ";Password=" + password;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                try
                {
                    Console.WriteLine("Openning Connection ...");
                    //open connection
                    conn.Open();
                    Console.WriteLine("Connection successful!");
                    //create a new SQL Query using StringBuilder
                    StringBuilder strBuilder = new StringBuilder();
                    Console.WriteLine("Wat is je naam?");
                    string insert1 = Console.ReadLine();
                    strBuilder.Append("INSERT INTO student (fname, lname, gender) VALUES ");
                    strBuilder.Append($"('{insert1}', N'console',N'male'),");
                    string sqlQuery = strBuilder.ToString();
                    using (SqlCommand command = new SqlCommand(sqlQuery, conn)) //pass SQL query created above and connection
                    {
                        command.ExecuteNonQuery(); //execute the Query
                        Console.WriteLine("Query Executed.");
                    }
                    strBuilder.Clear(); // clear all the string

                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e.Message);
                }
            }
        }
        static void Main(string[] args)
        {
            Program p1 = new Program();
            p1.InLezen();
            p1.Database();



            Console.ReadKey();
        }


        static List<Root> RootObject()
        {
            string Json = (@"C:\Users\talashraf\OneDrive - SnelStart Software B.V\Bureaublad\C#\Boekhouden\Boekhouden\json\inputData.json");
            if (File.Exists(Json))
            {
                var transactionRow = JsonConvert.DeserializeObject<List<Root>>(File.ReadAllText(Json));
                //string json = System.Text.Json.JsonSerializer.Serialize(transactionRow.ToArray());
                //File.WriteAllText(Json, json);
                return transactionRow;
            }
            else
            {
                return null;
            }
        }
        public void InLezen()
        {
            var roots = RootObject();
            if (roots != null)
            {
                foreach (var root in roots)
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("TableNumber: " + root.TableNumber + " ");
                    Console.WriteLine("SubTotal: " + root.SubTotal);
                    Console.WriteLine("CustomerDiscount: " + root.CustomerDiscount.DiscountAmount);
                    var totaal = root.SubTotal - root.CustomerDiscount.DiscountAmount;
                    Console.WriteLine("Total: " + totaal);
                    Console.WriteLine("OrderDateTime: " + root.OrderDateTime);

                    foreach (var row in root.TransactionRows)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("ProductDescription: " + row.ProductDescription);
                        Console.WriteLine("Price: " + row.Price);
                        Console.WriteLine("VatType: " + row.VatType);
                        Console.WriteLine("VatAmount: " + row.VatAmount);
                        Console.WriteLine("TransactionRowDiscount: " + row.TransactionRowDiscount);
                        Console.WriteLine("-------------------");
                    }

                    var prijsTotal = root.TransactionRows.Sum(tr => tr.Price);
                    var prijsMinDiscount = root.TransactionRows.Sum(tr => tr.Price - tr.TransactionRowDiscount);
                    var differenceSubTotal = root.SubTotal - prijsMinDiscount;

                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.WriteLine("Total: " + prijsTotal);
                    Console.WriteLine("Total(Min Discount): " + prijsMinDiscount);
                    Console.WriteLine("Difference: " + differenceSubTotal);
                }
            }
        }
    }
}
