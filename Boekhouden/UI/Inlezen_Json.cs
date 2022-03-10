using Boekhouden.Data;
using Boekhouden.UI;
using ConsoleTables;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Boekhouden
{
    public class Inlezen_Json
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
                    
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    var table = new ConsoleTable("TableNumber", "SubTotal", "CustomerDiscount", "Total", "OrderDateTime");
                    table.AddRow(invoice.TableNumber, invoice.SubTotal, invoice.CustomerDiscount.DiscountAmount, invoice.Total, invoice.OrderDateTime.ToString("dd/MM/yyyy"));
                    table.Write();
                    Console.WriteLine();

                    //Json file uitprinten

                    //Console.ForegroundColor = ConsoleColor.DarkYellow;
                    //Console.WriteLine("TableNumber: " + invoice.TableNumber + " ");
                    //Console.WriteLine("SubTotal: " + invoice.SubTotal);
                    //Console.WriteLine("CustomerDiscount: " + invoice.CustomerDiscount.DiscountAmount);
                    //Console.WriteLine("Total: " + invoice.Total);
                    //Console.WriteLine("OrderDateTime: " + invoice.OrderDateTime.ToString("dd/MM/yyyy"));
                    //Console.WriteLine("=======================");

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

                    bool gelukt = false;
                    string letter = "";
                    while (!gelukt)
                    {
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.WriteLine("Wil de gegevens opslaan? (y voor JA) of (n voor NEE)");
                        string invoer = Console.ReadLine();
                        try
                        {
                            letter = invoer;
                            if (letter == "y")
                            {
                                if (isInvoice1AlreadyExists)
                                {
                                    Console.ForegroundColor = ConsoleColor.DarkRed;
                                    Console.WriteLine("opslaan is niet gelukt :(  invoice bestaat al");

                                }
                                else
                                {
                                    _context.Add(invoice1);
                                    _context.SaveChanges();
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine("opslaan is gelukt :)");
                                }
                                gelukt = true;
                            }
                            else if (letter == "n")
                            {
                                gelukt = true;
                            }
                            else
                            {
                                Console.WriteLine("Kies JA of NEE");
                            }
                        }
                        catch (Exception)
                        {

                            Console.WriteLine("Kies JA of NEE AUB");
                        }
                    }
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Er is geen Item");
            }
        }
    }
}


