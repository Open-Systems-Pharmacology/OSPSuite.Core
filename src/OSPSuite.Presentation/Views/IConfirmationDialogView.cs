using OSPSuite.Presentation.Presenters;

namespace OSPSuite.Presentation.Views
{
   public interface IConfirmationDialogView : IModalView<IConfirmationDialogPresenter>
   {
      string Message { set; }
   }
}
