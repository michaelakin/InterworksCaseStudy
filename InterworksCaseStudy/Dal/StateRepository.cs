using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterworksCaseStudy.Helpers
{
    public static class StateRepository
    {
        private const string state_insert = @"INSERT INTO candidate2485.dim_state (abbrev, name) VALUES (@abbr, @name);";
        private const string state_select = @"SELECT * FROM candidate2485.dim_state WHERE abbrev = @abbr";

        public static void AddState(NpgsqlConnection conn, string stateAbbr, string stateName, string airportName, string city, Dictionary<string, Models.Dim_State> dict)
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
                && FindState(conn, tempState, dict) == null)
            {
                //if (stateName != string.Empty)
                //{
                    // Write the state to the database.
                    conn.Execute(state_insert, new
                    {
                        abbr = tempState,
                        name = stateName
                    });

                    // Find to add to has table.
                    FindState(conn, tempState, dict);
                //}
                //else
                //{
                //    Console.WriteLine($"Invalid State Data: Abbrev: {tempState}, Name: {stateName}");
                //}
            }
        }

        public static Models.Dim_State FindState(NpgsqlConnection conn, string abbrev, Dictionary<string, Models.Dim_State> dict)
        {
            // Try to find in dictionary first
            var hashState = dict.FirstOrDefault(w => w.Key == abbrev);
            if (hashState.Value != null)
                return  hashState.Value ;
            
            var result = conn.Query<Models.Dim_State>(state_select, new { abbr = abbrev });

            // add to hash table
            if (result.Any())
                dict.Add(result.First().abbrev, result.First());
            return result.FirstOrDefault();
        }
    }
}
