using System.Windows.Forms;
using OSPSuite.Presentation.Presenters.Charts;

namespace OSPSuite.Presentation.Views.Charts
{
   public interface IChartEditorAndDisplayControl : IView<IChartEditorAndDisplayPresenter>
   {
      string SaveLayoutToString();
      void LoadLayoutFromString(string layoutString);
      void AddEditor(IView view);
      void AddDisplay(Control chartDisplayControl);
   }
}