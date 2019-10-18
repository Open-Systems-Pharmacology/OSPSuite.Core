using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.DTO.Commands;
using OSPSuite.Presentation.Presenters.Commands;
using OSPSuite.Presentation.Services.Commands;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_CommentTask : ContextSpecification<ICommentTask>
   {
      protected IHistoryItemDTO _historyItemDTO;
      protected ICommentPresenter _commentPresenter;
      private IApplicationController _applicationController;

      protected override void Context()
      {
         _commentPresenter = A.Fake<ICommentPresenter>();
         _historyItemDTO = A.Fake<IHistoryItemDTO>();
         _applicationController = A.Fake<IApplicationController>();
         A.CallTo(() => _applicationController.Start<ICommentPresenter>()).Returns(_commentPresenter);
         sut = new CommentTask(_applicationController);
      }
   }

   public class When_editign_a_comment_for_an_history_item : concern_for_CommentTask
   {
      protected override void Because()
      {
         sut.EditCommentFor(_historyItemDTO);
      }

      [Observation]
      public void should_leverage_the_comment_presenter_to_edit_the_comment()
      {
         A.CallTo(() => _commentPresenter.EditCommentFor(_historyItemDTO)).MustHaveHappened();
      }
   }

   public class When_editign_a_comment_for_an_null_history_item : concern_for_CommentTask
   {
      protected override void Because()
      {
         sut.EditCommentFor(new NullHistoryItemDTO());
      }

      [Observation]
      public void should_not_leverage_the_comment_presenter_to_edit_the_comment()
      {
         A.CallTo(() => _commentPresenter.EditCommentFor(A<IHistoryItemDTO>._)).MustNotHaveHappened();
      }
   }
}