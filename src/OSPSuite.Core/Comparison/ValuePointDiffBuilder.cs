using OSPSuite.Core.Domain.Formulas;

namespace OSPSuite.Core.Comparison
{
   public class ValuePointDiffBuilder : DiffBuilder<ValuePoint>
   {
      public override void Compare(IComparison<ValuePoint> comparison)
      {
         CompareDoubleValues(x => x.X, x => x.X, comparison);
         CompareDoubleValues(x => x.Y, x => x.Y, comparison);
         CompareValues(x => x.RestartSolver, x => x.RestartSolver, comparison);
      }
   }
}