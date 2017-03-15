using OSPSuite.Presentation.Presenters.Main;
using OSPSuite.Presentation.Regions;
using OSPSuite.Presentation.Views.Journal;

namespace OSPSuite.Presentation.Presenters.Journal
{
   public interface IJournalDiagramMainPresenter : IPresenter<IJournalDiagramMainView>, IMainViewItemPresenter
   {
      void SaveDiagram();
      void RestoreChronologicalOrder();
   }

   public abstract class JournalDiagramMainPresenter : AbstractPresenter<IJournalDiagramMainView, IJournalDiagramMainPresenter>, IJournalDiagramMainPresenter
   {
      private readonly IRegion _region;
      private readonly IJournalDiagramPresenter _journalDiagramPresenter;

      protected JournalDiagramMainPresenter(
         IJournalDiagramMainView view,
         IJournalDiagramPresenter journalDiagramPresenter,
         IRegionResolver regionResolver,
         RegionName regionName)
         : base(view)
      {
         _region = regionResolver.RegionWithName(regionName);
         View.InsertDiagram(journalDiagramPresenter.View);
         _region.Add(View);
         _journalDiagramPresenter = journalDiagramPresenter;
      }

      public void ToggleVisibility()
      {
         _region.ToggleVisibility();
      }

      public void SaveDiagram()
      {
         _journalDiagramPresenter.SaveDiagram();
      }

      public void RestoreChronologicalOrder()
      {
         _journalDiagramPresenter.RestoreChronologicalOrder();
      }
   }
}
