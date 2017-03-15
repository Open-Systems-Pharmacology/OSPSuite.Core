using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.DTO.Commands;
using OSPSuite.Presentation.Presenters.Commands;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Services.Commands
{
   public interface ICommentTask
   {
      void EditCommentFor(IHistoryItemDTO historyItemDTO);
   }

   public class CommentTask : ICommentTask
   {
      private readonly IApplicationController _applicationController;

      public CommentTask(IApplicationController applicationController)
      {
         _applicationController = applicationController;
      }

      public void EditCommentFor(IHistoryItemDTO historyItemDTO)
      {
         if (historyItemDTO.IsAnImplementationOf<NullHistoryItemDTO>())
            return;

         using (var presenter = _applicationController.Start<ICommentPresenter>())
         {
            presenter.EditCommentFor(historyItemDTO);
         }
      }
   }
}