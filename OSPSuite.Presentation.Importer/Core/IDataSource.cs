using System.Collections.Generic;

namespace OSPSuite.Presentation.Importer.Core
{
   /// <summary>
   /// Collection of DataSets
   /// </summary>
   public interface IDataSource
   {
      IDictionary<string, IDataSet> DataSets { get; set; }
   }

   public class DataSource : IDataSource
   {
      public IDictionary<string, IDataSet> DataSets { get; set; } = new Dictionary<string, IDataSet>();
   }
}
