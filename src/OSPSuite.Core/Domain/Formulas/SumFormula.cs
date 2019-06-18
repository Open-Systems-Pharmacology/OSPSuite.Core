namespace OSPSuite.Core.Domain.Formulas
{
   /// <summary>
   /// Dynamic Formula realizing the sum of all <see cref="IFormulaUsable"/> defined by the condition
   /// </summary>
   public class SumFormula : DynamicFormula
   {
      protected override string Operation => "+";
   }
}