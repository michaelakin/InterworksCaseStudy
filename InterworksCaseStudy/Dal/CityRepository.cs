using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Etl.Core;

namespace InterworksCaseStudy.Helpers
{
    public static class CityRepository
    {
        private const string city_insert = @"INSERT INTO candidate2485.dim_city (city, state_id) VALUES (@city, @state_id);";
        private const string city_select = @"SELECT * FROM candidate2485.dim_city WHERE city = @city and state_id = @state_id";

        public static void AddCity(NpgsqlConnection conn, string city, string state, string airportName, Dictionary<string, Models.Dim_City> table, Dictionary<string, Models.Dim_State> stateDict)
        {
            if (city != string.Empty && !table.ContainsKey(city + state))
            {
                var tempState = state;
                if (tempState == string.Empty)
                {
                    var airportparts = airportName.Split(':');
                    if (airportparts.Any())
                        tempState = airportparts[0].Replace(city, "");
                }

                if (CityRepository.FindCity(conn, city, tempState, table, stateDict) == null)
                {
                    var cityState = StateRepository.FindState(conn, tempState, stateDict);
                    if (cityState != null)
                    {
                        // Write the state to the database.
                        conn.Execute(city_insert, new
                        {
                            city = city,
                            state_id = cityState.state_id
                        });

                        // find to Add to hash table.
                        CityRepository.FindCity(conn, city, tempState, table, stateDict);
                    }
                    else
                    {
                        Console.WriteLine($"Could not add City: {city}, State: {state}");
                    }
                }
            }
        }

        public static Models.Dim_City FindCity(NpgsqlConnection conn, string city, string state, Dictionary<string,Models.Dim_City> table, Dictionary<string, Models.Dim_State> stateDict)
        {
            var cityState = StateRepository.FindState(conn, state, stateDict);
            if (cityState == null) return null;

            var cachedCity = table.FirstOrDefault(w => w.Key == (city + state));
            if (cachedCity.Value != null) return cachedCity.Value;

            var result = conn.Query<Models.Dim_City>(city_select,
                new { city = city, state_id = cityState.state_id });

            // add to hash table
            if (result.Any() && !table.ContainsKey(city + state))
                table.Add(city + state,result.FirstOrDefault());

            return result.FirstOrDefault();
        }
    }
}
