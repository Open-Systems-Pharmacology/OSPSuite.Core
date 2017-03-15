using System.Collections.Generic;
using System.Linq;

namespace OSPSuite.Core.Domain
{
   public class CalculationMethodCombination
   {
      public IReadOnlyList<CalculationMethodWithCompoundName> CalculationMethods;

      public CalculationMethodCombination(IReadOnlyList<CalculationMethodWithCompoundName> calculationMethodWithCompoundNames)
      {
         CalculationMethods = calculationMethodWithCompoundNames;
      }

      public bool HasSingleCategory => AllCategories().Count() == 1;

      public bool HasSingleCompound => CompoundCount == 1;

      public int CompoundCount => CalculationMethods.Select(cm => cm.CompoundName).Distinct().Count();

      public IEnumerable<CalculationMethod> CalculationMethodsForCategory(string category)
      {
         return CalculationMethods.Where(x => string.Equals(x.CalculationMethod.Category, category)).Select(cm => cm.CalculationMethod);
      }

      public bool AllCompoundsUseSameCalculationMethod()
      {
         return AllCategories().All(category => CalculationMethodsForCategory(category).Distinct().Count() == 1);
      }

      public IEnumerable<string> AllCategories()
      {
         return CalculationMethods.Select(calculationMethod => calculationMethod.CalculationMethod.Category).Distinct();
      }
   }
}