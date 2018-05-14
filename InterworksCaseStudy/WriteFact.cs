using InterworksCaseStudy.Helpers;
using Npgsql;
using Rhino.Etl.Core;
using Rhino.Etl.Core.Operations;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;

namespace InterworksCaseStudy
{
    public class WriteFact : AbstractOperation
    {
        private readonly string conString = ConfigurationManager.ConnectionStrings["InterworksCaseStudy"].ConnectionString;
        private ConcurrentDictionary<string, Models.Dim_Airline> _dictAirline;
        private ConcurrentDictionary<string, Models.Dim_Airport> _dictAirport;
        private ConcurrentDictionary<string, Models.Dim_City> _dictCity;
        private ConcurrentDictionary<string, Models.Dim_State> _dictState;
        private ConcurrentDictionary<string, Models.Dim_Tail> _dictTail;


        private WriteFact() { }
        public WriteFact(ConcurrentDictionary<string, Models.Dim_Airline> dictAirline,
            ConcurrentDictionary<string, Models.Dim_Airport> dictAirport,
            ConcurrentDictionary<string, Models.Dim_City> dictCity,
            ConcurrentDictionary<string, Models.Dim_State> dictState,
            ConcurrentDictionary<string, Models.Dim_Tail> dictTail)
        {
            _dictAirline = dictAirline;
            _dictAirport = dictAirport;
            _dictCity = dictCity;
            _dictState = dictState;
            _dictTail = dictTail;
            
        }

        public override IEnumerable<Row> Execute(IEnumerable<Row> rows)
        {
            using (var conn = new NpgsqlConnection(conString))
            {
                conn.Open();
                foreach (var row in rows)
                {
                    FactRepository.Add(conn, row, _dictAirline, _dictAirport, _dictCity, _dictState, _dictTail); 
                    yield return row;
                }
            }
        }
    }
}
