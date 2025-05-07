using System.Windows.Forms;
using DevExpress.XtraEditors;
using OSPSuite.Assets;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Extensions;

namespace OSPSuite.UI.Views
{
   public partial class HeavyWorkView : BaseView, IHeavyWorkView
   {
      private IHeavyWorkPresenter _presenter;

      public HeavyWorkView()
      {
         InitializeComponent();
         FormBorderStyle = FormBorderStyle.None;
         StartPosition = FormStartPosition.CenterParent;
         btnCancel.InitWithImage(ApplicationIcons.Cancel, Captions.CancelButton, ImageLocation.MiddleRight);
         btnCancel.Text = Captions.CancelButton;
         btnCancel.Click += (o, e) => OnEvent(cancelButtonClick);
      }

      public void AttachPresenter(IHeavyWorkPresenter presenter)
      {
         base.AttachPresenter(presenter);

         _presenter = presenter;
      }

      private void setLayout()
      {
         ShowInTaskbar = false;
         if (CancelVisible)
         {
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            MaximizeBox = false;
            Height = MaximumSize.Height;
            Opacity = 1.0;
         }
         else
         {
            FormBorderStyle = FormBorderStyle.None;
            TransparencyKey = BackColor;
            Opacity = 0.7;
         }

         layoutControlItemCancelButton.ContentVisible = CancelVisible;
      }

      public void Display()
      {
         setLayout();
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

      public bool Canceled => false;

      public bool OkEnabled { get; set; }
      public bool ExtraEnabled { get; set; }
      public bool ExtraVisible { get; set; }
      public bool CancelVisible { get; set; }

      private void cancelButtonClick()
      {
         _presenter.Cancel();
      }
   }
}