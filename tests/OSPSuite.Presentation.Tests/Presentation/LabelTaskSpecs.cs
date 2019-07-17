using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.DTO.Commands;
using OSPSuite.Presentation.Presenters.Commands;
using OSPSuite.Presentation.Services.Commands;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_LabelTask : ContextSpecification<ILabelTask>
   {
      protected IHistoryManager _historyManager;
      protected ILabelPresenter _labelPresenter;
      private IApplicationController _applicationController;

      protected override void Context()
      {
         _historyManager = A.Fake<IHistoryManager>();
         _labelPresenter = A.Fake<ILabelPresenter>();
         _applicationController= A.Fake<IApplicationController>();
         A.CallTo(() => _applicationController.Start<ILabelPresenter>()).Returns(_labelPresenter);
         sut = new LabelTask(_applicationController);
      }

      protected override void Because()
      {
         sut.AddLabelTo(_historyManager);
      }
   }

   public class When_creating_a_label : concern_for_LabelTask
   {
      [Observation]
      public void should_leverage_the_label_presenter_to_retrieve_the_required_information_for_the_new_label()
      {
         A.CallTo(() => _labelPresenter.CreateLabel()).MustHaveHappened();
      }
   }

   public class When_the_action_of_retrieving_a_label_info_was_canceled_by_the_user : concern_for_LabelTask
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _labelPresenter.CreateLabel()).Returns(false);
      }

      [Observation]
      public void should_not_add_an_item_to_the_history()
      {
         A.CallTo(() => _historyManager.AddLabel(A<LabelCommand>.Ignored)).MustNotHaveHappened();
      }
   }

   public class When_the_action_of_retrieving_a_label_info_was_successful : concern_for_LabelTask
   {
      private string _label;
      private string _comment;
      private ILabelCommand _command;

      protected override void Context()
      {
         base.Context();
         _label = "label";
         _comment = "Comment";
         var labelDTO = new LabelDTO {Label = _label, Comment = _comment};

         A.CallTo(() => _labelPresenter.CreateLabel()).Returns(true);
         A.CallTo(() => _labelPresenter.LabelDTO).Returns(labelDTO);
         A.CallTo(() => _historyManager.AddLabel(A<ILabelCommand>.Ignored))
          .Invokes(x => _command = x.Arguments.Get<ILabelCommand>(0));
      }

      [Observation]
      public void should_add_a_command_to_the_history()
      {
         _command.ShouldNotBeNull();
         _command.Comment.ShouldBeEqualTo(_comment);
         _command.Description.ShouldBeEqualTo(_label);
      }
   }
}