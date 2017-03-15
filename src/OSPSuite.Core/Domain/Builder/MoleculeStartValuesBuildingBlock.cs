using OSPSuite.Assets;

namespace OSPSuite.Core.Domain.Builder
{
   public interface IMoleculeStartValuesBuildingBlock : IStartValuesBuildingBlock<IMoleculeStartValue>
   {
   }

   public class MoleculeStartValuesBuildingBlock : StartValueBuildingBlock<IMoleculeStartValue>, IMoleculeStartValuesBuildingBlock
   {
      public MoleculeStartValuesBuildingBlock()
      {
         Icon = IconNames.MOLECULE_START_VALUES;
      }
   }
}