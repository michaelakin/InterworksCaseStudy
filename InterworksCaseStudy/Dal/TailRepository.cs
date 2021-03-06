﻿using Dapper;
using Npgsql;
using System;
using System.Collections.Concurrent;
using System.Linq;

namespace InterworksCaseStudy.Dal
{
    public static class TailRepository
    {
        private const string tail_insert = @"INSERT INTO candidate2485.dim_tail (tail_no) VALUES (@tail_no);";
        private const string tail_select = @"SELECT * FROM candidate2485.dim_tail WHERE tail_no = @tail_no";

        public static void Add(NpgsqlConnection conn, string tail_no, ConcurrentDictionary<string, Models.Dim_Tail> dictTail)
        {
            if (TailRepository.Find(conn, tail_no, dictTail) == null)
            {
                // clean the tail
                var cleanTailNo = CleanTail(tail_no);

                // Write the airline to the database.
                conn.Execute(tail_insert, new
                {
                    tail_no = cleanTailNo
                });

                // find to Add to hash table.
                Find(conn, cleanTailNo, dictTail);
            }
        }

        public static Models.Dim_Tail Find(NpgsqlConnection conn, string tail_no, ConcurrentDictionary<string, Models.Dim_Tail> dictTail)
        {
            var cachedTailNo = dictTail.FirstOrDefault(w => w.Key == tail_no);
            if (cachedTailNo.Value != null) return cachedTailNo.Value;

            var result = conn.Query<Models.Dim_Tail>(tail_select,
                new { tail_no = tail_no });

            // add to hash table
            if (result.Any() && !dictTail.ContainsKey(tail_no))
                dictTail.TryAdd(tail_no, result.FirstOrDefault());

            return result.FirstOrDefault();
        }

        public static string CleanTail(string input)
        {
            return input.Replace("@", "").Replace("-", "");
        }
    }
}
