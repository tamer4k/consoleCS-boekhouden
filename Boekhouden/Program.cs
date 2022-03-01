using Boekhouden.UI;
using ConsoleTables;
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
        private static void Main(string[] args)
        {
            var inlezen = new Inlezen_Json();
            var berekenen = new Berekenen();

            inlezen.InLezen();
            berekenen.berekenen();

            Console.WriteLine("Press <Enter> to exit... ");
            while (Console.ReadKey().Key != ConsoleKey.Enter)
            {
            }
        }
    }
}
