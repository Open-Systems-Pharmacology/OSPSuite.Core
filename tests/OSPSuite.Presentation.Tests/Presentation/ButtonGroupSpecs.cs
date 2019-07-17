using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Presentation.MenuAndBars;

namespace OSPSuite.Presentation.Presentation
{

   public class When_adding_a_button_to_the_tool_bar : ContextSpecification<IButtonGroup>
   {
      private string _toolBarName;
      private IRibbonBarItem _buttonToAdd1;
      private IRibbonBarItem _buttonToAdd2;

      protected override void Context()
      {
         _toolBarName = "Simulation";
         _buttonToAdd1 = A.Fake<IRibbonBarItem>();
         _buttonToAdd2 = A.Fake<IRibbonBarItem>();
         sut = new ButtonGroup { Caption = _toolBarName };
      }

      protected override void Because()
      {
         sut.AddButton(_buttonToAdd1);
         sut.AddButton(_buttonToAdd2);
      }


      [Observation]
      public void should_add_the_button_into_the_underlying_list()
      {
         sut.Buttons.ShouldOnlyContainInOrder(_buttonToAdd1, _buttonToAdd2);
      }
   }


   public class When_retrieving_the_name_of_the_tool_bar : ContextSpecification<IButtonGroup>
   {
      private string _toolBarName;
      private string _result;

      [Observation]
      public void should_return_the_name_the_tool_bar_was_created_with()
      {
         _result.ShouldBeEqualTo(_toolBarName);
      }


      protected override void Because()
      {
         _result = sut.Caption;
      }

      protected override void Context()
      {
         _toolBarName = "Simulation";
         sut = new ButtonGroup { Caption = _toolBarName };
      }
   }


   public class When_creating_a_tool_bar_with_the_dsl : ContextSpecification<IButtonGroup>
   {
      private string _toolBarName;
      private string _result;
      private IRibbonBarItem _buttonToAdd1;
      private IRibbonBarItem _buttonToAdd2;

      [Observation]
      public void should_intialize_the_tool_bar_correctly()
      {
         _result.ShouldBeEqualTo(_toolBarName);
         sut.Buttons.ShouldOnlyContainInOrder(_buttonToAdd1, _buttonToAdd2);
      }


      protected override void Because()
      {
         _result = sut.Caption;
      }

      protected override void Context()
      {
         _toolBarName = "Simulation";
         _buttonToAdd1 = A.Fake<IRibbonBarItem>();
         _buttonToAdd2 = A.Fake<IRibbonBarItem>();
         sut = CreateButtonGroup.WithCaption(_toolBarName).WithButton(_buttonToAdd1).WithButton(_buttonToAdd2);
      }
   }
}	