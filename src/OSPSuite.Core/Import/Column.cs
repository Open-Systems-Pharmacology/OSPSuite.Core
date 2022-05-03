using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain;
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

      //unit is missing if the SelectedUnit is undefined and 1. the error is not geometric or 2. the mapped column is empty
      public bool MissingUnitMapping()
      {
         return (Unit.SelectedUnit == UnitDescription.InvalidUnit &&
                  (ErrorStdDev == null || ErrorStdDev.Equals(Constants.STD_DEV_ARITHMETIC)))
                  ||
                  ((Unit.SelectedUnit == null || Unit.SelectedUnit == UnitDescription.InvalidUnit) &&
                  Unit.ColumnName == string.Empty);
      }
   }
}
