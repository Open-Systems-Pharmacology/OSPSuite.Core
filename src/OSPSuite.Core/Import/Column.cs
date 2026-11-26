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

      // The private set is needed by OSPSuite-R
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
         ErrorStdDevColumn = baseColumn.ErrorStdDevColumn;
      }

      public string Name { get; set; }

      public UnitDescription Unit { get; set; }
      public IDimension Dimension { get; set; }

      public string LloqColumn { get; set; }

      public string ErrorStdDev { get; set; }

      /// <summary>
      ///    Column defining the error type of each data set. When set, <see cref="ErrorStdDev" /> is the default used for data
      ///    sets without a value in this column
      /// </summary>
      public string ErrorStdDevColumn { get; set; }

      /// <summary>
      ///    True if every data set is imported with a geometric standard deviation, i.e. the error type is geometric and not read
      ///    from a column
      /// </summary>
      public bool ErrorStdDevIsAlwaysGeometric => ErrorStdDev == Constants.STD_DEV_GEOMETRIC && string.IsNullOrEmpty(ErrorStdDevColumn);

      public override string ToString()
      {
         return $"{Name} [{Unit}]";
      }

      public bool MissingUnitMapping()
      {
         return
            //manual selection: selected unit not set and the column is not always a Geometric Standard Deviation
            (Unit.SelectedUnit == UnitDescription.InvalidUnit && !ErrorStdDevIsAlwaysGeometric)
            ||
            //select from a column: selected unit is null (not set) and no column has been selected
            (Unit.SelectedUnit == null &&
             Unit.ColumnName == string.Empty);
      }
   }
}