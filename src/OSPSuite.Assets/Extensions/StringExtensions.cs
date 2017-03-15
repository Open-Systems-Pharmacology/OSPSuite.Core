using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Assets.Extensions
{
   public static class StringExtensions
   {
      private static readonly Dictionary<string, string> _irregularPlurals = new Dictionary<string, string>
      {
         {"analysis", "analyses"},
         {"Analysis", "Analyses"},
         {"is", "are"},
         {"data", "data"},
         {"Data", "Data"}
      };

      public static string ToAcronym(this string input, string separator = "")
      {
         var words = input.Trim().Split(' ');
         return words.Select(word => word.First().ToString().ToUpper()).ToString(separator);
      }

      public static string Pluralize(this string input)
      {
         var irregularPlural = isKnownIrregularPlurals(input);
         if (string.IsNullOrEmpty(irregularPlural))
            return string.Concat(input, "s");

         return irregularPluralsFrom(input, irregularPlural);
      }

      private static string irregularPluralsFrom(string input, string plural)
      {
         return input.Replace(plural, _irregularPlurals[plural]);
      }

      private static string isKnownIrregularPlurals(string input)
      {
         return _irregularPlurals.Keys.FirstOrDefault(input.EndsWith);
      }

      /// <summary>
      ///    Returns the pluralized string <paramref name="stringToPluralize" /> only if the <paramref name="listOf" /> has 2
      ///    elements ore more
      /// </summary>
      public static string PluralizeIf(this string stringToPluralize, IReadOnlyList<object> listOf)
      {
         return PluralizeIf(stringToPluralize, listOf.Count);
      }

      /// <summary>
      ///    Returns the pluralized string <paramref name="stringToPluralize" /> only if the <paramref name="count" /> is greater
      ///    then or equal 2.
      /// </summary>
      public static string PluralizeIf(this string stringToPluralize, int count)
      {
         return PluralizeIf(stringToPluralize, count > 1);
      }

      /// <summary>
      ///    Returns the pluralized string <paramref name="stringToPluralize" /> only if the <paramref name="pluralize" /> is
      ///    <c>true</c>
      ///    or <paramref name="stringToPluralize" /> otherwise
      /// </summary>
      public static string PluralizeIf(this string stringToPluralize, bool pluralize)
      {
         return pluralize ? stringToPluralize.Pluralize() : stringToPluralize;
      }
   }
}