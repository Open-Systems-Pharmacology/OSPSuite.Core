namespace OSPSuite.Core.Domain.Formulas
{
   public static class FormulaWithFormulaStringExtensions
   {
      public static T WithFormulaString<T>(this T formula, string formulaString) where T : FormulaWithFormulaString
      {
         formula.FormulaString = formulaString;
         return formula;
      }
   }
}
