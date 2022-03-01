using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ConsoleTables;
using Newtonsoft.Json;

namespace Boekhouden.UI
{
    public class Berekenen
    {
        private static List<Invoice> RootObject()
        {
            var Json = @"C:\Users\talashraf\OneDrive - SnelStart Software B.V\Bureaublad\C#\Boekhouden\Boekhouden\json\inputData.json";
            if (File.Exists(Json))
            {
                var transactionRow = JsonConvert.DeserializeObject<List<Invoice>>(File.ReadAllText(Json));
                //string json = System.Text.Json.JsonSerializer.Serialize(transactionRow.ToArray());
                //File.WriteAllText(Json, json);
                return transactionRow;
            }

            return null;
        }

        public void berekenen()
        {
            var invoices = RootObject();

            if (invoices != null)
            {
                var table = new ConsoleTable(
                    "SubTotal incl btw",
                    "SubTotal excl btw",
                    "TotalVatAmount",
                    "VatAmount 9%",
                    "VatAmount 21%",
                    "CustomerDiscount: ",
                    "Total(Min CustomerDiscount): ",
                    "OrderDateTime: ",
                    "Difference"
                );

                foreach (var invoice in invoices)
                {
                    // Json file berekenen
                    double subTotalNoVat = 0; // vatType 0 btw is 0%
                    double subTotalInclLowVat = 0; // vatType 1 btw 9%
                    double subTotalInclHighVat = 0; // vatType 2 btw 21%
                    foreach (var row in invoice.TransactionRows)
                    {
                        double subTotaal = row.Price - row.TransactionRowDiscount; // totaal prijzen min totaal regels discount
                        int vatType = row.VatType;
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

                    double subTotalExclLowVat = Math.Round(subTotalInclLowVat / 1.09, 2);
                    double subTotalExclHighVat = Math.Round(subTotalInclHighVat / 1.21, 2);
                    double totalVatLow = Math.Round(subTotalInclLowVat - subTotalExclLowVat, 2);
                    double totalVatHigh = Math.Round(subTotalInclHighVat - subTotalExclHighVat, 2);

                    double prijsTotal = invoice.TransactionRows.Sum(tr => tr.Price); // totaal prijzen
                    double transactionRowDiscountToale = invoice.TransactionRows.Sum(tr => tr.TransactionRowDiscount); // totaal regels discount
                    int customerPercentage = invoice.CustomerDiscount.Percentage;
                    var orderDateTime = invoice.OrderDateTime.ToString("dd/MM/yyyy");
                    double subTotal = Math.Round(subTotalExclLowVat + subTotalExclHighVat + totalVatLow + totalVatHigh,2); // totaal prijzen min totaal regels discount
                    double totalVatAmount = Math.Round(totalVatLow + totalVatHigh, 2);
                    double customerDiscountAmount = Math.Round(subTotal / 100 * customerPercentage, 2); // customer discount is subtotal keer precentage
                    double total = subTotal - customerDiscountAmount; //var totaal = SubTotal - CustomerDiscountAmount;
                    double differenceSubTotal = invoice.SubTotal - subTotal;

                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    //  situatie: transactieregels met btw Laag EN btw hoog
                    //1 Debetregel: Totaal
                    //1 Creditregel: Omzet btw laag(ZONDER btw)
                    //1 Creditregel: Btw laag
                    //1 Creditregel: Omzet btw hoog(ZONDER btw)
                    //1 Creditregel: Btw hoog

                    //situatie: transactieregels met btw hoog en btw GEEN
                    //1 Debetregel: Totaal
                    //1 Creditregel: Omzet btw GEEN
                    //1 Creditregel: Omzet btw hoog(ZONDER btw)
                    //1 Creditregel: Btw hoog

                    table.AddRow(
                        subTotal,
                        subTotal - totalVatAmount,
                        totalVatAmount,
                        totalVatLow,
                        totalVatHigh,
                        customerDiscountAmount,
                        subTotal - customerDiscountAmount,
                        orderDateTime,
                        differenceSubTotal
                    );
                }

                table.Write();
            }
        }
    }
}
