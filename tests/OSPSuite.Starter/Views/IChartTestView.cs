using OSPSuite.Presentation.Views;
using OSPSuite.Presentation.Views.Charts;
using OSPSuite.Starter.Presenters;

namespace OSPSuite.Starter.Views
{
   public interface IChartTestView : IView<IChartTestPresenter>
   {
      void AddChartEditorView(IChartEditorView editorPresenterView);
      void AddChartDisplayView(IChartDisplayView displayPresenterView);
   }
}