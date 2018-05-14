using InterworksCaseStudy.Dal;
using Npgsql;
using Rhino.Etl.Core;
using Rhino.Etl.Core.Operations;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;

namespace InterworksCaseStudy
{
    public class WriteState : AbstractOperation
    {
        private readonly string conString = ConfigurationManager.ConnectionStrings["InterworksCaseStudy"].ConnectionString;
        private ConcurrentDictionary<string, Models.Dim_State> _stateDict = new ConcurrentDictionary<string, Models.Dim_State>();

        private WriteState() { }
        public WriteState(ConcurrentDictionary<string, Models.Dim_State> stateDict)
        {
            _stateDict = stateDict;
        }

        public override IEnumerable<Row> Execute(IEnumerable<Row> rows)
        {
            using (var conn = new NpgsqlConnection(conString))
            {
                conn.Open();
                foreach (var row in rows)
                {
                    StateRepository.Add(conn, (string)row["OriginState"], (string)row["OriginStateName"], (string)row["OriginAirportName"], (string)row["OriginCityName"], _stateDict);
                    StateRepository.Add(conn, (string)row["DestState"], (string)row["DestStateName"], (string)row["DestAirportName"], (string)row["DestCityName"], _stateDict);

                    yield return row;
                }
            }
        }
    }
}
