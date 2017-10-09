using Hydra.Such.Data.Logic;
using Hydra.Such.Data.Logic.Project;
using Hydra.Such.Data.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Dynamic;

namespace Hydra.Such.Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            //NumerationTest();

            StoredProcedureTeste();
        }

        public static void NumerationTest()
        {
            string numeration = DBNumerationConfigurations.GetNextNumeration(1);
            string numeration2 = DBNumerationConfigurations.GetNextNumeration(2);
            string numeration3 = DBNumerationConfigurations.GetNextNumeration(3);
            
            //Console.WriteLine(numeration);
            //Console.WriteLine(numeration2);
            //Console.WriteLine(numeration3);

            Console.ReadLine();
        }


        public static void StoredProcedureTeste()
        {
            var x = DBNAV2017ShippingAddresses.GetAll("SUCH_NAV_DEV", "CRONUS Portugal Ltd_");

            Console.ReadLine();
        }

        
    }
}
