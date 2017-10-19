using System.Drawing;

namespace OSPSuite.Presentation.Extensions
{
   public static class StringExtensions
   {
      /// <summary>
      ///    Format the given string so that it can be used in a label :
      ///    1 - Adds : at the end
      ///    2 - Make sure only the first word is uppercase if <paramref name="checkCase" /> is true
      ///    3 - Remove any mnemonic
      /// </summary>
      /// <param name="stringToFormat">The string to format</param>
      /// <param name="checkCase">If set to true (default) the string will also be formated</param>
      /// <param name="addColon">If set to true (default) a colon will be appended to the string</param>
      public static string FormatForLabel(this string stringToFormat, bool checkCase = true, bool addColon = true)
      {
         var terminatingCharacter = ":";
         var stringToCheck = stringToFormat;

         if (checkCase)
            stringToCheck = updateCase(stringToFormat);

         if (!addColon)
            terminatingCharacter = string.Empty;

         return $"{removeMnemonic(stringToCheck)}{terminatingCharacter}";
      }

      private static string updateCase(string stringToFormat)
      {
         if (string.IsNullOrEmpty(stringToFormat))
            return string.Empty;

         var lowerCaseArray = stringToFormat.ToLower().ToCharArray();
         lowerCaseArray[0] = char.ToUpper(lowerCaseArray[0]);

         return new string(lowerCaseArray);
      }

      /// <summary>
      ///    Surrounds the given string with the italic marker
      /// </summary>
      public static string FormatForDescription(this string stringToFormat) => InItalic(stringToFormat);

      public static string InBold(this string stringToFormat) => InHtml(stringToFormat, "b");

      public static string InItalic(this string stringToFormat) => InHtml(stringToFormat, "i");

      public static string InGreen(this string stringToFormat) => InColor(stringToFormat, Color.Green);

      public static string InRed(this string stringToFormat) => InColor(stringToFormat, Color.Red);

      public static string InOrange(this string stringToFormat) => InColor(stringToFormat, Color.Orange);

      public static string InColor(this string stringToFormat, Color color)
      {
         return InHtml(stringToFormat, "span", $"color:rgb({color.R},{color.G},{color.B})");
      }

      public static string InHtml(this string stringToFormat, string marker, string style = null)
      {
         var htmlStyle = string.IsNullOrEmpty(style) ? "" : $" style='{style}'";
         return $"<{marker}{htmlStyle}>{removeMnemonic(stringToFormat)}</{marker}>";
      }

      private static string removeMnemonic(string stringToFormat)
      {
         if (string.IsNullOrEmpty(stringToFormat))
            return stringToFormat;
         return stringToFormat.Replace("&", "&&");
      }

      /// <summary>
      ///    Returns the string formated with the "\n" at the end
      /// </summary>
      public static string AsFullLine(this string stringToFormat)
      {
         return $"{stringToFormat}\n";
      }

      /// <summary>
      ///    Remove any html that might be in the string (only b, i and u tags for now)
      /// </summary>
      /// <param name="stringToFormat"></param>
      /// <returns></returns>
      public static string RemoveHtml(this string stringToFormat)
      {
         if (string.IsNullOrEmpty(stringToFormat))
            return stringToFormat;

         return removeHtmlTags(stringToFormat, "b", "i", "u");
      }

      private static string removeHtmlTags(string s, params string[] tags)
      {
         foreach (var tag in tags)
         {
            s = s.Replace($"<{tag}>", "");
            s = s.Replace($"</{tag}>", "");
            s = s.Replace($"<{tag.ToUpper()}>", "");
            s = s.Replace($"</{tag.ToUpper()}>", "");
            s = s.Replace($"<{tag.ToLower()}>", "");
            s = s.Replace($"</{tag.ToLower()}>", "");
         }
         return s;
      }
   }
}