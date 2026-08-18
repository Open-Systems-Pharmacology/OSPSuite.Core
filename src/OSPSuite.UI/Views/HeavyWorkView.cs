using System.Drawing;
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
      private const int VIEW_WIDTH = 172;
      private const int VIEW_HEIGHT = 110;
      private const int VIEW_HEIGHT_WITH_CANCEL = 155;

      private IHeavyWorkPresenter _presenter;

      public HeavyWorkView()
      {
         InitializeComponent();
         FormBorderStyle = FormBorderStyle.None;
         StartPosition = FormStartPosition.CenterParent;
         //scrollbars should never appear in the progress popup, whatever the screen scaling
         uxLayoutControl.AutoScroll = false;
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
         //sizes defined in the designer are not scaled with screen DPI and need to be set explicitly
         ClientSize = new Size(UIConstants.Size.ScaleForScreenDPI(VIEW_WIDTH), UIConstants.Size.ScaleForScreenDPI(VIEW_HEIGHT));
         if (CancelVisible)
         {
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            MaximizeBox = false;
            Height = UIConstants.Size.ScaleForScreenDPI(VIEW_HEIGHT_WITH_CANCEL);
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