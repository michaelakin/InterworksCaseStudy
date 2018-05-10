
using Rhino.Etl.Core;
using Rhino.Etl.Core.ConventionOperations;
using Rhino.Etl.Core.Operations;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterworksCaseStudy
{
    public class WriteDataToConsole : AbstractOperation
    {
        //public WriteDataToConsole(ConnectionStringSettings connectionStringSettings) : base(connectionStringSettings)
        //{


        //}

        public override IEnumerable<Row> Execute(IEnumerable<Row> rows)
        {
            foreach (var row in rows)
            {
                //if ((bool)row["Diverted"])
                //    Console.WriteLine($"T {row["TransactionId"]}, Airline:{row["AirlineName"]}, Diverted:{row["Diverted"]}, Date:{row["FlightDate"]} ");
                yield return row;
            }
        }
    }
}
