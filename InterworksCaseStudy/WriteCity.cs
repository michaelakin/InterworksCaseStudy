using InterworksCaseStudy.Dal;
using Npgsql;
using Rhino.Etl.Core;
using Rhino.Etl.Core.Operations;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;

namespace InterworksCaseStudy
{
    public class WriteCity : AbstractOperation
    {
        private readonly string conString = ConfigurationManager.ConnectionStrings["InterworksCaseStudy"].ConnectionString;
        private ConcurrentDictionary<string, Models.Dim_City> _dictCity ;
        private ConcurrentDictionary<string, Models.Dim_State> _dictState ;

        private WriteCity() { }
        public WriteCity(ConcurrentDictionary<string, Models.Dim_City> dictCity, ConcurrentDictionary<string, Models.Dim_State> dictState)
        {
            _dictCity = dictCity;
            _dictState = dictState;
        }

        public override IEnumerable<Row> Execute(IEnumerable<Row> rows)
        {
            using (var conn = new NpgsqlConnection(conString))
            {
                conn.Open();
                foreach (var row in rows)
                {
                    CityRepository.Add(conn, (string)row["OriginCityName"], (string)row["OriginState"], (string)row["OriginAirportName"], _dictCity, _dictState);
                    CityRepository.Add(conn, (string)row["DestCityName"], (string)row["DestState"], (string)row["DestAirportName"], _dictCity, _dictState);

                    yield return row;
                }
            }
        }
    }
}
