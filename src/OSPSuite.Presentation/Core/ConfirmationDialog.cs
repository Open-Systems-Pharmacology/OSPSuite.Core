using OSPSuite.Presentation.Presenters;

namespace OSPSuite.Presentation.Core
{
   public interface IConfirmationDialog
   {
      bool Confirm(string message);
   }

   public class ConfirmationDialog : IConfirmationDialog
   {
      private readonly IApplicationController _applicationController;
      public ConfirmationDialog(IApplicationController applicationController)
      {
         _applicationController = applicationController;
      }

      public bool Confirm(string message)
      {
         using (var dialog = _applicationController.Start<IConfirmationDialogPresenter>())
         {
            dialog.ShowMessage(message);
            return !dialog.Canceled;
         }
      }
   }
}
