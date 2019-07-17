using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Extensions;

namespace OSPSuite.Core.Commands
{
   public class concern_for_OSPSuiteCommandExtensions : StaticContextSpecification
   {
      private IOSPSuiteCommand _originalCommand;
      private IOSPSuiteCommand _resultingCommand;

      protected override void Context()
      {
         _originalCommand = A.Fake<IOSPSuiteCommand>();
         _resultingCommand = A.Fake<IOSPSuiteCommand>();

         _originalCommand.CommandType = "commandType";
         _originalCommand.Description = "description";
         _originalCommand.ObjectType = "objectType";
         _originalCommand.BuildingBlockType = "buildingBlockType";
         _originalCommand.BuildingBlockName = "buildingBlockName";
      }

      protected override void Because()
      {
         _resultingCommand = _resultingCommand.WithHistoryEntriesFrom(_originalCommand);
      }

      [Observation]
      public void When_copying_history_data_to_the_new_command_all_three_data_must_be_transferred()
      {
         _resultingCommand.CommandType.ShouldBeEqualTo(_originalCommand.CommandType);
         _resultingCommand.Description.ShouldBeEqualTo(_originalCommand.Description);
         _resultingCommand.ObjectType.ShouldBeEqualTo(_originalCommand.ObjectType);
         _resultingCommand.BuildingBlockType.ShouldBeEqualTo(_originalCommand.BuildingBlockType);
         _resultingCommand.BuildingBlockName.ShouldBeEqualTo(_originalCommand.BuildingBlockName);
      }
   }
}