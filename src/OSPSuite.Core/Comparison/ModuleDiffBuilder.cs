using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Comparison
{
   /// <summary>
   ///    Compares two modules by content: the merge behavior, the PK-Sim version the module was exported with and every
   ///    contained building block. Building blocks that can occur only once per module (spatial structure, molecules,
   ///    reactions, ...) are matched by type, parameter values and initial conditions are matched by type and name.
   /// </summary>
   public class ModuleDiffBuilder : DiffBuilder<Module>
   {
      private readonly ObjectBaseDiffBuilder _objectBaseDiffBuilder;
      private readonly EnumerableComparer _enumerableComparer;

      public ModuleDiffBuilder(ObjectBaseDiffBuilder objectBaseDiffBuilder, EnumerableComparer enumerableComparer)
      {
         _objectBaseDiffBuilder = objectBaseDiffBuilder;
         _enumerableComparer = enumerableComparer;
      }

      public override void Compare(IComparison<Module> comparison)
      {
         _objectBaseDiffBuilder.Compare(comparison);
         CompareValues(x => x.MergeBehavior, x => x.MergeBehavior, comparison);
         CompareStringValues(x => x.PKSimVersion, Captions.Diff.PKSimVersion, comparison);

         if (!comparison.Settings.OnlyComputingRelevant)
            CompareValues(x => x.IsPKSimModule, x => x.IsPKSimModule, comparison);

         _enumerableComparer.CompareEnumerables(comparison, x => x.BuildingBlocks, areSameBuildingBlock, x => x.Name);
      }

      private static bool areSameBuildingBlock(IBuildingBlock buildingBlock1, IBuildingBlock buildingBlock2)
      {
         if (buildingBlock1.GetType() != buildingBlock2.GetType())
            return false;

         return !canOccurMultipleTimesInModule(buildingBlock1) || buildingBlock1.IsNamed(buildingBlock2.Name);
      }

      private static bool canOccurMultipleTimesInModule(IBuildingBlock buildingBlock) =>
         buildingBlock.IsAnImplementationOf<InitialConditionsBuildingBlock>() || buildingBlock.IsAnImplementationOf<ParameterValuesBuildingBlock>();
   }
}
