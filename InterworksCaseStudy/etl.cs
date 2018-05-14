using Rhino.Etl.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace InterworksCaseStudy
{
    public class Etl : EtlProcess
    {
        ConnectionStringSettings connection_string;
        string _flatFileLocation;
        private ConcurrentDictionary<string, Models.Dim_City> cityDict = new ConcurrentDictionary<string, Models.Dim_City>();
        private ConcurrentDictionary<string, Models.Dim_State> stateDict = new ConcurrentDictionary<string, Models.Dim_State>();
        private ConcurrentDictionary<string, Models.Dim_Airport> airportDict = new ConcurrentDictionary<string, Models.Dim_Airport>();
        private ConcurrentDictionary<string, Models.Dim_Airline> airlineDict = new ConcurrentDictionary<string, Models.Dim_Airline>();
        private ConcurrentDictionary<string, Models.Dim_Tail> tailDict = new ConcurrentDictionary<string, Models.Dim_Tail>();

        public Etl(ConnectionStringSettings connectionString, string flatFileLocaiton)
        {
            connection_string = connectionString;
            _flatFileLocation = flatFileLocaiton;
        }

        protected override void Initialize()
        {
            Register(new FileGetData(_flatFileLocation));
            //Register(new WriteDataToConsole());
            Register(new WriteState(stateDict));
            Register(new WriteCity(cityDict, stateDict));
            Register(new WriteAirport(airportDict, cityDict, stateDict));
            Register(new WriteAirline(airlineDict));
            Register(new WriteTail(tailDict));
        }
    }
}
