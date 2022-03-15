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
            var inputJson = @"C:\Users\talashraf\OneDrive - SnelStart Software B.V\Bureaublad\C#\Boekhouden\Boekhouden\json\inputData.json";
            var outputJson = @"C:\Users\talashraf\OneDrive - SnelStart Software B.V\Bureaublad\C#\Boekhouden\Boekhouden\Json\output.json";
            var outputUI = new OutputUI();
            outputUI.Output(inputJson, outputJson);
            
            //var inlezen = new Inlezen_Json();
            //inlezen.InLezen();

            Console.WriteLine("Press <Enter> to exit... ");
            while (Console.ReadKey().Key != ConsoleKey.Enter)
            {
            }

        }
    }
}
