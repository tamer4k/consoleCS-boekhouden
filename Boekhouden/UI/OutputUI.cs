using Boekhouden.Domains.DTO;
using ConsoleTables;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Boekhouden.UI
{
    public class OutputUI
    {
        private static List<Invoice> RootObject(string file)
        {
            return File.Exists(file) ? JsonSerializer.Deserialize<List<Invoice>>(File.ReadAllText(file)) : null;
        }

        private static void WriteJson(List<DagTotaal> value, string path)
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new DateTimeConverterUsingDateTimeParse());
            string JSONresult = JsonSerializer.Serialize(value, options);

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            using (var js = new StreamWriter(path, true))
            {
                js.WriteLine(JSONresult.ToString());
            }
        }

        public void Output(string inputFile, string outputFile)
        {
            if (!File.Exists(inputFile))
            {
                throw new IOException($"Bestand {inputFile} niet gevonden!");
            }

            var invoices = RootObject(inputFile);
            if (invoices != null)
            {
                var samenvatting = new List<DagTotaal>();

                foreach (var invoicesPerDay in invoices.GroupBy(x => x.OrderDateTime.Date).ToArray())
                {
                    var dagTotaal = new DagTotaal();



                    var table = new ConsoleTable(
                                "C&D",
                                "Datum",
                                "GrootboekRekening",
                                "Grootboek"
                                );
                    foreach (var invoice in invoicesPerDay)
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
                        double customerPercentage = invoice.CustomerDiscount.Percentage;
                        double subtotaVatLowAmountlMinPercentage = subTotalInclLowVat - Math.Round(subTotalInclLowVat / 100 * customerPercentage, 2);
                        double subtotaVatHighAmountlMinPercentage = subTotalInclHighVat - Math.Round(subTotalInclHighVat / 100 * customerPercentage, 2);
                        double transactionRowDiscountToale = invoice.TransactionRows.Sum(tr => tr.TransactionRowDiscount); // totaal regels discount
                        double subTotalExclLowVat = Math.Round(subtotaVatLowAmountlMinPercentage / 1.09, 2);
                        double subTotalExclHighVat = Math.Round(subtotaVatHighAmountlMinPercentage / 1.21, 2);
                        double totalVatLow = Math.Round(subtotaVatLowAmountlMinPercentage - subTotalExclLowVat, 2);
                        double totalVatHigh = Math.Round(subtotaVatHighAmountlMinPercentage - subTotalExclHighVat, 2);

                        //double prijsTotal = invoice.TransactionRows.Sum(tr => tr.Price); // totaal prijzen
                        var orderDateTime = invoice.OrderDateTime;
                        double debit = Math.Round(subTotalExclLowVat + subTotalExclHighVat + totalVatLow + totalVatHigh, 2);

                        dagTotaal.GeldOntvangen = new DebetRegel(debit, orderDateTime, 1300, "Geld ontvangen");
                        dagTotaal.OmzetBtwLaag = new CreditRegel(subTotalExclLowVat, orderDateTime, 8210, "Omzet Laag (diensten)");
                        dagTotaal.OmzetBtwHoog = new CreditRegel(subTotalExclHighVat, orderDateTime, 8200, "Omzet Hoog (diensten)");
                        dagTotaal.BtwLaag = new CreditRegel(totalVatLow, orderDateTime, 1670, "Btw Laag");
                        dagTotaal.BtwHoog = new CreditRegel(totalVatHigh, orderDateTime, 1671, "Btw Hoog");



                        Console.ForegroundColor = ConsoleColor.Yellow;
                        table.AddRow(
                            dagTotaal.GeldOntvangen.Debet,
                            dagTotaal.GeldOntvangen.Datum.ToString("d-M-yy"),
                            dagTotaal.GeldOntvangen.GrootboekRekening,
                            dagTotaal.GeldOntvangen.Grootboek
                        );
                        table.AddRow(
                            dagTotaal.OmzetBtwLaag.Credit,
                            dagTotaal.OmzetBtwLaag.Datum.ToString("d-M-yy"),
                            dagTotaal.OmzetBtwLaag.GrootboekRekening,
                            dagTotaal.OmzetBtwLaag.Grootboek
                        );
                        table.AddRow(
                            dagTotaal.OmzetBtwHoog.Credit,
                            dagTotaal.OmzetBtwHoog.Datum.ToString("d-M-yy"),
                            dagTotaal.OmzetBtwHoog.GrootboekRekening,
                            dagTotaal.OmzetBtwHoog.Grootboek
                        );
                        table.AddRow(
                            dagTotaal.BtwLaag.Credit,
                            dagTotaal.BtwLaag.Datum.ToString("d-M-yy"),
                            dagTotaal.BtwLaag.GrootboekRekening,
                            dagTotaal.BtwLaag.Grootboek
                        );
                        table.AddRow(
                            dagTotaal.BtwHoog.Credit,
                            dagTotaal.BtwHoog.Datum.ToString("d-M-yy"),
                            dagTotaal.BtwHoog.GrootboekRekening,
                            dagTotaal.BtwHoog.Grootboek
                        );

                        
                    }
                    samenvatting.Add(dagTotaal);
                    table.Write();

                }
                WriteJson(samenvatting, outputFile);
            }
        }
    }
}
