using OSPSuite.Core.Domain.Formulas;

namespace OSPSuite.Core.Comparison
{
   public class DynamicFormulaDiffBuilder : DiffBuilder<DynamicFormula>
   {
      private readonly FormulaWithFormulaStringDiffBuilder _formulaDiffBuilder;

      public DynamicFormulaDiffBuilder(FormulaWithFormulaStringDiffBuilder formulaDiffBuilder)
      {
         _formulaDiffBuilder = formulaDiffBuilder;
      }

      public override void Compare(IComparison<DynamicFormula> comparison)
      {
         _formulaDiffBuilder.Compare(comparison);
         CompareValues(x => x.Criteria, x => x.Criteria, comparison);
         CompareStringValues(x => x.VariablePattern, x => x.VariablePattern, comparison);
      }
   }
}