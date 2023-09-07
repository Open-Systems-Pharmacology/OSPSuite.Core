using OSPSuite.Core.Commands.Core;

namespace OSPSuite.Core.Commands
{
   public class OSPSuiteLabelCommand : LabelCommand, IOSPSuiteCommand<IOSPSuiteExecutionContext>
   {
      public string BuildingBlockType { get; set; }
      public string BuildingBlockName { get; set; }

      public void Execute(IOSPSuiteExecutionContext context)
      {
         context.ProjectChanged();
      }
   }
}