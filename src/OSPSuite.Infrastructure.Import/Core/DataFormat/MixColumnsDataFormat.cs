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
      protected override UnitDescription ExtractUnits(string description, IUnformattedData data, List<string> keys, List<IDimension> supportedDimensions, ref double rank)
      {
         var units = Regex.Match(description, @"\[.+\]").Value;
         if (!string.IsNullOrEmpty(units))
         {
            var unit = units
               .Substring(1, units.Length - 2) //remove the brackets
               .Trim()                         //remove whitespace
               .Split(',')                     //split comma separated list
               .Where(unitName => supportedDimensions.Any(x => x.HasUnit(unitName))) //only accepts valid and supported units
               .FirstOrDefault() ?? UnitDescription.InvalidUnit;     //default = ?
            rank++;
            return new UnitDescription(unit);
         }

         var unitKey = data.GetHeaders().FindHeader(description + "_UNIT");
         if (unitKey == null)
         {
            return new UnitDescription();
         }
         keys.Remove(unitKey);
         rank++;
         return new UnitDescription(data.GetColumn(unitKey).FirstOrDefault(u => !string.IsNullOrEmpty(u)), unitKey);
      }
   }
}
