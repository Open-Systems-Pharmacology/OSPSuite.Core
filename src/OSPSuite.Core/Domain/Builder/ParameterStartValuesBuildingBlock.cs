using OSPSuite.Assets;

namespace OSPSuite.Core.Domain.Builder
{
   public interface IParameterStartValuesBuildingBlock : IStartValuesBuildingBlock<IParameterStartValue>
   {
   }

   public class ParameterStartValuesBuildingBlock : StartValueBuildingBlock<IParameterStartValue>, IParameterStartValuesBuildingBlock
   {
      public ParameterStartValuesBuildingBlock()
      {
         Icon = IconNames.PARAMETER_START_VALUES;
      }
   }
}