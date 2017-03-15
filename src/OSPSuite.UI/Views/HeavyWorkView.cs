using System.Windows.Forms;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;

namespace OSPSuite.UI.Views
{
   public partial class HeavyWorkView : BaseView, IHeavyWorkView
   {

      public HeavyWorkView()
      {
         InitializeComponent();
         FormBorderStyle = FormBorderStyle.None;
         StartPosition = FormStartPosition.CenterParent;
         ShowInTaskbar = false;
         Opacity = 0.7;
      }

      public void AttachPresenter(IHeavyWorkPresenter presenter)
      {
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
   }
}