using Boekhouden.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Boekhouden.UI
{
    public class Berekenen
    {

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

        public void berekenen()
        {
            var invoices = RootObject();

            if (invoices != null)
            {
                foreach (var invoice in invoices)
                {
                    // Json file berekenen
                    double subTotalNoVat = 0; // vatType 0 btw is 0%
                    double subTotalInclLowVat = 0; // vatType 1 btw 9%
                    double subTotalInclHighVat = 0; // vatType 2 btw 21%
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
                    var subTotalExclLowVat = Math.Round(subTotalInclLowVat / 1.09, 2);
                    var subTotalExclHighVat = Math.Round(subTotalInclHighVat / 1.21, 2);
                    var totalVatLow = Math.Round(subTotalInclLowVat - subTotalExclLowVat, 2);
                    var totalVatHigh = Math.Round(subTotalInclHighVat - subTotalExclHighVat, 2);

                    var prijsTotal = invoice.TransactionRows.Sum(tr => tr.Price); // totaal prijzen
                    var transactionRowDiscountToale = invoice.TransactionRows.Sum(tr => tr.TransactionRowDiscount); // totaal regels discount
                    var customerPercentage = invoice.CustomerDiscount.Percentage;
                    var orderDateTime = invoice.OrderDateTime.ToString("dd/MM/yyyy");
                    var subTotal = Math.Round(subTotalExclLowVat + subTotalExclHighVat + totalVatLow + totalVatHigh, 2); // totaal prijzen min totaal regels discount
                    var totalVatAmount = Math.Round(totalVatLow + totalVatHigh, 2);
                    var customerDiscountAmount = Math.Round(subTotal / 100 * customerPercentage, 2); // customer discount is subtotal keer precentage
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
                }
            }
        }
    }
}
