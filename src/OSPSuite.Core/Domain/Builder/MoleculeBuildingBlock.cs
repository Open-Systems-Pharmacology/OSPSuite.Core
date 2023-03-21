using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;

namespace OSPSuite.Core.Domain.Builder
{
   public class MoleculeBuildingBlock : BuildingBlock<IMoleculeBuilder>
   {
      public MoleculeBuildingBlock()
      {
         Icon = IconNames.MOLECULE;
      }

      public IMoleculeBuilder this[string moleculeName] => this.FindByName(moleculeName);

      public IEnumerable<IMoleculeBuilder> AllFloating() => this.Where(x => x.IsFloating);

      public IEnumerable<IMoleculeBuilder> AllPresentFor(IEnumerable<MoleculeStartValuesBuildingBlock> moleculesStartValues)
      {
         var moleculeNames = moleculesStartValues.SelectMany(x => x)
            .Where(moleculeStartValue => moleculeStartValue.IsPresent)
            .Select(moleculeStartValue => moleculeStartValue.MoleculeName)
            .Distinct();


         return moleculeNames.Select(moleculeName => this[moleculeName]).Where(m => m != null);
      }

      public IEnumerable<IMoleculeBuilder> AllPresentFor(MoleculeStartValuesBuildingBlock moleculesStartValues)
      {
         return AllPresentFor(new[] {moleculesStartValues});
      }
   }
}