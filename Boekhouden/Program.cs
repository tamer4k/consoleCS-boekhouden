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
            Inlezen_Json J = new Inlezen_Json();
            J.InLezen();
            Console.ReadKey();

        }     
    }
}
