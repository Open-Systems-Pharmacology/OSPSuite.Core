using OSPSuite.Assets;

namespace OSPSuite.Core.Domain.Builder
{
   public class ParameterStartValuesBuildingBlock : StartValueBuildingBlock<ParameterStartValue>
   {
      public ParameterStartValuesBuildingBlock()
      {
         Icon = IconNames.PARAMETER_VALUES;
      }
   }
}