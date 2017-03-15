using OSPSuite.Core.Commands;

namespace OSPSuite.Core.Extensions
{
   public static class OSPSuiteCommandExtensions
   {
      public static TCommand WithHistoryEntriesFrom<TCommand>(this TCommand targetCommand, IOSPSuiteCommand sourceCommand) where TCommand : IOSPSuiteCommand
      {
         targetCommand.ObjectType = sourceCommand.ObjectType;
         targetCommand.Description = sourceCommand.Description;
         targetCommand.CommandType = sourceCommand.CommandType;
         targetCommand.BuildingBlockType = sourceCommand.BuildingBlockType;
         targetCommand.BuildingBlockName = sourceCommand.BuildingBlockName;
         return targetCommand;
      }
   }
}
