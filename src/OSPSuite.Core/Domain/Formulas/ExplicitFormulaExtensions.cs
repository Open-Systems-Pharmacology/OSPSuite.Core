namespace OSPSuite.Core.Domain.Formulas
{
   public static class ExplicitFormulaExtensions
   {
      public static T WithOriginId<T>(this T formula, string originId) where T : ExplicitFormula
      {
         formula.OriginId = originId;
         return formula;
      }
   }
}