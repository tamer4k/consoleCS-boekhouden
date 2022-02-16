using Boekhouden.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Boekhouden
{
    internal class Inlezen_Json
    {
        private readonly ApplicationDbContext _context;

        public Inlezen_Json()
        {
            _context  = new ApplicationDbContext();
        }
        static List<Invoice> RootObject()
        {
            string Json = (@"C:\Users\talashraf\OneDrive - SnelStart Software B.V\Bureaublad\C#\Boekhouden\Boekhouden\json\inputData.json");
            if (File.Exists(Json))
            {
                var transactionRow = JsonConvert.DeserializeObject<List<Invoice>>(File.ReadAllText(Json));
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
            var invoices = RootObject();
            if (invoices != null)
            {
                foreach (var invoice in invoices)
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("TableNumber: " + invoice.TableNumber + " ");
                    Console.WriteLine("SubTotal: " + invoice.SubTotal);
                    Console.WriteLine("CustomerDiscount: " + invoice.CustomerDiscount.DiscountAmount);
                    var totaal = invoice.SubTotal - invoice.CustomerDiscount.DiscountAmount;
                    Console.WriteLine("Total: " + totaal);
                    Console.WriteLine("OrderDateTime: " + invoice.OrderDateTime);
                    Console.WriteLine("=======================");


                    var prijsTotal = invoice.TransactionRows.Sum(tr => tr.Price);
                    var transactionRowDiscountToale = invoice.TransactionRows.Sum(tr => tr.TransactionRowDiscount);
                    var prijsMinDiscount = invoice.TransactionRows.Sum(tr => tr.Price - tr.TransactionRowDiscount);
                    var differenceSubTotal = invoice.SubTotal - prijsMinDiscount;


                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("SubTotal: " + (prijsTotal - transactionRowDiscountToale));
                    Console.WriteLine("Total(Min Discount): " + (prijsMinDiscount - invoice.CustomerDiscount.DiscountAmount));
                    Console.WriteLine("Difference: " + differenceSubTotal);
                    Console.WriteLine("=======================");




                    foreach (var row in invoice.TransactionRows)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("ProductDescription: " + row.ProductDescription);
                        Console.WriteLine("Price: " + row.Price);
                        Console.WriteLine("VatType: " + row.VatType);
                        Console.WriteLine("VatAmount: " + row.VatAmount);
                        Console.WriteLine("TransactionRowDiscount: " + row.TransactionRowDiscount);
                        Console.WriteLine("=======================");

                        var invoice1 = new Invoice();
                        invoice1.TableNumber = invoice.TableNumber;
                        invoice1.SubTotal = invoice.SubTotal;
                        invoice1.Total = invoice.Total;
                        invoice1.CustomerDiscount = invoice.CustomerDiscount;
                        invoice1.OrderDateTime = invoice.OrderDateTime;
                        _context.Add(invoice1);
                        _context.SaveChanges();

                        var transactionRow1 = new TransactionRow();
                        transactionRow1.ProductDescription = row.ProductDescription;
                        transactionRow1.Price = row.Price;
                        transactionRow1.TransactionRowDiscount = row.TransactionRowDiscount;
                        transactionRow1.VatType = row.VatType;
                        transactionRow1.VatAmount = row.VatAmount;
                        invoice1.TransactionRows = new List<TransactionRow>() { transactionRow1 };  
                        _context.Add(transactionRow1);
                        _context.SaveChanges();
                    }





                }
            }
            else
            {
                Console.WriteLine("Er is geen Item");
            }
        }
    }
}
