using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Import;
using OSPSuite.Infrastructure.Import.Services;

namespace OSPSuite.Infrastructure.Import.Core.DataFormat
{
   public class DataFormatHeadersWithUnits : AbstractColumnsDataFormat
   {
      public override string Name => "Headers with units";
      public override string Description => "https://github.com/Open-Systems-Pharmacology/OSPSuite.Core/issues/639";

      protected override string ExtractLLOQ(string description, DataSheet dataSheet, List<string> keys, ref double rank)
      {
         if (dataSheet.GetColumn(description).Any(element => element.Trim().StartsWith("<")))
         {
            rank++;
         }

         return null;
      }

      protected override UnitDescription ExtractUnits(string description, DataSheet dataSheet, List<string> keys, ColumnInfo columnInfo,
         ref double rank)
      {
         var (_, unit) = UnitExtractor.ExtractNameAndUnit(description);

         if (string.IsNullOrEmpty(unit))
            return new UnitDescription();

         unit = ValidateUnit(unit, columnInfo);
         if (unit != UnitDescription.InvalidUnit)
            rank++;

         return new UnitDescription(unit);
      }
   }
}