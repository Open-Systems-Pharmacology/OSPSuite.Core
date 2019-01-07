using System.Linq;
using System.Windows.Forms;
using OSPSuite.Utility.Extensions;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraTab;
using DevExpress.XtraTab.ViewInfo;
using OSPSuite.Assets;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;
using OSPSuite.UI.Services;

namespace OSPSuite.UI.Views
{
   public partial class EditAnalyzableView : BaseMdiChildTabbedView, IEditAnalyzableView, IViewWithPopup
   {
      public BarManager PopupBarManager { get; private set; }

      public EditAnalyzableView(IShell shell, IImageListRetriever imageListRetriever)
         : base(shell)
      {
         InitializeComponent();
         tabAnalyzable.ClosePageButtonShowMode = ClosePageButtonShowMode.InActiveTabPageHeader;
         tabAnalyzable.CloseButtonClick += (o, e) => OnEvent(closeButtonClick, e as ClosePageButtonEventArgs);
         tabAnalyzable.MouseDown += (o, e) => OnEvent(onTabMouseDown, e);
         tabAnalyzable.SelectedPageChanged += (o, e) => OnEvent(onSelectedPageChanged, e);
         PopupBarManager = new BarManager {Form = this, Images = imageListRetriever.AllImagesForContextMenu};
         ApplicationIcon = ApplicationIcons.Simulation;
         tabAnalyzable.EnableTabManagementInHeader(PopupBarManager);
      }

      private void onSelectedPageChanged(TabPageChangedEventArgs e)
      {
         editAnalyzablePresenter.SetSelectedTabIndex(tabAnalyzable.SelectedTabPageIndex);
      }

      private void closeButtonClick(ClosePageButtonEventArgs e)
      {
         var closingTab = e.Page as XtraTabPage;
         if (closingTab == null) return;
         editAnalyzablePresenter.RemoveAnalysis(analysisPresenterFrom(closingTab));
      }

      private static ISimulationAnalysisPresenter analysisPresenterFrom(XtraTabPage closingTab)
      {
         return closingTab.Tag.DowncastTo<ISimulationAnalysisPresenter>();
      }

      private void onTabMouseDown(MouseEventArgs e)
      {
         if (e.Button != MouseButtons.Right)
            return;

         var hi = tabAnalyzable.CalcHitInfo(e.Location);
         if (hi == null || hi.HitTest != XtraTabHitTest.PageHeader || hi.Page == null)
            return;

         var analysisPresenter = analysisPresenterFrom(hi.Page);
         editAnalyzablePresenter.CreatePopupMenuFor(analysisPresenter).At(PointToClient(Cursor.Position));
      }

      public override void AddSubItemView(ISubPresenterItem simulationItem, IView viewToAdd)
      {
         var page = AddPageFor(simulationItem, viewToAdd);
         page.ShowCloseButton = DefaultBoolean.False;
      }

      public void AddAnalysis(ISimulationAnalysisPresenter simulationAnalysisPresenter)
      {
         var page = AddPageFor(tabAnalyzable.TabPages.Count, simulationAnalysisPresenter.BaseView);
         page.Tag = simulationAnalysisPresenter;
         page.ShowCloseButton = DefaultBoolean.True;
         tabAnalyzable.SelectedTabPage = page;
      }

      public void RemoveAnalysis(ISimulationAnalysisPresenter simulationAnalysisPresenter)
      {
         var tab = tabAnalyzable.TabPages.FirstOrDefault(x => Equals(x.Tag, simulationAnalysisPresenter));
         removeTab(tab);
      }

      private void removeTab(XtraTabPage tab)
      {
         if (tab == null) return;
         tabAnalyzable.TabPages.Remove(tab);
      }

      public void UpdateTrafficLightFor(ISimulationAnalysisPresenter simulationAnalysisPresenter, ApplicationIcon icon)
      {
         foreach (XtraTabPage page in  tabAnalyzable.TabPages)
         {
            if (page.Tag != simulationAnalysisPresenter)
               continue;

            page.Image = icon.ToImage(UIConstants.ICON_SIZE_TAB);
         }
      }

      public void SelectTabByIndex(int tabIndex)
      {
         if (tabAnalyzable.TabPages.Count <= tabIndex)
            return;

         tabAnalyzable.SelectedTabPageIndex = tabIndex;
      }

      public override XtraTabControl TabControl => tabAnalyzable;

      private IEditAnalyzablePresenter editAnalyzablePresenter => _presenter.DowncastTo<IEditAnalyzablePresenter>();

   }
}