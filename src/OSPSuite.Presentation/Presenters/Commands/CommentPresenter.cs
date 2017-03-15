using OSPSuite.Assets;
using OSPSuite.Presentation.DTO.Commands;
using OSPSuite.Presentation.Views.Commands;

namespace OSPSuite.Presentation.Presenters.Commands
{
   public interface ICommentPresenter : IDisposablePresenter, IPresenter<ICommentView>
   {
      void EditCommentFor(IHistoryItemDTO historyItemDTO);
   }

   public class CommentPresenter : AbstractDisposablePresenter<ICommentView, ICommentPresenter>, ICommentPresenter
   {
      public CommentPresenter(ICommentView view) : base(view)
      {
      }

      public void EditCommentFor(IHistoryItemDTO historyItemDTO)
      {
         _view.Caption = Captions.Commands.CommentViewCaption(historyItemDTO.State);
         _view.CommandDescription = historyItemDTO.Description;
         _view.BindTo(historyItemDTO);
         _view.Display();
      }
   }
}