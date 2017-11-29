using OSPSuite.Core.Chart;
using OSPSuite.Presentation.Presenters.Charts;

namespace OSPSuite.Presentation.Services.Charts
{
   public interface IAxisBinderFactory
   {
      IAxisBinder Create(Axis axis, object chartControl, CurveChart chart);
   }
}