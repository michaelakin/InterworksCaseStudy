using Rhino.Etl.Core;
using Rhino.Etl.Core.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Npgsql;
using Dapper;
using System.Data;
using InterworksCaseStudy.Helpers;
using System.Collections.Concurrent;

namespace InterworksCaseStudy
{
    public class WriteAirline : AbstractOperation
    {
        private readonly string conString = ConfigurationManager.ConnectionStrings["InterworksCaseStudy"].ConnectionString;
        private ConcurrentDictionary<string, Models.Dim_Airline> _dictAirline;

        private WriteAirline() { }
        public WriteAirline(ConcurrentDictionary<string, Models.Dim_Airline> dictAirline)
        {
            _dictAirline = dictAirline;
        }

        public override IEnumerable<Row> Execute(IEnumerable<Row> rows)
        {
            using (var conn = new NpgsqlConnection(conString))
            {
                conn.Open();
                foreach (var row in rows)
                {
                    AirlineRepository.Add(conn, (string)row["AIRLINECODE"], (string)row["AIRLINENAME"], _dictAirline);
                    
                    yield return row;
                }
            }
        }


    }
}
