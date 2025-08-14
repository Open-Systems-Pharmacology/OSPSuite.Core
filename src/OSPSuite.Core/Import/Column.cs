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

      // This setter is needed by R
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

      public string ExtractUnit(Func<string, int> index, IEnumerable<string> row)
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
      public Column()
      {
      }

      public Column(Column baseColumn)
      {
         Name = baseColumn.Name;
         Unit = baseColumn.Unit;
         Dimension = baseColumn.Dimension;
         LloqColumn = baseColumn.LloqColumn;
         ErrorStdDev = baseColumn.ErrorStdDev;
      }

      public string Name { get; set; }

      public UnitDescription Unit { get; set; }
      public IDimension Dimension { get; set; }

      public string LloqColumn { get; set; }

      public string ErrorStdDev { get; set; }

      public override string ToString()
      {
         return $"{Name} [{Unit}]";
      }

      public bool MissingUnitMapping()
      {
         return
            //manual selection: selected unit not set and the column is not a Geometric Standard Deviation
            (Unit.SelectedUnit == UnitDescription.InvalidUnit &&
             (ErrorStdDev == null || ErrorStdDev.Equals(Constants.STD_DEV_ARITHMETIC)))
            ||
            //select from a column: selected unit is null (not set) and no column has been selected
            (Unit.SelectedUnit == null &&
             Unit.ColumnName == string.Empty);
      }
   }
}