using FileHelpers;
using System;

namespace InterworksCaseStudy.Models
{
    [DelimitedRecord("|")]
    [IgnoreFirst()]
    public class FlatFile
    {
        [FieldConverter(ConverterKind.Int32)]
        public int TransactionId;
        [FieldConverter(ConverterKind.Date, "yyyyMMdd")]
        public DateTime FlightDate;
        public string AirlineCode;
        public string AirlineName;
        public string TailNum;
        [FieldConverter(ConverterKind.Int32)]
        public int FlightNum;
        public string OriginAirportCode;
        public string OriginAirportName;
        public string OriginCityName;
        public string OriginState;
        public string OriginStateName;
        public string DestAirportCode;
        public string DestAirportName;
        public string DestCityName;
        public string DestState;
        public string DestStateName;
        [FieldConverter(typeof(Converters.TimeConverter))]
        public DateTime CrsDepTime;
        [FieldConverter(typeof(Converters.TimeConverter))]
        [FieldNullValue(typeof(DateTime), "0001-01-01")]
        public DateTime DepTime;
        [FieldConverter(ConverterKind.Int32)]
        [FieldNullValue(typeof(Int32), "0")]
        public int DepDelay;
        [FieldConverter(ConverterKind.Int32)]
        [FieldNullValue(typeof(Int32), "0")]
        public int TaxiOut;
        [FieldConverter(typeof(Converters.TimeConverter))]
        [FieldNullValue(typeof(DateTime), "0001-01-01")]
        public DateTime WheelsOff;
        [FieldConverter(typeof(Converters.TimeConverter))]
        [FieldNullValue(typeof(DateTime), "0001-01-01")]
        public DateTime WheelsOn;
        [FieldConverter(ConverterKind.Int32)]
        [FieldNullValue(typeof(Int32), "0")]
        public int TaxiIn;
        [FieldConverter(typeof(Converters.TimeConverter))]
        public DateTime CrsArrTime;
        [FieldConverter(typeof(Converters.TimeConverter))]
        [FieldNullValue(typeof(DateTime), "0001-01-01")]
        public DateTime ArrTime;
        [FieldConverter(ConverterKind.Int32)]
        [FieldNullValue(typeof(Int32),"0")]
        public int ArrDelay;
        [FieldConverter(ConverterKind.Int32)]
        [FieldNullValue(typeof(Int32), "0")]
        public int CrsElapsedTime;
        [FieldConverter(ConverterKind.Int32)]
        [FieldNullValue(typeof(Int32), "0")]
        public int ActualElapsedTime;
        [FieldConverter(typeof(Converters.BoolConverter))]
        public bool Cancelled;
        [FieldConverter(typeof(Converters.BoolConverter))]
        public bool Diverted;
        public string Distance;
    }
}

