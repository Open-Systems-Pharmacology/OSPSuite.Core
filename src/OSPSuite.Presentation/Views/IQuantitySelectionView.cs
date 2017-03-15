using OSPSuite.Presentation.Presenters;

namespace OSPSuite.Presentation.Views
{
   public interface IQuantitySelectionView : IView<IQuantitySelectionPresenter>
   {
      string Info { get; set; }
      string InfoError { get; set; }
      bool DeselectAllEnabled { get; set; }
      string Description { get; set; }

      void SetQuantityListView(IView view);
      void SetSelectedQuantityListView(IView view);
   }
}