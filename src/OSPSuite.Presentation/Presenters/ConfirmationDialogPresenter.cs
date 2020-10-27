using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation.Presenters
{
   public interface IConfirmationDialogPresenter : IDisposablePresenter
   {
      bool Canceled { get; }

      void ShowMessage(string message);
   }

   public class ConfirmationDialogPresenter : AbstractDisposablePresenter<IConfirmationDialogView, IConfirmationDialogPresenter>,
      IConfirmationDialogPresenter
   {
      public ConfirmationDialogPresenter(IConfirmationDialogView view) : base(view) { }

      public bool Canceled => _view.Canceled;

      public void ShowMessage(string message)
      {
         _view.Message = message;
         _view.Display();
      }
   }
}
