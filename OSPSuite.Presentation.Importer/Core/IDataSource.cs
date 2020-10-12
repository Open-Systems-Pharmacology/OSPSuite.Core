using OSPSuite.Utility.Collections;

namespace OSPSuite.Presentation.Importer.Core
{
   /// <summary>
   /// Collection of DataSets
   /// </summary>
   public interface IDataSource
   {
      Cache<string, IDataSet> DataSets { get; }
   }

   public class DataSource : IDataSource
   {
      public Cache<string, IDataSet> DataSets { get; } = new Cache<string, IDataSet>();
   }
}
