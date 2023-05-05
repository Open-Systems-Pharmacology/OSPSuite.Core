using OSPSuite.Assets;

namespace OSPSuite.Core.Domain.Builder
{
   public class ParameterValuesBuildingBlock : StartValueBuildingBlock<ParameterValue>
   {
      public ParameterValuesBuildingBlock()
      {
         Icon = IconNames.PARAMETER_VALUES;
      }
   }
}