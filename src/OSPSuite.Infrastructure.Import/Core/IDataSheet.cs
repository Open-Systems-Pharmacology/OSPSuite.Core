using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Reflection;

namespace OSPSuite.Infrastructure.Import.Core
{
   public class UnitDescription
   {
      public static readonly string InvalidUnit = "?";

      public UnitDescription()
      {
         Units = _ => InvalidUnit;
         ColumnName = null;
         SelectedUnit = InvalidUnit;
      }
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

      public Func<int, string> Units { get; private set; }

      public string SelectedUnit { get; private set; }

      public string ColumnName { get; }

      public void AttachUnitFunction(IEnumerable<string> column)
      {
         var units = column.ToList();
         var def = units.First(c => !string.IsNullOrWhiteSpace(c));
         Units = i => (i > 0) ? units[i] : def;
         SelectedUnit = def;
      }

      public override string ToString()
      {
         return SelectedUnit;
      }
   }

   public class Column
   {
      public string Name { get; set; }

      public UnitDescription Unit { get; set; }

      public string LloqColumn { get; set; }

      public string ErrorStdDev { get; set; } 

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