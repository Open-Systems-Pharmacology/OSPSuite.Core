using System.Collections.Generic;

namespace OSPSuite.Presentation.Importer.Core
{
   /// <summary>
   /// Collection of DataSets
   /// </summary>
   public interface IDataSource
   {
      IList<IDataSet> DataSets { get; set; }
   }

   public class DataSource : IDataSource
   {
      public IList<IDataSet> DataSets { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
   }
}
