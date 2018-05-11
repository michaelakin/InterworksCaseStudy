using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterworksCaseStudy.Helpers
{
    public static class StateRepository
    {
        private const string state_insert = @"INSERT INTO candidate2485.dim_state (abbrev, name) VALUES (@abbr, @name);";
        private const string state_select = @"SELECT * FROM candidate2485.dim_state WHERE abbrev = @abbr";

        public static void AddState(NpgsqlConnection conn, string state_abbr, string state_name, HashSet<string> table)
        {
            if (state_abbr != string.Empty && !table.Contains(state_abbr) && !FindState(conn, state_abbr, table))
            {
                // Write the state to the database.
                conn.Execute(state_insert, new
                {
                    abbr = state_abbr,
                    name = state_name
                });

                // Add to hash table.
                table.Add(state_abbr);
            }
        }

        public static bool FindState(NpgsqlConnection conn, string abbrev, HashSet<string> table)
        {
            var result = conn.Query<Models.Dim_State>(state_select, new { abbr = abbrev });

            // add to hash table
            if (result.Any())
                table.Add(result.First().abbrev);

            return result.Any();
        }
    }
}
