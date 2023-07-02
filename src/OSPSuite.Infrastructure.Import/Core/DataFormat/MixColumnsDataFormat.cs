using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Import;
using OSPSuite.Infrastructure.Import.Core.Extensions;
using OSPSuite.Infrastructure.Import.Services;

namespace OSPSuite.Infrastructure.Import.Core.DataFormat
{
   public class MixColumnsDataFormat : AbstractColumnsDataFormat
   {
      public override string Name => "Mixin";

      public override string Description =>
         "https://github.com/Open-Systems-Pharmacology/OSPSuite.Core/issues/639\rhttps://github.com/Open-Systems-Pharmacology/OSPSuite.Core/issues/797";

      protected override string ExtractLLOQ(string description, DataSheet dataSheet, List<string> keys, ref double rank)
      {
         if (dataSheet.GetColumn(description).Any(element => element.Trim().StartsWith("<")))
         {
            rank++;
            return null;
         }

         var lloqKey = dataSheet.GetHeaders().FindHeader(description + "_LLOQ");
         if (lloqKey == null)
         {
            return "";
         }

         keys.Remove(lloqKey);
         rank++;
         return lloqKey;
      }

      protected override UnitDescription ExtractUnits(string description, DataSheet dataSheet, List<string> keys,
         ColumnInfo columnInfo, ref double rank)
      {
         var (_, unit) = UnitExtractor.ExtractNameAndUnit(description);

         if (!string.IsNullOrEmpty(unit))
         {
            unit = ValidateUnit(unit, columnInfo);
            rank++;
            return new UnitDescription(unit);
         }

         if (dataSheet == null)
            return new UnitDescription();

         var unitKey = dataSheet.GetHeaders().FindHeader(description + "_UNIT");
         if (unitKey == null)
            return new UnitDescription();

         keys.Remove(unitKey);
         rank++;
         return new UnitDescription(dataSheet.GetColumn(unitKey).FirstOrDefault(u => !string.IsNullOrEmpty(u)), unitKey);
      }
   }
}