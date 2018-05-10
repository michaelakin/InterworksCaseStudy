using FileHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterworksCaseStudy.Models
{
    [DelimitedRecord("|")]
    [IgnoreFirst()]
    public class FlatFile
    {
        public string TransactionId { get; set; }
        [FieldConverter(ConverterKind.Date, "yyyyMMdd")]
        public DateTime FlightDate;
        public string AirlineCode { get; set; }
        public string AirlineName { get; set; }
        public string TailNum { get; set; }
        public string FlightNum { get; set; }
        public string OriginAirportCode { get; set; }
        public string OriginAirportName { get; set; }
        public string OriginCityName { get; set; }
        public string OriginState { get; set; }
        public string OriginStateName { get; set; }
        public string DestAirportCode { get; set; }
        public string DestAirportName { get; set; }
        public string DestCityName { get; set; }
        public string DestState { get; set; }
        public string DestStateName { get; set; }
        public string CrsDepTime { get; set; }
        public string DepTime { get; set; }
        public string DepDelay { get; set; }
        public string TaxiOut { get; set; }
        public string WheelsOff { get; set; }
        public string WheelsOn { get; set; }
        public string TaxiIn { get; set; }
        public string CrsArrTime { get; set; }
        public string ArrTime { get; set; }
        public string ArrDelay { get; set; }
        public string CrsElapsedTime { get; set; }
        public string ActualElapsedTime { get; set; }
        [FieldConverter(typeof(BoolConverter))]
        public bool Cancelled;
        [FieldConverter(typeof(BoolConverter))]
        public bool Diverted;
        public string Distance { get; set; }


    }
}

