using System;

namespace InterworksCaseStudy.Models
{
    public class Fact_Flights
    {
        public int transaction_id { get; set; }
        public DateTime flight_date { get; set; }
        public int airline_id { get; set; }
        public int tail_id { get; set; }
        public int flight_num { get; set; }
        public int origin_airport_id { get; set; }
        public int dest_airport_id { get; set; }
        public DateTime crs_dep_time { get; set; }
        public DateTime dep_time { get; set; }
        public int dep_delay { get; set; }
        public int taxi_out { get; set; }
        public DateTime wheels_off { get; set; }
        public DateTime wheels_on { get; set; }
        public int taxi_in { get; set; }
        public DateTime crs_arr_time { get; set; }
        public DateTime arr_time { get; set; }
        public int arr_delay { get; set; }
        public int crs_elapsed_time { get; set; }
        public int actual_elapsed_time { get; set; }
        public bool cancelled { get; set; }
        public bool diverted { get; set; }
        public int  distance { get; set; }
        public string distance_unit { get; set; }
        public string distance_group { get; set; }
        public bool long_dep_delay { get; set; }
        public bool arr_next_day { get; set; }
    }
}
