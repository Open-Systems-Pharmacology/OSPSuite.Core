using System.Collections.Generic;
using System.Linq;
using OSPSuite.Infrastructure.Import.Core.Extensions;

namespace OSPSuite.Infrastructure.Import.Core.DataFormat
{
   public class DataFormatNonmem : AbstractColumnsDataFormat
   {
      private const string _name = "Nonmem";
      private const string _description = "https://github.com/Open-Systems-Pharmacology/OSPSuite.Core/issues/797";
      public override string Name => _name;
      public override string Description => _description;

      protected override string ExtractLloq(string description, IUnformattedData data, List<string> keys, ref double rank)
      {
         var lloqKey = data.GetHeaders().FindHeader(description + "_LLOQ");
         if (lloqKey == null)
         {
            return "";
         }

         keys.Remove(lloqKey);
         rank++;
         return lloqKey;
      }

      protected override UnitDescription ExtractUnits(string description, IUnformattedData data, List<string> keys, ref double rank)
      {
         var unitKey = data.GetHeaders().FindHeader(description + "_UNIT");
         if (unitKey == null)
         {
            return new UnitDescription();
         }

         keys.Remove(unitKey);
         rank++;
         return new UnitDescription(data.GetColumn(unitKey), unitKey);
      }
   }
}
