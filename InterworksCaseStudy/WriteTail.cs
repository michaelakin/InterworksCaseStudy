using Rhino.Etl.Core;
using Rhino.Etl.Core.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Npgsql;
using Dapper;
using System.Data;
using InterworksCaseStudy.Helpers;
using System.Collections.Concurrent;

namespace InterworksCaseStudy
{
    public class WriteTail : AbstractOperation
    {
        private readonly string conString = ConfigurationManager.ConnectionStrings["InterworksCaseStudy"].ConnectionString;
        private ConcurrentDictionary<string, Models.Dim_Tail> _dictTail;

        private WriteTail() { }
        public WriteTail(ConcurrentDictionary<string, Models.Dim_Tail> dictTail)
        {
            _dictTail = dictTail;
        }

        public override IEnumerable<Row> Execute(IEnumerable<Row> rows)
        {
            using (var conn = new NpgsqlConnection(conString))
            {
                conn.Open();
                foreach (var row in rows)
                {
                    TailRepository.Add(conn, (string)row["TAILNUM"],  _dictTail);
                    
                    yield return row;
                }
            }
        }


    }
}
