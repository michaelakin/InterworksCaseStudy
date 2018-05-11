using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Diagnostics;

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

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            var etl = new Etl(settings, flat_file_location);
            etl.Execute();

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Console.WriteLine("RunTime " + elapsedTime);
            Console.WriteLine("done, hit a key to quit");
            Console.ReadKey();
        }
    }
}
