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
      public string Name 
      {
         get;
         set; 
      }

      public string Unit 
      { 
         get;
         set; 
      }
   }

   /// <summary>
   /// e.g. a sheet in an excel file
   /// </summary>
   public interface IDataSheet
   {
      Dictionary<string, IList<string>> RawData { get; set; }
      IDataFormat Format { get; set; }
   }

   public class DataSheet : IDataSheet
   {
      public Dictionary<string, IList<string>> RawData 
      { get; set; }
      
      public IDataFormat Format { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
   }
}
