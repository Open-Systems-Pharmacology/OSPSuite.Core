using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Commands
{
   public class OSPSuiteInfoCommand : InfoCommand, IOSPSuiteCommand<IOSPSuiteExecutionContext>
   {
      public string BuildingBlockType
      {
         get => ExtendedPropertyValueFor(Constants.Command.BUILDING_BLOCK_TYPE);
         set => AddExtendedProperty(Constants.Command.BUILDING_BLOCK_TYPE, value);
      }

      public string BuildingBlockName
      {
         get => ExtendedPropertyValueFor(Constants.Command.BUILDING_BLOCK_NAME);
         set => AddExtendedProperty(Constants.Command.BUILDING_BLOCK_NAME, value);
      }

      public void Execute(IOSPSuiteExecutionContext context)
      {
         context.ProjectChanged();
      }
   }
}