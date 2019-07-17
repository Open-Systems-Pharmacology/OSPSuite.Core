using System.Linq;
using FakeItEasy;
using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Helpers;
using OSPSuite.Presentation.DTO.Commands;
using OSPSuite.Presentation.Mappers;
using OSPSuite.Presentation.Presenters.Commands;
using OSPSuite.UI.Mappers;

namespace OSPSuite.UI
{
   public abstract class concern_for_HistoryToHistoryDTOMapper : ContextSpecification<IHistoryToHistoryDTOMapper>
   {
      private IHistoryBrowserConfiguration _historyBrowserConfiguration;

      public override void GlobalContext()
      {
 	       base.GlobalContext();
            _historyBrowserConfiguration =new HistoryBrowserConfiguration();
         _historyBrowserConfiguration.AddDynamicColumn("p1", "Col Display1");
         _historyBrowserConfiguration.AddDynamicColumn("p2", "Col Display2");
      }

      protected override void Context()
      {
         sut = new HistoryToHistoryDTOMapper(_historyBrowserConfiguration);
         sut.EnableHistoryPruning = true;
      }
   }

   
   public class When_mapping_an_history_item_representing_a_single_command : concern_for_HistoryToHistoryDTOMapper
   {
      private IHistoryItem _simpleHistoryItem;
      private IHistoryItemDTO _result;
      private ICommand _command;

      protected override void Context()
      {
         base.Context();
         _simpleHistoryItem = A.Fake<IHistoryItem>();
         _command = new MySimpleCommand();
         _command.Visible = true;
         _simpleHistoryItem.State = 1;
         A.CallTo(() => _simpleHistoryItem.User).Returns("toto");
         A.CallTo(() => _simpleHistoryItem.Command).Returns(_command);
         _command.CommandType = "CommandType";
         _command.ObjectType = "ObjectType";
         _command.Description = "Description";
         _command.Comment = "One comment";
         _command.ExtendedDescription = "One LongDescription";
         _command.AddExtendedProperty("p1","one value for p1");
         _command.AddExtendedProperty("p2","one value for p2");
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_simpleHistoryItem);
      }

