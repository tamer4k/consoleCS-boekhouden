using System;
using System.Collections.Generic;
using System.Text;

namespace Boekhouden
{
    internal class ReadMe
    {
        // 1- om database te kunnen maken voor dit project 
        // na de clone ga maar naar Tools -> NnGet Package Manager - > Package Manager Console
        // schrijf na de PM> add-migration Naam
        // schrijf na de PM> update-database
        // ga maar naar View -> SQL Server Object Explorer daar na click op SQL Server -> localDb -> databases, ja kan nu de Boekhouden pDB database met tabellen en kolommen zien

        // 2- om facturen te kunnen halen out input Data file ga maar naar Json folder dan richt kliken  daarna properties kopieer de path en ga naar app config file en plaats daar de path 
        //  voor de output data hetzelfde path gebruiken maar een plaats van inputData.json verandert de naam naar output.json
    }
}
