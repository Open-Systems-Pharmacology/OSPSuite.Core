using OSPSuite.Core.Domain;
using OSPSuite.Presentation.Presenters.Charts;

namespace OSPSuite.Presentation.Views.Charts
{
   public interface ISimpleChartView : IView<ISimpleChartPresenter>
   {
      void AddView(IView chartView);

      bool LogLinSelectionEnabled { get;set; }

      /// <summary>
      /// Sets the indicator to log or lin based on <paramref name="scale"/>
      /// </summary>
      void SetChartScale(Scalings scale);
   }
}