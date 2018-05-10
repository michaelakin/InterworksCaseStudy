using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace InterworksCaseStudy
{
    class Program
    {


        static void Main(string[] args)
        {
            ConnectionStringSettings settings =
                ConfigurationManager.ConnectionStrings["InterworksCaseStudy.Properties.Settings.tests_data_engineeringConnectionString"];

            string con_string = string.Empty;
            string flat_file_location = @"C:\Users\Michael\Downloads\flights\flights.txt";
            // If found, return the connection string.
            if (settings != null)
                con_string = settings.ConnectionString;


            var etl = new Etl(settings, flat_file_location);
            etl.Execute();
            Console.ReadKey();
        }
    }
}
