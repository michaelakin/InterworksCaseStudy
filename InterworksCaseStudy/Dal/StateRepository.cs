using Dapper;
using Npgsql;
using System.Collections.Concurrent;
using System.Linq;

namespace InterworksCaseStudy.Dal
{
    public static class StateRepository
    {
        private const string state_insert = @"INSERT INTO candidate2485.dim_state (abbrev, name) VALUES (@abbr, @name);";
        private const string state_select = @"SELECT * FROM candidate2485.dim_state WHERE abbrev = @abbr";

        public static void Add(NpgsqlConnection conn, string stateAbbr, string stateName, string airportName, string city, ConcurrentDictionary<string, Models.Dim_State> dict)
        {
            var tempState = stateAbbr;
            if (tempState == string.Empty)
            {
                var airportparts = airportName.Split(':');
                if (airportparts.Any())
                    tempState = airportparts[0].Replace(city, "");
            }

            if (tempState != string.Empty
                && !dict.ContainsKey(tempState)
                && Find(conn, tempState, dict) == null)
            {
                // Write the state to the database.
                conn.Execute(state_insert, new
                {
                    abbr = tempState,
                    name = stateName
                });

                // Find to add to has table.
                Find(conn, tempState, dict);
            }
        }

        public static Models.Dim_State Find(NpgsqlConnection conn, string abbrev, ConcurrentDictionary<string, Models.Dim_State> dict)
        {
            // Try to find in ConcurrentDictionary first
            var hashState = dict.FirstOrDefault(w => w.Key == abbrev);
            if (hashState.Value != null)
                return hashState.Value;

            var result = conn.Query<Models.Dim_State>(state_select, new { abbr = abbrev });

            // add to hash table
            if (result.Any())
                dict.TryAdd(result.First().abbrev, result.First());
            return result.FirstOrDefault();
        }
    }
}
