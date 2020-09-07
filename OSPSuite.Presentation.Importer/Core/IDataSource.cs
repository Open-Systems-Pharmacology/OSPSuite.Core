using System.Collections.Generic;

namespace OSPSuite.Presentation.Importer.Core
{
   /// <summary>
   /// Collection of DataSets
   /// </summary>
   public interface IDataSource
   {
      IReadOnlyDictionary<string, IDataSet> DataSets { get; set; }
   }

   public class DataSource : IDataSource
   {
      public IReadOnlyDictionary<string, IDataSet> DataSets { get; set; }
   }
}
