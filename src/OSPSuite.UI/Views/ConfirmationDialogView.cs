using OSPSuite.Assets;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;

namespace OSPSuite.UI.Views
{
   public partial class ConfirmationDialogView : BaseModalView, IConfirmationDialogView 
   {
      public ConfirmationDialogView()
      {
         InitializeComponent();
         Text = Captions.ConfirmationDialog;
      }

      public void AttachPresenter(IConfirmationDialogPresenter presenter) { }

      public string Message
      {
         set
         {
            labelControl.Text = value;
         }
      }
   }
}
