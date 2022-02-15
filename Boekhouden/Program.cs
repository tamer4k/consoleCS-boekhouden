using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Boekhouden
{
    internal class Program
    {
        static void Main(string[] args)
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
            return null;
        }
    }
}
