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
    public class WriteAirport : AbstractOperation
    {
        private readonly string conString = ConfigurationManager.ConnectionStrings["InterworksCaseStudy"].ConnectionString;
        private ConcurrentDictionary<string, Models.Dim_City> _dictCity;
        private ConcurrentDictionary<string, Models.Dim_State> _dictState;
        private ConcurrentDictionary<string, Models.Dim_Airport> _dictAirport;

        private WriteAirport() { }
        public WriteAirport(ConcurrentDictionary<string, Models.Dim_Airport> dictAirport, 
            ConcurrentDictionary<string, Models.Dim_City> dictCity, 
            ConcurrentDictionary<string, Models.Dim_State> dictState)
        {
            _dictCity = dictCity;
            _dictState = dictState;
            _dictAirport = dictAirport;
        }

        public override IEnumerable<Row> Execute(IEnumerable<Row> rows)
        {
            using (var conn = new NpgsqlConnection(conString))
            {
                conn.Open();
                foreach (var row in rows)
                {
                    AirportRepository.Add(conn, (string)row["OriginAirportName"], (string)row["OriginCityName"], (string)row["OriginState"], _dictAirport, _dictCity, _dictState);
                    AirportRepository.Add(conn, (string)row["DestAirportName"], (string)row["DestCityName"], (string)row["DestState"], _dictAirport, _dictCity, _dictState);
                    
                    yield return row;
                }
            }
        }


    }
}
