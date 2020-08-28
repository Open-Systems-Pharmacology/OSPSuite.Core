using System.Collections.Generic;

namespace OSPSuite.Presentation.Importer.Core
{
   public class Column
   {
      public enum ColumnNames
      {
         Time,
         Concentration,
         Error
      }

      public ColumnNames Name { get; set; }
      public string Unit { get; set; }
      public IEnumerable<string> AvailableUnits { get; set; }

      public override string ToString()
      {
         return $"{Name} [{Unit}]";
      }

      public override bool Equals(object obj)
      {
         var other = obj as Column;
         return other.Name == Name && other.Unit == Unit; //TODO Resharper
      }

      public override int GetHashCode()
      {
         return base.GetHashCode(); //TODO Resharper
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