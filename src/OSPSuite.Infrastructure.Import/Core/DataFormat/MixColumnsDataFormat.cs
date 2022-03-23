using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Infrastructure.Import.Core.Extensions;
using System.Text.RegularExpressions;
using OSPSuite.Core.Import;
using OSPSuite.Infrastructure.Import.Services;

namespace OSPSuite.Infrastructure.Import.Core.DataFormat
{
   public class MixColumnsDataFormat : AbstractColumnsDataFormat
   {
      private const string _name = "Mixin";
      private const string _description = "https://github.com/Open-Systems-Pharmacology/OSPSuite.Core/issues/639\rhttps://github.com/Open-Systems-Pharmacology/OSPSuite.Core/issues/797";
      public override string Name => _name;
      public override string Description => _description;
      
      protected override string ExtractLloq(string description, IUnformattedData data, List<string> keys, ref double rank)
      {
         if (data.GetColumn(description).Any(element => element.Trim().StartsWith("<")))
         {
            rank++;
            return null;
         }

         var lloqKey = data.GetHeaders().FindHeader(description + "_LLOQ");
         if (lloqKey == null)
         {
            return "";
         }
         keys.Remove(lloqKey);
         rank++;
         return lloqKey;
      }

      protected override UnitDescription ExtractUnits(string description, IUnformattedData data, List<string> keys, IReadOnlyList<IDimension> supportedDimensions, ref double rank)
      {
         var (_, unit) = UnitExtractor.ExtractNameAndUnit(description);

         if (!string.IsNullOrEmpty(unit))
         {
            unit = ValidateUnit(unit, supportedDimensions);
            rank++;
            return new UnitDescription(unit);
         }

         if (data == null)
            return new UnitDescription();

         var unitKey = data.GetHeaders().FindHeader(description + "_UNIT");
         if (unitKey == null)
            return new UnitDescription();

         keys.Remove(unitKey);
         rank++;
         return new UnitDescription(data.GetColumn(unitKey).FirstOrDefault(u => !string.IsNullOrEmpty(u)), unitKey);
      }
   }
}
