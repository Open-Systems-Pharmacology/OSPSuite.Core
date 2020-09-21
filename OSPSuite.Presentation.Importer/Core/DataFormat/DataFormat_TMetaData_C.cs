using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System;
using OSPSuite.Core.Importer;

namespace OSPSuite.Presentation.Importer.Core.DataFormat
{
   public class DataFormat_TMetaData_C : AbstractColumnsDataFormat
   {
      public override string Name { get; } = "015_TMetaData_C(E)";

      public override string Description { get; } = "https://github.com/Open-Systems-Pharmacology/OSPSuite.Core/issues/639";

      protected override Func<int, string> extractUnits(string description, IUnformattedData data, List<string> keys)
      {
         var units = Regex.Match(description, @"\[.+\]").Value;
         if (String.IsNullOrEmpty(units))
            return _ => "?";
         var unit = units.Substring(1, units.Length - 2).Trim().Split(',').FirstOrDefault()??"?";
         return _ => unit;
      }
   }
}