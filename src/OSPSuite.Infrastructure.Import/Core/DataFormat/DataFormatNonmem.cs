using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Import;
using OSPSuite.Infrastructure.Import.Core.Extensions;

namespace OSPSuite.Infrastructure.Import.Core.DataFormat
{
   public class DataFormatNonmem : AbstractColumnsDataFormat
   {
      public override string Name => "Nonmem";
      public override string Description => "https://github.com/Open-Systems-Pharmacology/OSPSuite.Core/issues/797";

      protected override string ExtractLLOQ(string description, DataSheet dataSheet, List<string> keys, ref double rank)
      {
         var lloqKey = dataSheet.GetHeaders().FindHeader(description + "_LLOQ");
         if (lloqKey == null)
         {
            return "";
         }

         keys.Remove(lloqKey);
         rank++;
         return lloqKey;
      }

      protected override UnitDescription ExtractUnits(string description, DataSheet dataSheet, List<string> keys, ColumnInfo columnInfo,
         ref double rank)
      {
         if (dataSheet == null)
            return new UnitDescription();

         var unitKey = dataSheet.GetHeaders().FindHeader(description + "_UNIT");
         if (unitKey == null)
         {
            return new UnitDescription();
         }

         keys.Remove(unitKey);
         rank++;
         return new UnitDescription(dataSheet.GetColumn(unitKey).FirstOrDefault(u => !string.IsNullOrEmpty(u)), unitKey);
      }
   }
}