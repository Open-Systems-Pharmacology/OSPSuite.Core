using System.Windows.Forms;
using DevExpress.XtraEditors;
using OSPSuite.Assets;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Views
{
   public partial class HeavyWorkCancellableView : BaseView, IHeavyWorkCancellableView
   {
      private IHeavyWorkCancellablePresenter cancelablePresenter;

      public HeavyWorkCancellableView()
      {
         InitializeComponent();
         FormBorderStyle = FormBorderStyle.None;
         StartPosition = FormStartPosition.CenterParent;
         ShowInTaskbar = false;
         btnCancel.InitWithImage(ApplicationIcons.Cancel, Captions.CancelButton, ImageLocation.MiddleRight);
         btnCancel.Text = Captions.CancelButton;
         btnCancel.Click += (o, e) => OnEvent(btnCancel_click);
      }

      public void AttachPresenter(IHeavyWorkCancellablePresenter presenter)
      {
         base.AttachPresenter(presenter);

         cancelablePresenter = presenter;
      }

      public void Display()
      {
         ShowDialog();
      }

      public void CloseView()
      {
         Close();
      }

      public override string Caption
      {
         set
         {
            progressBar.Properties.ShowTitle = !string.IsNullOrEmpty(value);
            progressBar.Text = value;
         }
      }

      public bool Canceled
      {
         get { return false; }
      }

      public bool OkEnabled { get; set; }
      public bool ExtraEnabled { get; set; }
      public bool ExtraVisible { get; set; }
      public bool CancelVisible { get; set; }

      private void btnCancel_click()
      {
         cancelablePresenter.Cancel();
      }
   }
}