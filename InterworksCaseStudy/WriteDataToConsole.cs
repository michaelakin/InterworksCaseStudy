
using Rhino.Etl.Core;
using Rhino.Etl.Core.Operations;
using System;
using System.Collections.Generic;

namespace InterworksCaseStudy
{
    public class WriteDataToConsole : AbstractOperation
    {
        public override IEnumerable<Row> Execute(IEnumerable<Row> rows)
        {
            foreach (var row in rows)
            {
                //if ((bool)row["Diverted"])
                    Console.WriteLine($"C: T {row["TransactionId"]}, Airline:{row["AirlineName"]}, Diverted:{row["Diverted"]}, Date:{row["FlightDate"]} ");
                yield return row;
            }
        }
    }
}
