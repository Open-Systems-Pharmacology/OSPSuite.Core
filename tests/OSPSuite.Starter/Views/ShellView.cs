using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using OSPSuite.Assets;
using OSPSuite.Starter.Presenters;
using OSPSuite.UI.Views;
using OSPSuite.Utility.Container;

namespace OSPSuite.Starter.Views
{
   public partial class ShellView : BaseShell, IShellView
   {
      private IShellPresenter _presenter;

      public ShellView(IContainer container)
      {
         InitializeComponent();

         // WITH AUTOFAC
         var dxContainer = container.Resolve<DxContainer>();
         dxContainer.RibbonBarManager = ribbon.Manager;
         dxContainer.BarManager = ribbon.Manager;
         dxContainer.UserLookAndFeel = defaultLookAndFeel.LookAndFeel;
         dxContainer.XtraTabbedMdiManager = xtraTabbedMdiManager;
         dxContainer.ApplicationMenu = new ApplicationMenu();
         dxContainer.PanelControl = new PanelControl();
         dxContainer.RibbonControl = ribbon;

         //WITH WINDSOR
         container.RegisterImplementationOf(ribbon.Manager);
         container.RegisterImplementationOf(ribbon.Manager as BarManager);
         container.RegisterImplementationOf(defaultLookAndFeel.LookAndFeel);
         container.RegisterImplementationOf(xtraTabbedMdiManager);
         container.RegisterImplementationOf(new ApplicationMenu());
         container.RegisterImplementationOf(new PanelControl());
         container.RegisterImplementationOf(ribbon);
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         ApplicationIcon = ApplicationIcons.PKSim;
      }

      public void AttachPresenter(IShellPresenter presenter)
      {
         _presenter = presenter;
         base.AttachPresenter(presenter);
      }
   }
}