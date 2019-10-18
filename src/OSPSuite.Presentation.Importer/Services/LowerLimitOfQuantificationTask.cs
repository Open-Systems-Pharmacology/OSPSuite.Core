using System;
using OSPSuite.Core.Importer;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Services
{
   public interface ILowerLimitOfQuantificationTask
   {
      /// <summary>
      ///    Detects if <paramref name="stringValue" /> matches the pattern for lower limit of quantification such as "&lt;0.01"
      /// </summary>
      /// <returns>true if the string has the characteristics of first character is &lt; and all other characters form a number</returns>
      bool IsLLOQ(string stringValue);

      /// <summary>
      ///    Returns the parsed lower limit of quantification as a double from <paramref name="stringValue" />.
      /// </summary>
      /// <returns>
      ///    The lower limit of quantification from the portion of the string after &lt; If the string cannot be parsed,
      ///    returns double.NaN
      /// </returns>
      float ParseLLOQ(string stringValue);

      /// <summary>
      ///    The interpretation of lower limit of quantification is the value from <paramref name="stringValue" /> divided by 2
      /// </summary>
      /// <returns>lower limit of quantification divided by 2 or double.NaN if the limit cannot be determined</returns>
      float GetInterpretationOfLLOQ(string stringValue, ImportDataColumn importDataColumn);

      /// <summary>
      ///   Evaluates the <paramref name="limit1"/> and <paramref name="limit2"/> to determine the best value for LLOQ
      /// </summary>
      /// <returns>If one value is NaN, returns the other value, otherwise returns the greater of the two</returns>
      float DetermineBestLLOQ(float limit1, float limit2);

      /// <summary>
      ///   Applies the <paramref name="lloq"/> to the <paramref name="column"/>
      /// </summary>
      void AttachLLOQ(ImportDataColumn column, float lloq);
   }

   public class LowerLimitOfQuantificationTask : ILowerLimitOfQuantificationTask
   {
      private const string LESS_THAN = "<";

      public bool IsLLOQ(string stringValue)
      {
         return !float.IsNaN(ParseLLOQ(stringValue));
      }

      public float ParseLLOQ(string stringValue)
      {
         if (!stringValue.Contains(LESS_THAN))
            return float.NaN;

         var trimmedString = trimString(stringValue);

         try
         {
            return trimmedString.ConvertedTo<float>();
         }
         catch
         {
            return float.NaN;
         }
      }


      public float GetInterpretationOfLLOQ(string stringValue, ImportDataColumn importDataColumn)
      {
         if (float.IsNaN(ParseLLOQ(stringValue)) && float.IsNaN(importDataColumn.LLOQProperty()))
            return float.NaN;

         return DetermineBestLLOQ(ParseLLOQ(stringValue), importDataColumn.LLOQProperty()) / 2;
      }

      public float DetermineBestLLOQ(float limit1, float limit2)
      {
         if (float.IsNaN(limit1))
            return limit2;

         if(float.IsNaN(limit2))
            return limit1;

         return Math.Max(limit1, limit2);
      }

      public void AttachLLOQ(ImportDataColumn column, float lloq)
      {
         column.AttachLLOQ(DetermineBestLLOQ(lloq, column.LLOQProperty()));
      }

      private static string trimString(string stringValue)
      {
         // trim leading and trailing whitespace and then the leading '<' symbol
         return stringValue.Trim().TrimStart(LESS_THAN.ToCharArray());
      }
   }
}