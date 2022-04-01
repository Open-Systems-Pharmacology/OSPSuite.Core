using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Infrastructure.Import.Core.Extensions;
using System.Text.RegularExpressions;
using OSPSuite.Core.Import;

namespace OSPSuite.Infrastructure.Import.Core.DataFormat
{
   public class MixColumnsDataFormat : AbstractColumnsDataFormat
   {
      private const string _name = "Mixin";
      private const string _description = "https://github.com/Open-Systems-Pharmacology/OSPSuite.Core/issues/639\rhttps://github.com/Open-Systems-Pharmacology/OSPSuite.Core/issues/797";
      public override string Name => _name;
      public override string Description => _description;
      
      protected override string ExtractLloq(string description, IDataSheet dataSheet, List<string> keys, ref double rank)
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

      protected override UnitDescription ExtractUnits(string description, IDataSheet dataSheet, List<string> keys, IReadOnlyList<IDimension> supportedDimensions, ref double rank)
      {
         var units = GetLastBracketsOfString(description);

         if (!string.IsNullOrEmpty(units))
         {
            var unit = GetAndValidateUnitFromBrackets(units, supportedDimensions);
            rank++;
            return new UnitDescription(unit);
         }

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
