using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.Presentation.DTO.Commands;
using OSPSuite.Presentation.Presenters.Commands;
using OSPSuite.Presentation.Views.Commands;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_comment_presenter : ContextSpecification<ICommentPresenter>
   {
      protected ICommentView _view;

      protected override void Context()
      {
         _view = A.Fake<ICommentView>();
         sut = new CommentPresenter(_view);
      }
   }

   public class When_told_to_edit_the_comment_for_the_history_item : concern_for_comment_presenter
   {
      private IHistoryItemDTO _historyItemDTO;

      protected override void Context()
      {
         base.Context();
         _historyItemDTO = A.Fake<IHistoryItemDTO>();
      }

      protected override void Because()
      {
         sut.EditCommentFor(_historyItemDTO);
      }

      [Observation]
      public void should_tell_the_view_to_bind_to_the_history_item()
      {
         A.CallTo(() => _view.BindTo(_historyItemDTO)).MustHaveHappened();
      }

      [Observation]
      public void should_display_the_view()
      {
         A.CallTo(() => _view.Display()).MustHaveHappened();
      }
   }
}