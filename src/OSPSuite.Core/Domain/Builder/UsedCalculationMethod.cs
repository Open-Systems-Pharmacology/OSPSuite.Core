using System;

namespace OSPSuite.Core.Domain.Builder
{
   public class UsedCalculationMethod
   {
      /// <summary>
      /// Name of the category where the calculation is defined
      /// </summary>
      public string Category { get; set; }

      /// <summary>
      /// Name of the calculation method
      /// </summary>
      public string CalculationMethod { get; set; }

      [Obsolete("For serialization")]
      public UsedCalculationMethod()
      {
      }

      public UsedCalculationMethod(string category, string calculationMethod)
      {
         Category = category;
         CalculationMethod = calculationMethod;
      }

      public UsedCalculationMethod Clone()
      {
         return new UsedCalculationMethod(Category, CalculationMethod);
      }
   }
}