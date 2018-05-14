using Rhino.Etl.Core;
using System.Collections.Concurrent;
using System.Configuration;

namespace InterworksCaseStudy
{
    public class Etl : EtlProcess
    {
        ConnectionStringSettings connection_string;
        string _flatFileLocation;
        private ConcurrentDictionary<string, Models.Dim_City> dictCity = new ConcurrentDictionary<string, Models.Dim_City>();
        private ConcurrentDictionary<string, Models.Dim_State> dictState = new ConcurrentDictionary<string, Models.Dim_State>();
        private ConcurrentDictionary<string, Models.Dim_Airport> dictAirport = new ConcurrentDictionary<string, Models.Dim_Airport>();
        private ConcurrentDictionary<string, Models.Dim_Airline> dictAirline = new ConcurrentDictionary<string, Models.Dim_Airline>();
        private ConcurrentDictionary<string, Models.Dim_Tail> dictTail = new ConcurrentDictionary<string, Models.Dim_Tail>();

        public Etl(ConnectionStringSettings connectionString, string flatFileLocaiton)
        {
            connection_string = connectionString;
            _flatFileLocation = flatFileLocaiton;
        }

        protected override void Initialize()
        {
            Register(new FileGetData(_flatFileLocation));
            //Register(new WriteDataToConsole());
            Register(new WriteState(dictState));
            Register(new WriteCity(dictCity, dictState));
            Register(new WriteAirport(dictAirport, dictCity, dictState));
            Register(new WriteAirline(dictAirline));
            Register(new WriteTail(dictTail));
            Register(new WriteFact(dictAirline, dictAirport, dictCity, dictState, dictTail));
        }
    }
}
