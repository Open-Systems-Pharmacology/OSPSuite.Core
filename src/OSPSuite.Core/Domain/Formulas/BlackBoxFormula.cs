using System.Collections.Generic;

namespace OSPSuite.Core.Domain.Formulas
{
   /// <summary>
   /// Represents a formula that will be used as a placeholder for formula in parameter that should be replaced
   /// during the simulation creation process by generic formula defined in calculation method.
   /// </summary>
   public class BlackBoxFormula : Formula
   {
      protected override double CalculateFor(IEnumerable<IObjectReference> usedObjects, IUsingFormula dependentObject)
      {
         return double.NaN;
      }
   }
}