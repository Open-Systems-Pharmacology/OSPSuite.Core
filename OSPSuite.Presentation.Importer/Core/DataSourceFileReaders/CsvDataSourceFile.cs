using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSPSuite.Presentation.Importer.Core.DataSourceFileReaders
{
   public class CsvDataSourceFile : DataSourceFile
   {
      public CsvDataSourceFile(string path) : base(path) { }

      override protected Dictionary<string, IDataTable> LoadFromFile(string path)
      {
         throw new System.NotImplementedException();
      }
   }
}
