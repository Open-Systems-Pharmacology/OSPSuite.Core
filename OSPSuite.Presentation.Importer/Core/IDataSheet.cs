using System.Collections.Generic;

namespace OSPSuite.Presentation.Importer.Core
{
   public class Column
   {
      public string Name { get; set; }
      public string Unit { get; set; }
      public IEnumerable<string> AvailableUnits { get; set; }

      public override string ToString()
      {
         return $"{Name} [{Unit}]";
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