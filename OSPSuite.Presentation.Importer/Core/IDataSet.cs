using System.Collections.Generic;

namespace OSPSuite.Presentation.Importer.Core
{
   //When parsing the DataSets we  group by metadata and we need to remember the values,
   //so we can use them for the naming conventions. This way an instantiated metadata represents
   //the value of a metadata (Id) on each group
   public class InstantiatedMetaData
   {
      public int Id { get; set; }
      public string Value { get; set; }
   }

   /// <summary>
   /// Data from a single experiment
   /// </summary>
   public interface IDataSet
   {
      IEnumerable<ParsedDataSet> Data { get; set; }
   }

   public class DataSet : IDataSet
   {
      public IEnumerable<ParsedDataSet> Data { get; set; } = new List<ParsedDataSet>();
   }

}