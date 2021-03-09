using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using OSPSuite.Core.Domain;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Extensions
{
   public static class StringExtensions
   {
      /// <summary>
      ///    Given a string such as A|B|C returns a string enumerable containing A, B, C
      /// </summary>
      /// <returns></returns>
      public static string[] ToPathArray(this string pathString)
      {
         if (string.IsNullOrEmpty(pathString))
            return new string[] { };

         return pathString.Split(ObjectPath.PATH_DELIMITER.ToCharArray());
      }

      public static string WithEllipsis(this string valueToFormatWithEllipsis)
      {
         return string.Concat(valueToFormatWithEllipsis, "...");
      }

      /// <summary>
      ///    Returns a string that has been removed of pre and post white space
      /// </summary>
      public static string TrimmedValue(this string valueToTrim)
      {
         if (!string.IsNullOrEmpty(valueToTrim))
            return valueToTrim.Trim();

         return valueToTrim;
      }

      /// <summary>
      ///    Returns true if the trimmed string <paramref name="stringToCheck" /> is not empty otherwise false
      /// </summary>
      public static bool StringIsNotEmpty(this string stringToCheck)
      {
         return !string.IsNullOrEmpty(TrimmedValue(stringToCheck));
      }

      /// <summary>
      ///    Returns a path that has been converted to UNC Path
      /// </summary>
      /// <param name="pathToConvert">Path that should be converted</param>
      /// <returns></returns>
      public static string ToUNCPath(this string pathToConvert)
      {
         if (string.IsNullOrEmpty(pathToConvert))
            return pathToConvert;

         return pathToConvert.Replace('\\', '/');
      }

      /// <summary>
      ///    Converts CamelCase string into human-readable names.
      /// </summary>
      /// <example> CamelCase returns Camel Case</example>
      public static string SplitToUpperCase(this string stringToSplit)
      {
         if (stringToSplit.IsNullOrEmpty())
            return stringToSplit;

         //(?!^) - i.e. we're not at the beginning of the string
         //(?=[A-Z]) - i.e. we're just before an uppercase letter
         var r = new Regex(@"(?!^)(?=[A-Z])");
         return r.Replace(stringToSplit, " ");
      }

      /// <summary>
      ///    Returns the molecule name in the given <paramref name="moleculePath" />. We assume that the molecule name is
      ///    always the one before last item in the path
      /// </summary>
      public static string MoleculeName(this IReadOnlyList<string> moleculePath)
      {
         if (moleculePath.Count == 0)
            return string.Empty;

         if (moleculePath.Count == 1)
            return moleculePath[0];

         //returns the one before last
         return moleculePath[moleculePath.Count - 2];
      }

      /// <summary>
      ///    Returns a string where the occurence of a unit in bracket has been removed (only if the unit was the very last item
      ///    of the string)
      ///    <example>
      ///       "Organism|Volume [ml]".StripUnit() => "Organism|Volume
      ///       "AnotherStringWith [xx] not at the end".StripUnit() => "AnotherStringWith [xx] not at the end"
      ///    </example>
      /// </summary>
      public static string StripUnit(this string stringToStrip)
      {
         if (string.IsNullOrEmpty(stringToStrip))
            return stringToStrip;

         if (!stringToStrip.TrimEnd().EndsWith("]"))
            return stringToStrip;

         var indexOfUnitStart = stringToStrip.LastIndexOf("[", StringComparison.Ordinal);

         return indexOfUnitStart == -1 ? stringToStrip : stringToStrip.Remove(indexOfUnitStart, stringToStrip.Length - indexOfUnitStart).TrimEnd();
      }

      /// <summary>
      /// Returns the give text within quotes. 
      /// </summary>
      /// <example>
      /// InQuotes(test) => "test"
      /// </example>
      /// <param name="text">Text to encapsulate in quotes</param>
      public static string InQuotes(this string text) => $"\"{text}\"";
   }
}