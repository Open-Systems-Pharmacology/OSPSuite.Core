using System;
using System.Text.RegularExpressions;
using OSPSuite.Core.Extensions;

namespace OSPSuite.Infrastructure.Import.Services
{
   public static class UnitExtractor
   {
      /// <summary>
      ///    Extracts the name and  unit from the given <paramref name="text" /> by matching unit has being the last entry in
      ///    [].
      ///    If not unit is found, return an empty string for the unit
      /// </summary>
      /// <example>
      ///    ExtractNameAndUnit("Concentration [mg/l]") => "Concentration" and "mg/l"
      /// </example>
      public static (string name, string unit) ExtractNameAndUnit(string text)
      {
         var (unitWithBrackets, unit) = extractUnit(text);
         if (string.IsNullOrEmpty(unit))
            return (text, unit);

         var bracketIndex = text.LastIndexOf(unitWithBrackets, StringComparison.Ordinal);
         var name = text.Remove(bracketIndex, unitWithBrackets.Length).TrimmedValue();
         return (name, unit);
      }

      private static (string unitWithBrackets, string unit) extractUnit(string text)
      {
         var trimmedText = text.TrimmedValue();
         var openBracketIndex = trimmedText.LastIndexOf("[", StringComparison.Ordinal);
         var closeBracketIndex = trimmedText.LastIndexOf("]", StringComparison.Ordinal);
         
         //No open bracket or close bracket is not the last element => invalid unit
         if (openBracketIndex < 0 || closeBracketIndex != trimmedText.Length - 1)
            return (string.Empty, string.Empty);

         var unitWithBrackets = trimmedText.Substring(openBracketIndex);
         var unit = unitWithBrackets.Substring(1, unitWithBrackets.Length - 2).TrimmedValue();

         return (unitWithBrackets, unit);
      }
   }
}