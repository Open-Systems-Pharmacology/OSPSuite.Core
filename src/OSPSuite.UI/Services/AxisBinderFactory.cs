using OSPSuite.Core.Chart;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Services.Charts;
using OSPSuite.UI.Binders;
using OSPSuite.UI.Controls;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Format;

namespace OSPSuite.UI.Services
{
   public class AxisBinderFactory : IAxisBinderFactory
   {
      public IAxisBinder Create(Axis axis, object chartControl, CurveChart chart)
      {
         var uxChartControl = chartControl.DowncastTo<UxChartControl>();
         
         return new AxisBinder(axis, uxChartControl, NumericFormatterOptions.Instance);
      }
   }
}