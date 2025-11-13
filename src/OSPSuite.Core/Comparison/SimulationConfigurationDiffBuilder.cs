using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Comparison
{
   public class SimulationConfigurationDiffBuilder : ShallowDiffBuilder<SimulationConfiguration>
   {
      private readonly IObjectComparer _objectComparer;
      private readonly EnumerableComparer _enumerableComparer;

      public SimulationConfigurationDiffBuilder(IObjectComparer objectComparer, IObjectTypeResolver objectTypeResolver, EnumerableComparer enumerableComparer) : base(objectTypeResolver)
      {
         _objectComparer = objectComparer;
         _enumerableComparer = enumerableComparer;
      }

      public override void Compare(IComparison<SimulationConfiguration> comparison)
      {
         AddShallowDifference(x => x.Individual, comparison);
         
         AddPresentByNameDifference(x => x.ExpressionProfiles, comparison);

         // perform a full comparison of the settings
         _objectComparer.Compare(comparison.ChildComparison(x => x.SimulationSettings));

         CompareValues(x => x.SimModelExportMode, x => x.SimModelExportMode, comparison);
         CompareValues(x => x.CreateAllProcessRateParameters, x => x.CreateAllProcessRateParameters, comparison);
         CompareValues(x => x.PerformCircularReferenceCheck, x => x.PerformCircularReferenceCheck, comparison);
         CompareValues(x => x.ShouldValidate, x => x.ShouldValidate, comparison);
         CompareValues(x => x.ShowProgress, x => x.ShowProgress, comparison);

         _enumerableComparer.CompareEnumerables(comparison, x => x.ModuleConfigurations, x => x.Module.Name);
      }

   }
}