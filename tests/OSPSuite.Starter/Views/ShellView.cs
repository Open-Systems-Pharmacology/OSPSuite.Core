using OSPSuite.Utility.Container;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using OSPSuite.Starter.Presenters;
using OSPSuite.UI.Views;

namespace OSPSuite.Starter.Views
{
   public partial class ShellView : BaseShell, IShellView
   {
      private IShellPresenter _presenter;

      public ShellView(IContainer container)
      {
         InitializeComponent();

         container.RegisterImplementationOf(ribbon.Manager);
         container.RegisterImplementationOf(ribbon.Manager as BarManager);
         container.RegisterImplementationOf(defaultLookAndFeel.LookAndFeel);
         container.RegisterImplementationOf(xtraTabbedMdiManager);
         container.RegisterImplementationOf(new ApplicationMenu());
         container.RegisterImplementationOf(new PanelControl());
         container.RegisterImplementationOf(ribbon);
      }

      public void AttachPresenter(IShellPresenter presenter)
      {
         _presenter = presenter;
         base.AttachPresenter(presenter);
      }

   }
}