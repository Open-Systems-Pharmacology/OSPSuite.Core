using OSPSuite.Core.Chart;
using OSPSuite.Presentation.Presenters.Charts;

namespace OSPSuite.Presentation.Services.Charts
{
   public interface ICurveBinderFactory
   {
      ICurveBinder CreateFor(Curve curve, object chartControl, CurveChart chart, IAxisBinder yAxisBinder);
   }
}