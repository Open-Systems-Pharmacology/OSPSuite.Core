using OSPSuite.Assets;

namespace OSPSuite.Core.Domain.Builder
{
   public class ParameterValuesBuildingBlock : PathAndValueEntityBuildingBlock<ParameterValue>
   {
      public ParameterValuesBuildingBlock()
      {
         Icon = IconNames.PARAMETER_VALUES;
      }
   }
}