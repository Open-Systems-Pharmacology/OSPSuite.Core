using OSPSuite.Core.Import;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Infrastructure.Import.Services;

namespace OSPSuite.Infrastructure.Import.Core.DataFormat
{
   public class DataFormatHeadersWithUnits : AbstractColumnsDataFormat
   {
      private const string _name = "Headers with units";
      private const string _description = "https://github.com/Open-Systems-Pharmacology/OSPSuite.Core/issues/639";
      public override string Name { get; } = _name;

      public override string Description { get; } = _description;

      protected override string ExtractLloq(string description, IUnformattedData data, List<string> keys, ref double rank)
      {
         if (data.GetColumn(description).Any(element => element.Trim().StartsWith("<")))
         {
            rank++;
         }

         return null;
      }

      protected override UnitDescription ExtractUnits(string description, IUnformattedData data, List<string> keys, IReadOnlyList<IDimension> supportedDimensions, ref double rank)
      {
         var (_, unit) = UnitExtractor.ExtractLabelAndUnit(description);
         
         if (string.IsNullOrEmpty(unit))
            return new UnitDescription();

         unit = GetAndValidateUnitFromBrackets(unit, supportedDimensions);
         if (unit != UnitDescription.InvalidUnit) 
            rank++;

         return new UnitDescription(unit);
      }
   }
}