using OSPSuite.Assets;

namespace OSPSuite.Core.Domain.Builder
{
   public interface IParameterStartValuesBuildingBlock : IStartValuesBuildingBlock<ParameterStartValue>
   {
   }

   public class ParameterStartValuesBuildingBlock : StartValueBuildingBlock<ParameterStartValue>, IParameterStartValuesBuildingBlock
   {
      public ParameterStartValuesBuildingBlock()
      {
         Icon = IconNames.PARAMETER_START_VALUES;
      }
   }
}