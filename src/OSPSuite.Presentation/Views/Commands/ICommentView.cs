using OSPSuite.Presentation.DTO.Commands;
using OSPSuite.Presentation.Presenters.Commands;

namespace OSPSuite.Presentation.Views.Commands
{
   public interface ICommentView:IModalView<ICommentPresenter>
   {
      void BindTo(IHistoryItemDTO command);
      string CommandDescription { set; }
   }
}