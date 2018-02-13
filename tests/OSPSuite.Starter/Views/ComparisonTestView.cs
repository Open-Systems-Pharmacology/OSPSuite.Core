using OSPSuite.Presentation.Regions;
using OSPSuite.Starter.Presenters;
using OSPSuite.UI.Controls;
using OSPSuite.Utility.Container;

namespace OSPSuite.Starter.Views
{
   public partial class ComparisonTestView : BaseUserControl, IComparisonTestView
   {
      private IComparisonTestPresenter _presenter;

      public ComparisonTestView()
      {
         InitializeComponent();
         initializeRegions();
      }

      public void AttachPresenter(IComparisonTestPresenter presenter)
      {
         _presenter = presenter;
      }

      private void initializeRegions()
      {
         registerRegion(comparisonPanel, RegionNames.Comparison);
      }

      private void registerRegion(UxDockPanel dockPanel, RegionName regionName)
      {
         IoC.RegisterImplementationOf((IRegion) dockPanel, regionName.Name);
         dockPanel.InitializeWith(regionName);
      }
   }
}