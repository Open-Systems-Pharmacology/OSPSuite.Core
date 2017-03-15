using OSPSuite.Utility.Container;
using OSPSuite.Presentation.Regions;
using OSPSuite.Starter.Presenters;
using OSPSuite.UI.Controls;

namespace OSPSuite.Starter.Views
{
   public partial class ExplorerTestView : BaseUserControl, IExplorerTestView
   {
      private IExplorerTestPresenter _presenter;

      public ExplorerTestView()

      {
         InitializeComponent();
         initializeRegions();
      }

      public void AttachPresenter(IExplorerTestPresenter presenter)
      {
         _presenter = presenter;
      }

      private void initializeRegions()
      {
         registerRegion(explorerPanel, RegionNames.Explorer);
      }

      private void registerRegion(UxDockPanel dockPanel, RegionName regionName)
      {
         IoC.RegisterImplementationOf((IRegion)dockPanel, regionName.Name);
         dockPanel.InitializeWith(regionName);
      }
   }
}