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
        static void Main(string[] args)
        {
            Inlezen_Json j = new Inlezen_Json();
            j.InLezen();


            Console.WriteLine("Press <Enter> to exit... ");
            while (Console.ReadKey().Key != ConsoleKey.Enter) { }

        }
    }
}
