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
        public Etl(ConnectionStringSettings connectionString, string flatFileLocaiton)
        {
            connection_string = connectionString;
            _flatFileLocation = flatFileLocaiton;
        }

        protected override void Initialize()
        {
            Register(new FileGetData(_flatFileLocation));
            Register(new WriteDataToConsole());
            //Register(new WriteDataToConsole(connection_string));
        }
    }
}
