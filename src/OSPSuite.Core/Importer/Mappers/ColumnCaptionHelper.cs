using System;

namespace OSPSuite.Core.Importer.Mappers
{
   public interface IColumnCaptionHelper
   {
      /// <summary>
      /// Takes a column string and removes the units section (indicated by surrounding [] braces)
      /// </summary>
      /// <param name="columnCaptionString">The string being trimmed</param>
      /// <returns>The trimmed output if unit delimiters are found, otherwise the unmodified string</returns>
      string TrimUnits(string columnCaptionString);

      /// <summary>
      /// This function separates the unit information from a column caption.
      /// </summary>
      /// <param name="columnName">Name of the column containing unit information.</param>
      /// <returns>The Units if any are found, otherwise empty string</returns>
      string GetUnit(string columnName);

      /// <summary>
      /// This function checks where two columns are equal.
      /// </summary>
      /// <remarks>
      /// The check is case insensitive and before the compare on both strings the units are trimmed away.
      /// </remarks>
      /// <param name="leftSide">First column name to compare.</param>
      /// <param name="rightSide">Second column name to compare.</param>
      /// <returns>Whether the column name should be seen as equal.</returns>
      bool IsEquivalent(string leftSide, string rightSide);
   }

   internal class ColumnCaptionHelper : IColumnCaptionHelper
   {
      private const string OPEN_BRACE = "[";
      private const string CLOSE_BRACE = "]";
      public string TrimUnits(string columnCaptionString)
      {
         return (containsUnitDelimiters(columnCaptionString) ? columnCaptionString.Remove(columnCaptionString.IndexOf(OPEN_BRACE, StringComparison.Ordinal)) : columnCaptionString).Trim();
      }

      public string GetUnit(string columnName)
      {
         if (!containsUnitDelimiters(columnName)) return string.Empty;
         try
         {
            return columnName.Substring(
               columnName.IndexOf(OPEN_BRACE, StringComparison.Ordinal) + 1,
               columnName.IndexOf(CLOSE_BRACE, StringComparison.Ordinal) -
               columnName.IndexOf(OPEN_BRACE, StringComparison.Ordinal) - 1);
         }
         catch (Exception)
         {
            return string.Empty;
         }
      }

      public bool IsEquivalent(string leftSide, string rightSide)
      {
         return string.Equals(TrimUnits(leftSide), TrimUnits(rightSide), StringComparison.InvariantCultureIgnoreCase);
      }

      private static bool containsUnitDelimiters(string columnCaptionString)
      {
         return 
            columnCaptionString.Contains(OPEN_BRACE) && 
            columnCaptionString.Contains(CLOSE_BRACE) && 
            (columnCaptionString.IndexOf(OPEN_BRACE, StringComparison.Ordinal) < columnCaptionString.IndexOf(CLOSE_BRACE, StringComparison.Ordinal));
      }
   }
}
