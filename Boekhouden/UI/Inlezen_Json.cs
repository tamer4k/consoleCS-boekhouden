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
            _context = new ApplicationDbContext();
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
                    // Json file uitprinten
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("TableNumber: " + invoice.TableNumber + " ");
                    Console.WriteLine("SubTotal: " + invoice.SubTotal);
                    Console.WriteLine("CustomerDiscount: " + invoice.CustomerDiscount.DiscountAmount);
                    Console.WriteLine("Total: " + invoice.Total);
                    Console.WriteLine("OrderDateTime: " + invoice.OrderDateTime.ToString("dd/MM/yyyy"));
                    Console.WriteLine("=======================");

                    // Json file berekenen
                    double subTotalNoVat = 0; // vatType 0 btw is 0%
                    double subTotalInclLowVat = 0; // vatType 1 btw 9%
                    double subTotalInclHighVat = 0; // vatType 2 btw 21%
                    double subTotalVat = 0;
                    foreach (var row in invoice.TransactionRows)
                    {
                        var subTotaal = row.Price - row.TransactionRowDiscount; // totaal prijzen min totaal regels discount
                        var vatType = row.VatType;
                        switch (vatType)
                        {
                                case 0:
                                subTotalNoVat += subTotaal;
                                break;
                                case 1:
                                subTotalInclLowVat += subTotaal;
                                break;
                                case 2:
                                subTotalInclHighVat += subTotaal;
                                break;
                        }
                    }

                    var subTotalExclLowVat = Math.Round(subTotalInclLowVat / 1.09,2);
                    var subTotalExclHighVat = Math.Round(subTotalInclHighVat / 1.21,2);
                    var totalVatLow = Math.Round(subTotalInclLowVat - subTotalExclLowVat,2);
                    var totalVatHigh = Math.Round(subTotalInclHighVat - subTotalExclHighVat,2);



                    var prijsTotal = invoice.TransactionRows.Sum(tr => tr.Price); // totaal prijzen
                    var transactionRowDiscountToale = invoice.TransactionRows.Sum(tr => tr.TransactionRowDiscount); // totaal regels discount
                    var customerPercentage = invoice.CustomerDiscount.Percentage; 
                    var orderDateTime = invoice.OrderDateTime.ToString("dd/MM/yyyy");  
                    var subTotal = Math.Round(subTotalExclLowVat + subTotalExclHighVat + totalVatLow + totalVatHigh,2); // totaal prijzen min totaal regels discount
                    var totalVatAmount = Math.Round(totalVatLow + totalVatHigh,2);
                    var customerDiscountAmount = Math.Round(subTotal / 100 * customerPercentage , 2); // customer discount is subtotal keer precentage
                    var total = subTotal - customerDiscountAmount;  //var totaal = SubTotal - CustomerDiscountAmount;
                    var differenceSubTotal = invoice.SubTotal - subTotal;
                   


                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine("SubTotal incl btw: " + subTotal);
                    Console.WriteLine("SubTotal excl btw: " + (subTotal - totalVatAmount));
                    Console.WriteLine("TotalVatAmount " + totalVatAmount);
                    Console.WriteLine("VatAmount 9% : " + totalVatLow);
                    Console.WriteLine("VatAmount 21% : " + totalVatHigh);
                    Console.WriteLine("CustomerDiscount: " + customerDiscountAmount);
                    Console.WriteLine("Total(Min CustomerDiscount): " + (subTotal - customerDiscountAmount));
                    Console.WriteLine("OrderDateTime: " + orderDateTime);
                    Console.WriteLine("Difference: " + differenceSubTotal);
                    Console.WriteLine("=======================");


                    var invoice1 = new Invoice();
                    invoice1.TableNumber = invoice.TableNumber;
                    invoice1.SubTotal = invoice.SubTotal;
                    invoice1.Total = invoice.Total;
                    invoice1.CustomerDiscount = invoice.CustomerDiscount;
                    //string ites = invoice.OrderDateTime.ToString("yyyy/MM/dd");
                    invoice1.OrderDateTime = invoice.OrderDateTime;
                    invoice1.DateCreated = DateTime.Now;
                    invoice1.TransactionRows = new List<TransactionRow>();
                    foreach (var row in invoice.TransactionRows)
                    {
                        var transactionRow1 = new TransactionRow();
                        transactionRow1.ProductDescription = row.ProductDescription;
                        transactionRow1.Price = row.Price;
                        transactionRow1.TransactionRowDiscount = row.TransactionRowDiscount;
                        transactionRow1.VatType = row.VatType;
                        transactionRow1.VatAmount = row.VatAmount;
                        invoice1.TransactionRows.Add(transactionRow1);

                    }
                    var isInvoice1AlreadyExists = _context.Invoice.Any(m => m.OrderDateTime == invoice.OrderDateTime);
                    if (isInvoice1AlreadyExists)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("opslaan is niet gelukt :( \ninvoice bestaat al");

                    }
                    else
                    {
                        _context.Add(invoice1);
                        _context.SaveChanges();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("opslaan is gelukt :)");
                    }

                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Er is geen Item");
            }
        }

        public void berekenen()
        {

        }
    }
}


