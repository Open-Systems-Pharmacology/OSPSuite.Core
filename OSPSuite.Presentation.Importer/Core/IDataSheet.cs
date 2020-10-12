using System;

namespace OSPSuite.Presentation.Importer.Core
{
   public class UnitDescription
   {
      public UnitDescription(Func<int, string> units, string columnName = "")
      {
         Units = units;
         ColumnName = columnName;
         SelectedUnit = units(-1);
      }

      public UnitDescription(string selectedUnit)
      {
         Units = _ => selectedUnit;
         ColumnName = null;
         SelectedUnit = selectedUnit;
      }

      public Func<int, string> Units { get; }

      public string SelectedUnit { get; }

      public string ColumnName { get; }

      public override string ToString()
      {
         return SelectedUnit;
      }
   }

   public class Column
   {
      public string Name { get; set; }

      public UnitDescription Unit { get; set; }

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