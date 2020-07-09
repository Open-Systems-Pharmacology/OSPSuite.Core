using System.Collections.Generic;

namespace OSPSuite.Presentation.Importer.Core
{
   public interface IColumn
   {
      string Name { get; set; }
      string Unit { get; set; }
   }

   public class Column : IColumn
   {
      public string Name { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
      public string Unit { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
   }

   /// <summary>
   /// e.g. a sheet in an excel file
   /// </summary>
   public interface IDataTable
   {
      Dictionary<string, IList<string>> RawData { get; set; }
      IFormat Format { get; set; }
   }

   public class DataTable : IDataTable
   {
      public Dictionary<string, IList<string>> RawData 
      { get; set; }
      
      public IFormat Format { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
   }
}
