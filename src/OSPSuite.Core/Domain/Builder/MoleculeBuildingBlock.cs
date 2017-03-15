using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;

namespace OSPSuite.Core.Domain.Builder
{
   public interface IMoleculeBuildingBlock : IBuildingBlock<IMoleculeBuilder>
   {
      IMoleculeBuilder this[string moleculeName] { get; }
      IEnumerable<IMoleculeBuilder> AllFloating();
      IEnumerable<IMoleculeBuilder> AllPresentFor(IMoleculeStartValuesBuildingBlock moleculesStartValues);
   }

   public class MoleculeBuildingBlock : BuildingBlock<IMoleculeBuilder>, IMoleculeBuildingBlock
   {
      public MoleculeBuildingBlock()
      {
         Icon = IconNames.MOLECULE;
      }
      public IMoleculeBuilder this[string moleculeName]
      {
         get { return this.FirstOrDefault(moleculeBuilder => moleculeBuilder.Name == moleculeName); }
      }

      public IEnumerable<IMoleculeBuilder> AllFloating()
      {
         return this.Where(x => x.IsFloating);
      }

      public IEnumerable<IMoleculeBuilder> AllPresentFor(IMoleculeStartValuesBuildingBlock moleculesStartValues)
      {
         var moleculeNames = (from moleculeStartValue in moleculesStartValues
                              where moleculeStartValue.IsPresent
                              select moleculeStartValue.MoleculeName).Distinct();


         return moleculeNames.Select(moleculeName => this[moleculeName]).Where(m => m != null);
      }
   }
}