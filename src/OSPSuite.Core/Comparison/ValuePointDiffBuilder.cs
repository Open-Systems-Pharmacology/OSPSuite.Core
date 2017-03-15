using OSPSuite.Core.Domain.Formulas;

namespace OSPSuite.Core.Comparison
{
   public class ValuePointDiffBuilder : DiffBuilder<ValuePoint>
   {
      public override void Compare(IComparison<ValuePoint> comparison)
      {
         CompareValues(x => x.X, x => x.X, comparison);
         CompareValues(x => x.Y, x => x.Y, comparison);
         CompareValues(x => x.RestartSolver, x => x.RestartSolver, comparison);
      }
   }
}