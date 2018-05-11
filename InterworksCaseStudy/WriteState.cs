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

namespace InterworksCaseStudy
{
    public class WriteState : AbstractOperation
    {
        private readonly string conString = ConfigurationManager.ConnectionStrings["InterworksCaseStudy"].ConnectionString;
        private HashSet<string> table = new HashSet<string>();

        public override IEnumerable<Row> Execute(IEnumerable<Row> rows)
        {
            using (var conn = new NpgsqlConnection(conString))
            {
                conn.Open();
                foreach (var row in rows)
                {
                    StateRepository.AddState(conn, (string)row["OriginState"], (string)row["OriginStateName"], table);
                    StateRepository.AddState(conn, (string)row["DestState"], (string)row["DestStateName"], table);

                    yield return row;
                }
            }
        }


    }
}
