using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Etl.Core;
using System.Collections.Concurrent;

namespace InterworksCaseStudy.Helpers
{
    public static class CityRepository
    {
        private const string city_insert = @"INSERT INTO candidate2485.dim_city (city, state_id) VALUES (@city, @state_id);";
        private const string city_select = @"SELECT * FROM candidate2485.dim_city WHERE city = @city and state_id = @state_id";

        public static void Add(NpgsqlConnection conn, string city, string state, string airportName, ConcurrentDictionary<string, Models.Dim_City> table, ConcurrentDictionary<string, Models.Dim_State> stateDict)
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

                if (CityRepository.Find(conn, city, tempState, table, stateDict) == null)
                {
                    var cityState = StateRepository.Find(conn, tempState, stateDict);
                    if (cityState != null)
                    {
                        // Write the city to the database.
                        conn.Execute(city_insert, new
                        {
                            city = city,
                            state_id = cityState.state_id
                        });

                        // find to Add to hash table.
                        CityRepository.Find(conn, city, tempState, table, stateDict);
                    }
                    else
                    {
                        Console.WriteLine($"Could not add City: {city}, State: {state}");
                    }
                }
            }
        }

        public static Models.Dim_City Find(NpgsqlConnection conn, string city, string state, ConcurrentDictionary<string,Models.Dim_City> dictCity, ConcurrentDictionary<string, Models.Dim_State> stateDict)
        {
            var cityState = StateRepository.Find(conn, state, stateDict);
            if (cityState == null) return null;

            var cachedCity = dictCity.FirstOrDefault(w => w.Key == (city + state));
            if (cachedCity.Value != null) return cachedCity.Value;

            var result = conn.Query<Models.Dim_City>(city_select,
                new { city = city, state_id = cityState.state_id });

            // add to hash table
            if (result.Any() && !dictCity.ContainsKey(city + state))
                dictCity.TryAdd(city + state,result.FirstOrDefault());

            return result.FirstOrDefault();
        }
    }
}
