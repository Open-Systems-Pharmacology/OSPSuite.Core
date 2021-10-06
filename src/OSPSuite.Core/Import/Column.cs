using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Import
{
   public class UnitDescription
   {
      public static readonly string InvalidUnit = "?";

      public string SelectedUnit { get; private set; }

      public string ColumnName { get; }

      public UnitDescription()
      {
         ColumnName = "";
         SelectedUnit = InvalidUnit;
      }
      public UnitDescription(string defUnit, string columnName = "")
      {
         SelectedUnit = defUnit;
         ColumnName = columnName;
      }

      public string ExtractUnit(Func<string,int> index, IEnumerable<string> row)
      {
         if (string.IsNullOrEmpty(ColumnName))
         {
            return SelectedUnit;
         }

         return row.ElementAt(index(ColumnName));
      }

      public UnitDescription(string selectedUnit)
      {
         ColumnName = null;
         SelectedUnit = selectedUnit;
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
      public IDimension Dimension { get; set; }

      public string LloqColumn { get; set; }

      public string ErrorStdDev { get; set; }

      public override string ToString()
      {
         return $"{Name} [{Unit}]";
      }
   }
}
