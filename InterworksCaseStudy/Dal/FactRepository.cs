using Dapper;
using Npgsql;
using Rhino.Etl.Core;
using System;
using System.Collections.Concurrent;
using System.Linq;

namespace InterworksCaseStudy.Dal
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
  @transaction_id,
  @flight_date,
  @airline_id,
  @tail_id,
  @flight_num,
  @origin_airport_id,
  @destination_airport_id,
  @crs_dep_time,
  @dep_time,
  @dep_delay,
  @taxi_out,
  @wheels_off,
  @wheels_on,
  @taxi_in,
  @crs_arr_time,
  @arr_time,
  @arr_delay,
  @crs_elapsed_time,
  @actual_elapsed_time,
  @cancelled,
  @diverted,
  @distance,
  @distance_unit,
  @distance_group,
  @long_dep_delay,
  @arr_next_day
)";
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
            string tempDistanceGroup = string.Empty;
            var tempDistArray = tempDistance.Split(' ');
            string unit = string.Empty;
            if (tempDistArray.Length == 2)
            {
                tempDistanceInt = Convert.ToInt32(tempDistArray[0]);
                tempDistanceUnits = tempDistArray[1].ToString().Trim();
                tempDistanceGroup = GetDistanceGroup(tempDistanceInt);
            }
            else
            {
                throw new Exception("Invalid disance");
            }

            // Find codes
            var airline = dictAirline.Where(w => w.Key == (string)row["AirlineCode"]).FirstOrDefault().Value; ;
            var tail = dictTail.Where(w => w.Key == TailRepository.CleanTail((string)row["TailNum"])).FirstOrDefault().Value;
            var origin_airport = dictAirport.Where(w => w.Key == AirportRepository.Clean((string)row["OriginAirportName"])).FirstOrDefault().Value;
            var dest_airport = dictAirport.Where(w => w.Key == AirportRepository.Clean((string)row["DestAirportName"])).FirstOrDefault().Value;

            // Write the airline to the database.
            conn.Execute(fact_insert, new
            {
                transaction_id = row["TransactionId"],
                flight_date = row["FlightDate"],
                airline_id = airline.airline_id,
                tail_id = tail.tail_id,
                flight_num = row["FlightNum"],
                origin_airport_id = origin_airport.airport_id,
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
                distance = tempDistanceInt,
                distance_unit = tempDistanceUnits,
                distance_group = GetDistanceGroup(tempDistanceInt),
                long_dep_delay = ((int)row["DepDelay"] > 15),
                arr_next_day = (DateTime.Compare((DateTime)row["ArrTime"], (DateTime)row["DepTime"]) < 0)
            });
        }

        public static Models.Fact_Flights Map(Row row,
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
            string tempDistanceGroup = string.Empty;
            var tempDistArray = tempDistance.Split(' ');
            string unit = string.Empty;
            if (tempDistArray.Length == 2)
            {
                tempDistanceInt = Convert.ToInt32(tempDistArray[0]);
                tempDistanceUnits = tempDistArray[1].ToString().Trim();
                tempDistanceGroup = GetDistanceGroup(tempDistanceInt);
            }
            else
            {
                throw new Exception("Invalid disance");
            }

            // Find codes
            var airline = dictAirline.Where(w => w.Key == (string)row["AirlineCode"]).FirstOrDefault().Value; ;
            var tail = dictTail.Where(w => w.Key == TailRepository.CleanTail((string)row["TailNum"])).FirstOrDefault().Value;
            var origin_airport = dictAirport.Where(w => w.Key == AirportRepository.Clean((string)row["OriginAirportName"])).FirstOrDefault().Value;
            var dest_airport = dictAirport.Where(w => w.Key == AirportRepository.Clean((string)row["DestAirportName"])).FirstOrDefault().Value;

            // Write the airline to the database.
            return new Models.Fact_Flights()
            {
                transaction_id = (int)row["TransactionId"],
                flight_date = (DateTime)row["FlightDate"],
                airline_id = airline.airline_id,
                tail_id = tail.tail_id,
                flight_num = (int)row["FlightNum"],
                origin_airport_id = origin_airport.airport_id,
                dest_airport_id = dest_airport.airport_id,
                crs_dep_time = (DateTime) row["CrsDepTime"],
                dep_time = (DateTime)row["DepTime"],
                dep_delay = (int)row["DepDelay"],
                taxi_out = (int)row["TaxiOut"],
                wheels_off = (DateTime)row["WheelsOff"],
                wheels_on = (DateTime)row["WheelsOn"],
                taxi_in = (int)row["TaxiIn"],
                crs_arr_time = (DateTime)row["CrsArrTime"],
                arr_time = (DateTime)row["ArrTime"],
                arr_delay = (int)row["ArrDelay"],
                crs_elapsed_time = (int)row["CrsElapsedTime"],
                actual_elapsed_time = (int)row["ActualElapsedTime"],
                cancelled = (bool)row["Cancelled"],
                diverted = (bool)row["Diverted"],
                distance = tempDistanceInt,
                distance_unit = tempDistanceUnits,
                distance_group = GetDistanceGroup(tempDistanceInt),
                long_dep_delay = ((int)row["DepDelay"] > 15),
                arr_next_day = (DateTime.Compare((DateTime)row["ArrTime"], (DateTime)row["DepTime"]) < 0)
            };
        }

        public static string GetDistanceGroup(int distance)
        {
            decimal ceiling = Math.Ceiling(distance / 100M) * 100;
            decimal floor = ceiling - 100;
            return $"{floor}-{ceiling}";
        }
    }
}
