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
      private IHeavyWorkPresenter _presenter;

      public HeavyWorkView()
      {
         InitializeComponent();

         FormBorderStyle = FormBorderStyle.FixedToolWindow;
         StartPosition = FormStartPosition.CenterParent;
         MaximizeBox = false;
         MaximizeBox = false;
         ShowInTaskbar = false;
         Opacity = 0.7;
         btnCancel.InitWithImage(ApplicationIcons.Cancel, Captions.CancelButton, ImageLocation.MiddleRight);
         btnCancel.Text = Captions.CancelButton;
         btnCancel.Click += (o, e) => OnEvent(btnCancel_click);
      }

      public void AttachPresenter(IHeavyWorkPresenter presenter)
      {
         base.AttachPresenter(presenter);

         _presenter = presenter;
      }

      public void Display()
      {
         CloseBox = false;
         btnCancel.Visible = CancelVisible;
         if (CancelVisible)
         {
            Padding = new Padding(1);
            this.Paint += (s, e) =>
            {
               ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle,
                  Color.Gray, ButtonBorderStyle.Solid);
            };
         }
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
         _presenter.Cancel();
      }
   }
}