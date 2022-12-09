using OSPSuite.Assets;

namespace OSPSuite.Core.Domain.Builder
{
   public interface IMoleculeStartValuesBuildingBlock : IStartValuesBuildingBlock<MoleculeStartValue>
   {
   }

   public class MoleculeStartValuesBuildingBlock : StartValueBuildingBlock<MoleculeStartValue>, IMoleculeStartValuesBuildingBlock
   {
      public MoleculeStartValuesBuildingBlock()
      {
         Icon = IconNames.MOLECULE_START_VALUES;
      }
   }
}