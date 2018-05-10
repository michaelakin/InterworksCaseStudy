using Rhino.Etl.Core.Operations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InterworksCaseStudy.Models;
using Rhino.Etl.Core.Files;
using Rhino.Etl.Core;

namespace InterworksCaseStudy
{
    public class FileGetData : AbstractOperation
    {
        public string FilePath { get; set; }
        public FileGetData(string inPutFilepath)
        {
            FilePath = inPutFilepath;
        }

        public override IEnumerable<Row> Execute(IEnumerable<Row> rows)
        {
            using (FileEngine file = FluentFile.For<FlatFile>().From(FilePath))
            {
                foreach (FlatFile fileRow in file)
                {
                    yield return Row.FromObject(fileRow);
                }
            }
        }
    }
}
