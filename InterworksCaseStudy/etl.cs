using Rhino.Etl.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterworksCaseStudy
{
    public class Etl : EtlProcess
    {
        ConnectionStringSettings connection_string;
        string _flatFileLocation;
        private Dictionary<string, Models.Dim_City> cityDict = new Dictionary<string, Models.Dim_City>();
        private Dictionary<string, Models.Dim_State> stateDict = new Dictionary<string, Models.Dim_State>();

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
            Register(new WriteCity(cityDict,stateDict));
        }
    }
}