      [Observation]
      public void should_return_a_dto_object_mapped_to_the_history_item()
      {
         _result.State.ShouldBeEqualTo(_simpleHistoryItem.State);
         _result.User.ShouldBeEqualTo(_simpleHistoryItem.User);
         _result.DateTime.ShouldBeEqualTo(_simpleHistoryItem.DateTime);
         _result.CommandType.ShouldBeEqualTo(_command.CommandType);
         _result.ObjectType.ShouldBeEqualTo(_command.ObjectType);
         _result.Description.ShouldBeEqualTo(_command.Description);
         _result.Comment.ShouldBeEqualTo(_command.Comment);
         _result.ExtendedDescription.ShouldBeEqualTo(_command.ExtendedDescription);
         _result.ExtendedProperties[0].ShouldBeEqualTo("one value for p1");
         _result.ExtendedProperties[1].ShouldBeEqualTo("one value for p2");
      }
   }


   public class When_mapping_an_history_item_that_contains_a_macro_command_with_sub_command_that_are_not_visible : concern_for_HistoryToHistoryDTOMapper
   {
      private IHistoryItem _historyItem;
      private IMacroCommand _macroCommand;
      private IHistoryItemDTO _result;
      private ICommand _subCommand2;
      private ICommand _subCommand1;
      private ICommand _subCommand3;

      protected override void Context()
      {
         base.Context();
         _historyItem = A.Fake<IHistoryItem>();
         _macroCommand = A.Fake<IMacroCommand>();
         _macroCommand.Visible = true;
         _subCommand1 = A.Fake<ICommand>();
         _subCommand2 = A.Fake<ICommand>();
         _subCommand3 =A.Fake<ICommand>();
         _subCommand1.Visible = true;
         _subCommand2.Visible = false;
         _subCommand3.Visible = true;
         A.CallTo(() => _historyItem.Command).Returns(_macroCommand);
         A.CallTo(() => _macroCommand.All()).Returns(new[] { _subCommand1, _subCommand2, _subCommand3 });
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_historyItem);
      }

      [Observation]
      public void should_return_a_dto_object_that_contain_one_child_for_each_visible_sub_command()
      {
         _result.Children().Count().ShouldBeEqualTo(2);
      }
   }


   public class When_mapping_an_history_item_that_contains_a_macro_command_with_only_one_sub_command_visible : concern_for_HistoryToHistoryDTOMapper
   {
      private IHistoryItem _historyItem;
      private IMacroCommand _macroCommand;
      private IMacroCommand _subCommand1;
      private ICommand _subCommand2;
      private ICommand _subCommand3;
      private ICommand _subCommand11;
      private IMacroCommand _subCommand12;
      private ICommand _subCommand121;
      private ICommand _subCommand122;
      private IHistoryItemDTO _result;

      protected override void Context()
      {
         base.Context();
         _macroCommand = A.Fake<IMacroCommand>();
         _macroCommand.Visible = true;
         _subCommand1 = A.Fake<IMacroCommand>();
         _subCommand2 = A.Fake<ICommand>();
         _subCommand3 = A.Fake<ICommand>();
         _subCommand1.Visible = true;
         _subCommand2.Visible = false;
         _subCommand3.Visible = true;
         _subCommand11 = A.Fake<ICommand>();
         _subCommand11.Visible = false;
         _subCommand12 = A.Fake<IMacroCommand>();
         _subCommand12.Visible = true;
         _subCommand121 = A.Fake<ICommand>();
         _subCommand121.Visible = false;
         _subCommand122 = A.Fake<ICommand>();
         _subCommand122.Visible = true;
         _historyItem = A.Fake<IHistoryItem>();
         A.CallTo(() => _historyItem.Command).Returns(_macroCommand);
         A.CallTo(() => _macroCommand.All()).Returns(new[] { _subCommand1, _subCommand2, _subCommand3 });
         A.CallTo(() => _subCommand1.All()).Returns(new[] { _subCommand11, _subCommand12 });
         A.CallTo(() => _subCommand12.All()).Returns(new[] { _subCommand121, _subCommand122 });
      }

      protected override void Because()
      {
         _result = sut.MapFrom(_historyItem);
      }

      [Test]
      public void should_return_a_dto_object_that_does_not_contain_the_macro_command()
      {
         _result.Children().Count().ShouldBeEqualTo(2);
         _result.Children().ElementAt(0).Command.ShouldBeEqualTo(_subCommand3);
         _result.Children().ElementAt(1).Command.ShouldBeEqualTo(_subCommand122);

      }
   }
   
   public class When_mapping_an_history_item_that_contains_a_macro_command_with_many_sub_commands_visible : concern_for_HistoryToHistoryDTOMapper
   {
      private IHistoryItem _historyItem;
      private IMacroCommand _macroCommand;
      private IMacroCommand _subCommand1;
      private ICommand _subCommand2;
      private ICommand _subCommand3;
      private ICommand _subCommand11;
      private IMacroCommand _subCommand12;
      private ICommand _subCommand121;
      private ICommand _subCommand122;
      private IHistoryItemDTO _result;

      protected override void Context()
      {
         base.Context();
         _macroCommand = A.Fake<IMacroCommand>();
         _macroCommand.Description = "_macroCommand";
         _macroCommand.Visible = true;
         _subCommand1 = A.Fake<IMacroCommand>();
         _subCommand1.Description = "_subCommand1";
         _subCommand2 = A.Fake<ICommand>();
         _subCommand2.Description = "_subCommand2"; 
         _subCommand3 = A.Fake<ICommand>();
         _subCommand3.Description = "_subCommand3";
         _subCommand1.Visible = true;
         _subCommand2.Visible = false;
         _subCommand3.Visible = false;
         _subCommand11 = A.Fake<ICommand>();
         _subCommand11.Description = "_subCommand11";
         _subCommand11.Visible = false;
         _subCommand12 = A.Fake<IMacroCommand>();
         _subCommand12.Description = "_subCommand12";
         _subCommand12.Visible = true;
         _subCommand121 = A.Fake<ICommand>();
         _subCommand121.Description = "_subCommand121";
         _subCommand121.Visible = false;
         _subCommand122 = A.Fake<ICommand>();
         _subCommand122.Description = "_subCommand122";
         _subCommand122.Visible = true;
         _historyItem = A.Fake<IHistoryItem>();

         A.CallTo(() => _historyItem.Command).Returns(_macroCommand);
         A.CallTo(() => _macroCommand.All()).Returns(new[] { _subCommand1, _subCommand2, _subCommand3 });
         A.CallTo(() => _subCommand1.All()).Returns(new[] { _subCommand11, _subCommand12 });
         A.CallTo(() => _subCommand12.All()).Returns(new[] { _subCommand121, _subCommand122 });
      }
      protected override void Because()
      {
         _result = sut.MapFrom(_historyItem);
      }

      [Test]
      public void should_return_a_dto_object_that_does_not_contain_the_macro_command()
      {
         _result.Command.ShouldBeEqualTo(_subCommand122);

      }
   }
}