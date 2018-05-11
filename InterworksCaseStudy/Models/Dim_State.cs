using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterworksCaseStudy.Models
{
    public class Dim_State
    {
 
        public int state_id { get; set; }
        public string abbrev { get; set; }
        public string name { get; set; }
    }


}
