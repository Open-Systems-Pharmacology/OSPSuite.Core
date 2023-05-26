using OSPSuite.Assets;

namespace OSPSuite.Core.Domain.Builder
{
   public class InitialConditionsBuildingBlock : PathAndValueEntityBuildingBlock<InitialCondition>
   {
      public InitialConditionsBuildingBlock()
      {
         Icon = IconNames.INITIAL_CONDITIONS;
      }
   }
}