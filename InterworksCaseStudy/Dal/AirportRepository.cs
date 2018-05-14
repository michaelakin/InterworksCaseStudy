using Dapper;
using Npgsql;
using System;
using System.Collections.Concurrent;
using System.Linq;

namespace InterworksCaseStudy.Helpers
{
    public static class AirportRepository
    {
        private const string airport_insert = @"INSERT INTO candidate2485.dim_airport (airport_name, city_id) VALUES (@airport_name, @city_id);";
        private const string airport_select = @"SELECT * FROM candidate2485.dim_airport WHERE airport_name = @airport_name";

        public static void Add(NpgsqlConnection conn, string airportName, string city, string state,  
            ConcurrentDictionary<string,Models.Dim_Airport> dictAirports, 
            ConcurrentDictionary<string, Models.Dim_City> dictCity, 
            ConcurrentDictionary<string, Models.Dim_State> dictState)
        {
            // Clean airport name
            string cleanAirport = Clean(airportName);

            if (cleanAirport != string.Empty && !dictAirports.ContainsKey(cleanAirport))
            {
                var tempState = state;
                if (tempState == string.Empty)
                {
                    var airportparts = airportName.Split(':');
                    if (airportparts.Any())
                        tempState = airportparts[0].Replace(city, "");
                }

                if (AirportRepository.Find(conn, cleanAirport, city, tempState, dictAirports, dictCity ) == null)
                {
                    var cityRec = CityRepository.Find(conn, city, tempState, dictCity, dictState);
                    if (cityRec != null)
                    {
                        // Write the airport to the database.
                        conn.Execute(airport_insert, new
                        {
                            airport_name = cleanAirport,
                            city_id = cityRec.city_id
                        });

                        // find to Add to hash table.
                        AirportRepository.Find(conn, cleanAirport, city, tempState, dictAirports, dictCity);
                    }
                    else
                    {
                        Console.WriteLine($"Could not add airport: {airportName}");
                    }
                }
            }
        }

        public static Models.Dim_Airport Find(NpgsqlConnection conn, string airport, string city, string state, ConcurrentDictionary<string,Models.Dim_Airport> dictAirport, ConcurrentDictionary<string,Models.Dim_City> dictCity)
        {
            var cachedCity = dictCity.FirstOrDefault(w => w.Key == (city + state));
            if (cachedCity.Value == null) return null;

            var cachedAirport = dictAirport.FirstOrDefault(w => w.Key == airport);
            if (cachedAirport.Value != null) return cachedAirport.Value ;

            var result = conn.Query<Models.Dim_Airport>(airport_select,
                new { airport_name = airport, city_id = cachedCity.Value.city_id });

            // add to hash table
            if (result.Any() && !dictAirport.ContainsKey(airport))
                dictAirport.TryAdd(airport,result.FirstOrDefault());

            return result.FirstOrDefault();
        }

        public static string Clean(string input)
        {
            string cleanAirport = string.Empty;
            var temp = input.Split(':');
            if (temp.Count() > 1)
                return temp[1].Trim();
            else
                return string.Empty;
        }
    }
}
