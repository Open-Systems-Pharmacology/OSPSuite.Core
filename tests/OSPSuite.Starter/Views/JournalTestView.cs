using OSPSuite.Utility.Container;
using OSPSuite.Presentation.Regions;
using OSPSuite.Presentation.Views;
using OSPSuite.Starter.Presenters;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Extensions;

namespace OSPSuite.Starter.Views
{
   public partial class JournalTestView : BaseUserControl, IJournalTestView
   {
      private IJournalTestPresenter _presenter;

      public JournalTestView()
      {
         
         InitializeComponent();
         initializeRegions();
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         addPageButton.Click += (o, e) => OnEvent(addNewPageToJournal);
         selectJournalButton.Click += (o, e) => OnEvent(selectJournal);
         exportJournalButton.Click += (o, e) => OnEvent(exportJournal);
         saveDiagramButton.Click += (o, e) => OnEvent(saveDiagram);
         searchButton.Click += (o, e) => OnEvent(searchJournal);
         btnResetOrdering.Click += (o, e) => OnEvent(()=>_presenter.RestoreChronologicalOrder());
         btnView.Click += (o, e) => OnEvent(() => _presenter.ShowInStandardViewer());
      }

      private void searchJournal()
      {
         _presenter.SearchJournal();
      }

      private void saveDiagram()
      {
         _presenter.SaveDiagram();
      }

      private void initializeRegions()
      {
         registerRegion(journalPanel, RegionNames.Journal);
      }

      private void registerRegion(UxDockPanel dockPanel, RegionName regionName)
      {
         IoC.RegisterImplementationOf((IRegion) dockPanel, regionName.Name);
         dockPanel.InitializeWith(regionName); 
      }

      private void exportJournal()
      {
         _presenter.ExportJournal();
      }

      private void selectJournal()
      {
         _presenter.SelectJournal();
      }

      private void addNewPageToJournal()
      {
         _presenter.AddNewPageToJournal();
      }

      public void AttachPresenter(IJournalTestPresenter presenter)
      {
         _presenter = presenter;
      }

      public void AddDiagram(IView view)
      {
         diagramPanel.FillWith(view);
      }
   }

  
}
