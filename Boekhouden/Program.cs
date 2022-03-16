using Boekhouden.UI;
using ConsoleTables;
using Microsoft.IdentityModel.Protocols;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
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
            var inputString = ConfigurationManager.AppSettings["inputData"];
            var outputString = ConfigurationManager.AppSettings["outputData"];

            var inputJson = inputString;
            var outputJson = outputString;


            var inlezen = new Inlezen_Json();
            inlezen.InLezen();
            Console.WriteLine("======================\n\n\n");


            OutputUI.Output(inputJson, outputJson); 

            //var outputUI = new OutputUI();
            //outputUI.Output(inputJson, outputJson);

            Console.WriteLine("Press <Enter> to exit... ");
            while (Console.ReadKey().Key != ConsoleKey.Enter)
            {
            }

        }
    }
}
