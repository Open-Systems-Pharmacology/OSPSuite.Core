using System;
using System.Collections.Generic;
using System.Linq;

namespace OSPSuite.Infrastructure.Import.Core
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

      public string ExtractUnit(IUnformattedData data, IEnumerable<string> row)
      {
         if (ColumnName == null)
         {
            return SelectedUnit;
         }

         return row.ElementAt(data.GetColumnDescription(ColumnName).Index);
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