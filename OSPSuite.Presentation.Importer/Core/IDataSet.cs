using System.Collections.Generic;

namespace OSPSuite.Presentation.Importer.Core
{
   public class InstanstiatedMetaData
   {
      public int Id { get; set; }
      public string Value { get; set; }
   }

   /// <summary>
   /// Data from a single experiment
   /// </summary>
   public interface IDataSet
   {
      IDictionary<IEnumerable<InstanstiatedMetaData> ,Dictionary<Column, IList<ValueAndLloq>>> Data { get; set; }
   }

   public class DataSet : IDataSet
   {
      public IDictionary<IEnumerable<InstanstiatedMetaData>, Dictionary<Column, IList<ValueAndLloq>>> Data { get; set; } = new Dictionary<IEnumerable<InstanstiatedMetaData>, Dictionary<Column, IList<ValueAndLloq>>>();
   }

}