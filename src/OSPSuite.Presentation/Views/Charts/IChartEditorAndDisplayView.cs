using OSPSuite.Presentation.Presenters.Charts;

namespace OSPSuite.Presentation.Views.Charts
{
   public interface IChartEditorAndDisplayView : IView<IChartEditorAndDisplayPresenter>
   {
      string SaveLayoutToString();
      void LoadLayoutFromString(string layoutString);
      void AddEditor(IView view);
      void AddDisplay(IView view);
   }
}