using Dapper;
using Npgsql;
using System;
using System.Collections.Concurrent;
using System.Linq;

namespace InterworksCaseStudy.Helpers
{
    public static class AirlineRepository
    {
        private const string airline_insert = @"INSERT INTO candidate2485.dim_airline (airline_code, name) VALUES (@airline_code, @name);";
        private const string airline_select = @"SELECT * FROM candidate2485.dim_airline WHERE airline_code = @airline_code";

        public static void Add(NpgsqlConnection conn, string airline_code, string name, ConcurrentDictionary<string, Models.Dim_Airline> dictAirline)
        {
            // Clean airline name
            string cleanName = string.Empty;
            var temp = name.Split(':');
            if (temp.Count() > 1)
            {
                cleanName = temp[0].Trim();

                if (cleanName != string.Empty && !dictAirline.ContainsKey(cleanName))
                {

                    if (AirlineRepository.Find(conn, airline_code, cleanName, dictAirline) == null)
                    {
                        // Write the airline to the database.
                        conn.Execute(airline_insert, new
                        {
                            airline_code = airline_code,
                            name = cleanName
                        });

                        // find to Add to hash table.
                        AirlineRepository.Find(conn, airline_code, cleanName, dictAirline);
                    }
                }
                else
                {
                    Console.WriteLine($"Could not add airline: {airline_code}");
                }
            }
        }

        public static Models.Dim_Airline Find(NpgsqlConnection conn, string airline_code, string name, ConcurrentDictionary<string, Models.Dim_Airline> dictAirline)
        {
            var cachedAirline = dictAirline.FirstOrDefault(w => w.Key == airline_code);
            if (cachedAirline.Value != null) return cachedAirline.Value;

            var result = conn.Query<Models.Dim_Airline>(airline_select,
                new { airline_code = airline_code });

            // add to hash table
            if (result.Any() && !dictAirline.ContainsKey(airline_code))
                dictAirline.TryAdd(airline_code, result.FirstOrDefault());

            return result.FirstOrDefault();
        }
    }
}
