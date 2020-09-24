using System;

namespace OSPSuite.Presentation.Importer.Core
{
   public class Column
   {
      public string Name { get; set; }
      public Func<int, string> Units { get; set; }

      public string SelectedUnit { get; set; }

      public override string ToString()
      {
         return $"{Name} [{SelectedUnit}]";
      }
   }

   /// <summary>
   ///    e.g. a sheet in an excel file
   /// </summary>
   public interface IDataSheet
   {
      UnformattedData RawData { get; set; }
   }

   public class DataSheet : IDataSheet
   {
      public UnformattedData RawData { get; set; }
   }
}