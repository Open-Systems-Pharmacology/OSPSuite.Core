using OSPSuite.Core.Domain.Formulas;

namespace OSPSuite.Core.Domain
{
   public static class UsingFormulaExtensions
   {
      public static T WithFormula<T>(this T usingFormula, IFormula formula) where T : IUsingFormula
      {
         usingFormula.Formula = formula;
         return usingFormula;
      }
   }
}