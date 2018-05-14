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
            ConcurrentDictionary<string, Models.Dim_Tail> dictTail)
        {
            // Convert Distance.
            string tempDistance = (string)row["Distance"];
            int tempDistanceInt = 0;
            string tempDistanceUnits = string.Empty;
            string tempDistanceGrup = string.Empty;
            var tempDistArray = tempDistance.Split(' ');
            string unit = string.Empty;
            if (tempDistArray.Length == 2)
            {
                tempDistanceInt = Convert.ToInt32(tempDistance[0]);
                tempDistanceUnits = tempDistance[1].ToString().Trim();
                tempDistanceGrup = GetDistanceGroup(tempDistanceInt);
            }
            else
            {
                throw new Exception("Invalid disance");
            }

            // Find codes
            var airline = dictAirline.Where(w => w.Key == (string)row["AirlineCode"]).FirstOrDefault().Value; ;
            var tail = dictTail.Where(w => w.Key == TailRepository.CleanTail((string)row["TailNum"])).FirstOrDefault().Value ;
            var origin_airport = dictAirport.Where(w => w.Key == AirportRepository.Clean((string)row["OriginAirportName"])).FirstOrDefault().Value;
            var dest_airport = dictAirport.Where(w => w.Key   == AirportRepository.Clean((string)row["DestAirportName"])).FirstOrDefault().Value;

            // Write the airline to the database.
            conn.Execute(fact_insert, new
            {
                transaction_id = row["TransactionId"],
                flight_date = row["TransactionId"],
                airline_id = airline.airline_id,
                tail_id = tail.tail_id,
                flight_num = row["FlightNum"],
                origin_airport_id =  origin_airport.airport_id,
                destination_airport_id = dest_airport.airport_id,
                crs_dep_time = row["CrsDepTime"],
                dep_time = row["DepTime"],
                dep_delay = row["DepDelay"],
                taxi_out = row["TaxiOut"],
                wheels_off = row["WheelsOff"],
                wheels_on = row["WheelsOn"],
                taxi_in = row["TaxiIn"],
                crs_arr_time = row["CrsArrTime"],
                arr_time = row["ArrTime"],
                arr_delay = row["ArrDelay"],
                crs_elapsed_time = row["CrsElapsedTime"],
                actual_elapsed_time = row["ActualElapsedTime"],
                cancelled = row["Cancelled"],
                diverted = row["Diverted"],
                distance = tempDistance,
                distance_unit = tempDistanceUnits,
                distance_group = GetDistanceGroup(tempDistanceInt),
                long_dep_delay =  ((int)row["DepDelay"] > 15),
                arr_next_day = (DateTime.Compare((DateTime)row["ArrTime"],(DateTime)row["DepTime"]) < 0)
            });

        }

        public static string GetDistanceGroup(int distance)
        {
            decimal ceiling = Math.Ceiling(distance / 100M) * 100;
            decimal floor = ceiling - 100;
            return $"{floor}-{ceiling}";
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
