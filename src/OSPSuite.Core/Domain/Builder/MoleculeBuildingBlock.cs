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

      public IEnumerable<MoleculeBuilder> AllPresentFor(IEnumerable<MoleculeStartValuesBuildingBlock> moleculesStartValues)
      {
         var moleculeNames = moleculesStartValues.SelectMany(x => x)
            .Where(moleculeStartValue => moleculeStartValue.IsPresent)
            .Select(moleculeStartValue => moleculeStartValue.MoleculeName)
            .Distinct();


         return moleculeNames.Select(moleculeName => this[moleculeName]).Where(m => m != null);
      }

      public IEnumerable<MoleculeBuilder> AllPresentFor(MoleculeStartValuesBuildingBlock moleculesStartValues)
      {
         return AllPresentFor(new[] {moleculesStartValues});
      }
   }
}