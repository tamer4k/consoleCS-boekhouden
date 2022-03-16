using Boekhouden.Domains.DTO;
using ConsoleTables;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Boekhouden.UI
{
    public static class OutputUI
    {
        private static List<Invoice> RootObject(string file)
        {
            return File.Exists(file) ? JsonSerializer.Deserialize<List<Invoice>>(File.ReadAllText(file)) : null;
        }


        private static void WriteJson(List<DagTotaal> value, string path)
        {
            var options = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
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


        private static void UitPrint(DagTotaal dagTotaal)
        {
            var table = new ConsoleTable(
               "C&D",
               "Datum",
               "GrootboekRekening",
               "Grootboek"
               );


            Console.ForegroundColor = ConsoleColor.Yellow;

            table.AddRow(
                dagTotaal.GeldOntvangen.Debet,
                dagTotaal.GeldOntvangen.Datum.ToString("d-M-yy"),
                dagTotaal.GeldOntvangen.GrootboekRekening,
                dagTotaal.GeldOntvangen.Grootboek
            );

            if (dagTotaal.OmzetBtwLaag.Credit != 0)
            {
                table.AddRow(
                    dagTotaal.OmzetBtwLaag.Credit,
                    dagTotaal.OmzetBtwLaag.Datum.ToString("d-M-yy"),
                    dagTotaal.OmzetBtwLaag.GrootboekRekening,
                    dagTotaal.OmzetBtwLaag.Grootboek
                );
            }
            if (dagTotaal.OmzetBtwHoog.Credit != 0)
            {
                table.AddRow(
                    dagTotaal.OmzetBtwHoog.Credit,
                    dagTotaal.OmzetBtwHoog.Datum.ToString("d-M-yy"),
                    dagTotaal.OmzetBtwHoog.GrootboekRekening,
                    dagTotaal.OmzetBtwHoog.Grootboek
                );
            }
            if (dagTotaal.BtwLaag.Credit != 0)
            {
                table.AddRow(
                    dagTotaal.BtwLaag.Credit,
                    dagTotaal.BtwLaag.Datum.ToString("d-M-yy"),
                    dagTotaal.BtwLaag.GrootboekRekening,
                    dagTotaal.BtwLaag.Grootboek
                );
            }
            if (dagTotaal.BtwHoog.Credit != 0)
            {
                table.AddRow(
                    dagTotaal.BtwHoog.Credit,
                    dagTotaal.BtwHoog.Datum.ToString("d-M-yy"),
                    dagTotaal.BtwHoog.GrootboekRekening,
                    dagTotaal.BtwHoog.Grootboek
                );
            }
            if (dagTotaal.OmzetGeenBTW.Credit != 0)
            {
                table.AddRow(
                     dagTotaal.OmzetGeenBTW.Credit,
                     dagTotaal.OmzetGeenBTW.Datum.ToString("d-M-yy"),
                     dagTotaal.OmzetGeenBTW.GrootboekRekening,
                     dagTotaal.OmzetGeenBTW.Grootboek
                 );

            }

            table.Options.EnableCount = false; 
            table.Write();
            Console.WriteLine();


        }


        private static FactuurTotaal Berekenen(Invoice invoice)
        {

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
            double subtotalNotVatMintPersentage = subTotalNoVat - Math.Round(subTotalNoVat / 100 * customerPercentage, 2);
            double subtotaVatLowAmountlMinPercentage = subTotalInclLowVat - Math.Round(subTotalInclLowVat / 100 * customerPercentage, 2);
            double subtotaVatHighAmountlMinPercentage = subTotalInclHighVat - Math.Round(subTotalInclHighVat / 100 * customerPercentage, 2);
            double transactionRowDiscountToale = invoice.TransactionRows.Sum(tr => tr.TransactionRowDiscount); // totaal regels discount
            double subTotalNoLowVatNoHighVat = Math.Round(subtotalNotVatMintPersentage, 2);
            double subTotalExclLowVat = Math.Round(subtotaVatLowAmountlMinPercentage / 1.09, 2);
            double subTotalExclHighVat = Math.Round(subtotaVatHighAmountlMinPercentage / 1.21, 2);
            double totalVatLow = Math.Round(subtotaVatLowAmountlMinPercentage - subTotalExclLowVat, 2);
            double totalVatHigh = Math.Round(subtotaVatHighAmountlMinPercentage - subTotalExclHighVat, 2);

            //double prijsTotal = invoice.TransactionRows.Sum(tr => tr.Price); // totaal prijzen
            var orderDateTime = invoice.OrderDateTime;
            double debit = Math.Round(subTotalExclLowVat + subTotalExclHighVat + totalVatLow + totalVatHigh + subTotalNoLowVatNoHighVat, 2);

            var resultaat = new FactuurTotaal();

            resultaat.GeldOntvangen = new DebetRegel(debit, orderDateTime, 1300, "Geld ontvangen");
            resultaat.OmzetBtwLaag = new CreditRegel(subTotalExclLowVat, orderDateTime, 8210, "Omzet Laag (diensten)");
            resultaat.OmzetBtwHoog = new CreditRegel(subTotalExclHighVat, orderDateTime, 8200, "Omzet Hoog (diensten)");
            resultaat.BtwLaag = new CreditRegel(totalVatLow, orderDateTime, 1670, "Btw Laag");
            resultaat.BtwHoog = new CreditRegel(totalVatHigh, orderDateTime, 1671, "Btw Hoog");
            resultaat.OmzetGeenBTW = new CreditRegel(subTotalNoLowVatNoHighVat, orderDateTime, 2222, "GeenBTW (diensten)");
            return resultaat;
        }



        public static void Output(string inputFile, string outputFile)
        {
            if (!File.Exists(inputFile))
            {
                throw new IOException($"Bestand {inputFile} niet gevonden!");
            }

            var invoices = RootObject(inputFile);
            if (invoices != null)
            {
                var samenvatting = new List<DagTotaal>();

                foreach (IGrouping<DateTime, Invoice> invoicesPerDay in invoices.GroupBy(x => x.OrderDateTime.Date).OrderByDescending(o => o.Key).ToArray())
                {

                    var dagTotaal = new DagTotaal();

                    foreach (var invoice in invoicesPerDay)
                    {

                        var totaal = Berekenen(invoice);

                        dagTotaal.GeldOntvangen.Debet += totaal.GeldOntvangen.Debet;
                        dagTotaal.GeldOntvangen.GrootboekRekening = totaal.GeldOntvangen.GrootboekRekening;
                        dagTotaal.GeldOntvangen.Datum = totaal.GeldOntvangen.Datum;
                        dagTotaal.GeldOntvangen.Grootboek = totaal.GeldOntvangen.Grootboek;


                        dagTotaal.OmzetBtwHoog.Credit += totaal.OmzetBtwHoog.Credit;
                        dagTotaal.OmzetBtwHoog.GrootboekRekening = totaal.OmzetBtwHoog.GrootboekRekening;
                        dagTotaal.OmzetBtwHoog.Datum = totaal.OmzetBtwHoog.Datum;
                        dagTotaal.OmzetBtwHoog.Grootboek = totaal.OmzetBtwHoog.Grootboek;

                        dagTotaal.OmzetBtwLaag.Credit += totaal.OmzetBtwLaag.Credit;
                        dagTotaal.OmzetBtwLaag.GrootboekRekening = totaal.OmzetBtwLaag.GrootboekRekening;
                        dagTotaal.OmzetBtwLaag.Datum = totaal.OmzetBtwLaag.Datum;
                        dagTotaal.OmzetBtwLaag.Grootboek = totaal.OmzetBtwLaag.Grootboek;

                        dagTotaal.BtwHoog.Credit += totaal.BtwHoog.Credit;
                        dagTotaal.BtwHoog.GrootboekRekening = totaal.BtwHoog.GrootboekRekening;
                        dagTotaal.BtwHoog.Datum = totaal.BtwHoog.Datum;
                        dagTotaal.BtwHoog.Grootboek = totaal.BtwHoog.Grootboek;

                        dagTotaal.BtwLaag.Credit += totaal.BtwLaag.Credit;
                        dagTotaal.BtwLaag.GrootboekRekening = totaal.BtwLaag.GrootboekRekening;
                        dagTotaal.BtwLaag.Datum = totaal.BtwLaag.Datum;
                        dagTotaal.BtwLaag.Grootboek = totaal.BtwLaag.Grootboek;

                        dagTotaal.OmzetGeenBTW.Credit += totaal.OmzetGeenBTW.Credit;
                        dagTotaal.OmzetGeenBTW.GrootboekRekening = totaal.OmzetGeenBTW.GrootboekRekening;
                        dagTotaal.OmzetGeenBTW.Datum = totaal.OmzetGeenBTW.Datum;
                        dagTotaal.OmzetGeenBTW.Grootboek = totaal.OmzetGeenBTW.Grootboek;

                    }
                    UitPrint(dagTotaal);

                    if (dagTotaal.OmzetBtwHoog.Credit == 0)
                    {
                        dagTotaal.OmzetBtwHoog = null;
                    }
                    if (dagTotaal.OmzetBtwLaag.Credit == 0)
                    {
                        dagTotaal.OmzetBtwLaag = null;
                    }
                    if (dagTotaal.BtwHoog.Credit == 0)
                    {
                        dagTotaal.BtwHoog = null;
                    }
                    if (dagTotaal.BtwLaag.Credit == 0)
                    {
                        dagTotaal.BtwLaag = null;
                    }
                    if (dagTotaal.OmzetGeenBTW.Credit == 0)
                    {
                        dagTotaal.OmzetGeenBTW = null;
                    }

                    samenvatting.Add(dagTotaal);

                }
                WriteJson(samenvatting, outputFile);
            }
        }



    }


}
