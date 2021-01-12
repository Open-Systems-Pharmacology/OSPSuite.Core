using System;
using System.Collections.Generic;
using System.Linq;

namespace OSPSuite.Infrastructure.Import.Core
{
   public class UnitDescription
   {
      public static readonly string InvalidUnit = "?";

      public UnitDescription()
      {
         Units = _ => InvalidUnit;
         ColumnName = "";
         SelectedUnit = InvalidUnit;
      }
      public UnitDescription(IEnumerable<string> units, string columnName = "")
      {
         AttachUnitFunction(units);
         ColumnName = columnName;
      }

      public UnitDescription(string selectedUnit)
      {
         Units = _ => selectedUnit;
         ColumnName = null;
         SelectedUnit = selectedUnit;
      }

      private IList<string> _units = null;

      public Func<int, string> Units { get; private set; }

      public string SelectedUnit { get; private set; }

      public string ColumnName { get; }

      public void AttachUnitFunction(IEnumerable<string> column)
      {
         if (column == null) return;
         _units = column.ToList();
         if (_units.Count == 0) return; 

         var def = _units.First(c => !string.IsNullOrWhiteSpace(c));
         if (_units.Count == 1)
            Units = _ => def;
         else
            Units = i => (i > 0 && i < _units.Count) ? _units[i] : def;
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