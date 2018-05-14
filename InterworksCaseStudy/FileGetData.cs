using InterworksCaseStudy.Models;
using Rhino.Etl.Core;
using Rhino.Etl.Core.Files;
using Rhino.Etl.Core.Operations;
using System.Collections.Generic;

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
                int i = 0;
                foreach (FlatFile fileRow in file)
                {
                    yield return Row.FromObject(fileRow);
                    i++;
                }
            }
        }
    }
}
