using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;

namespace OSPSuite.Core.Domain.Builder
{
   public class MoleculeBuildingBlock : BuildingBlock<MoleculeBuilder>
   {
      public MoleculeBuildingBlock()
      {
         Icon = IconNames.MOLECULE;
      }

      public MoleculeBuilder this[string moleculeName] => this.FindByName(moleculeName);

      public IEnumerable<MoleculeBuilder> AllFloating() => this.Where(x => x.IsFloating);

      public IEnumerable<MoleculeBuilder> AllPresentFor(IEnumerable<InitialConditionsBuildingBlock> initialConditions)
      {
         var moleculeNames = initialConditions.SelectMany(x => x)
            .Where(initialCondition => initialCondition.IsPresent)
            .Select(initialCondition => initialCondition.MoleculeName)
            .Distinct();


         return moleculeNames.Select(moleculeName => this[moleculeName]).Where(m => m != null);
      }

      public IEnumerable<MoleculeBuilder> AllPresentFor(InitialConditionsBuildingBlock moleculesStartValues)
      {
         return AllPresentFor(new[] {moleculesStartValues});
      }
   }
}