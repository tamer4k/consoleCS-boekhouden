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
        public char Sort { get; set; }
        public double Sum1 { get; set; }
        public double Sum2 { get; set; }
        public double Calck()
        {
            switch (Sort)
            {
                case '+':
                    return (Sum1 + Sum2);
                case '-':
                    return (Sum1 - Sum2);
                default:
                    throw new ArgumentException("The gender argument is not valid");
            }
            //var invoices = RootObject();
            //foreach (var invoice in invoices)
            //{
            //    Total = invoice.Total;
            //}
        }



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
                return transactionRow;
            }
            else
            {
                return null;
            }
    
        }
        public  void InLezen()
        {
            var invoices = RootObject();
            if (invoices != null)
            {
                foreach (var invoice in invoices)
                {
                    Console.WriteLine("\n");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    var table = new ConsoleTable("TableNumber", "SubTotal", "CustomerDiscount", "Total", "OrderDateTime");
                    table.AddRow(invoice.TableNumber, invoice.SubTotal, invoice.CustomerDiscount.DiscountAmount, invoice.Total, invoice.OrderDateTime.ToString("dd/MM/yyyy/HH:mm"));
                    table.Write();
                    Console.WriteLine("\n");

                    var invoice1 = new Invoice();
                    invoice1.TableNumber = invoice.TableNumber;
                    invoice1.SubTotal = invoice.SubTotal;
                    invoice1.Total = invoice.Total;
                    invoice1.CustomerDiscount = invoice.CustomerDiscount;
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
                                    Console.WriteLine("\n");
                                    Console.WriteLine("opslaan is niet gelukt :(  invoice bestaat al");

                                }
                                else
                                {
                                    _context.Add(invoice1);
                                    _context.SaveChanges();
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine("\n");
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
                                Console.WriteLine("\n");
                                Console.WriteLine("Kies JA of NEE");
                            }
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("\n");
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


