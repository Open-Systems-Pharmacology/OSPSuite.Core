using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Comparison
{
   public class SimulationSettingsDiffBuilder : DiffBuilder<ISimulationSettings>
   {
      private readonly ObjectBaseDiffBuilder _objectBaseDiffBuilder;
      private readonly EnumerableComparer _enumerableComparer;
      private readonly IObjectComparer _objectComparer;

      public SimulationSettingsDiffBuilder(ObjectBaseDiffBuilder objectBaseDiffBuilder,
         EnumerableComparer enumerableComparer, IObjectComparer objectComparer)
      {
         _objectBaseDiffBuilder = objectBaseDiffBuilder;
         _enumerableComparer = enumerableComparer;
         _objectComparer = objectComparer;
      }

      public override void Compare(IComparison<ISimulationSettings> comparison)
      {
         _objectBaseDiffBuilder.Compare(comparison);
         _objectComparer.Compare(comparison.ChildComparison(x => x.OutputSchema));
         _objectComparer.Compare(comparison.ChildComparison(x => x.Solver));
         _objectComparer.Compare(comparison.ChildComparison(x => x.OutputSelections));
         // not normal usage, but OutputSelections is only a enumeration at this point
         _enumerableComparer.CompareEnumerables(comparison, x => x.OutputSelections.AllOutputs, item => item.Path); 
         _enumerableComparer.CompareEnumerables(comparison, x => x.ChartTemplates, item => item.Name);
      }
   }
}