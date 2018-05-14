using Dapper;
using Npgsql;
using Rhino.Etl.Core;
using System;
using System.Collections.Concurrent;
using System.Linq;

namespace InterworksCaseStudy.Helpers
{
    public static class FactRepository
    {
        private const string fact_insert = @"INSERT INTO candidate2485.
fact_flights
(
  transaction_id,
  flight_date,
  airline_id,
  tail_id,
  flight_num,
  origin_airport_id,
  destination_airport_id,
  crs_dep_time,
  dep_time,
  dep_delay,
  taxi_out,
  wheels_off,
  wheels_on,
  taxi_in,
  crs_arr_time,
  arr_time,
  arr_delay,
  crs_elapsed_time,
  actual_elapsed_time,
  cancelled,
  diverted,
  distance,
  distance_unit,
  distance_group,
  long_dep_delay,
  arr_next_day
)
VALUES
(
  @transaction_id_value,
  @flight_date_value,
  @airline_id_value,
  @tail_id_value,
  @flight_num_value,
  @origin_airport_id_value,
  @destination_airport_id_value,
  @crs_dep_time_value,
  @dep_time_value,
  @dep_delay_value,
  @taxi_out_value,
  @wheels_off_value,
  @wheels_on_value,
  @taxi_in_value,
  @crs_arr_time_value,
  @arr_time_value,
  @arr_delay_value,
  @crs_elapsed_time_value,
  @actual_elapsed_time_value,
  @cancelled_value,
  @diverted_value,
  @distance_value,
  @distance_unit_value,
  @distance_group_value,
  @long_dep_delay_value,
  @arr_next_day_value
)";
        //private const string fact_select  = @";

        public static void Add(NpgsqlConnection conn, Row row, 
            ConcurrentDictionary<string, Models.Dim_Airline> dictAirline,
            ConcurrentDictionary<string, Models.Dim_Airport> dictAirport,
            ConcurrentDictionary<string, Models.Dim_City> dictCity,
            ConcurrentDictionary<string, Models.Dim_State> dictState,
            ConcurrentDictionary<string, Models.Dim_Tail> dictAdictTail)
        {
                    //if (FactRepository.Find(conn, airline_code, cleanName, dictAirline) == null)
                    //{
                    //    // Write the airline to the database.
                    //    conn.Execute(fact_insert, new
                    //    {
                    //        transaction_id = row["TransactionId"],
                    //        flight_date = row["TransactionId"],
                    //        //airline_id = 
                    //        //tail_id =
                    //        flight_num = row["FlightNum"],
                    //        //origin_airport_id = 
                    //        //destination_airport_id = 
                    //        crs_dep_time = 


                    //        airline_code = airline_code,
                    //        name = cleanName
                    //    });

                    //    // find to Add to hash table.
                    //    FactRepository.Find(conn, airline_code, cleanName, dictAirline);
                    //}
            //}
        }

        //public static Models.Fact_Flights Find(NpgsqlConnection conn)
        //{
            //var cachedAirline = dictAirline.FirstOrDefault(w => w.Key == airline_code);
            //if (cachedAirline.Value != null) return cachedAirline.Value;

            //var result = conn.Query<Models.Dim_Airline>(airline_select,
            //    new { airline_code = airline_code });

            //// add to hash table
            //if (result.Any() && !dictAirline.ContainsKey(airline_code))
            //    dictAirline.TryAdd(airline_code, result.FirstOrDefault());

            //return result.FirstOrDefault();
        //}
    }
}
