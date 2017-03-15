using OSPSuite.Core.Chart;
using OSPSuite.Presentation.Presenters.Charts;

namespace OSPSuite.Presentation.Services.Charts
{
   public interface ICurveAdapterFactory
   {
      ICurveAdapter CreateFor(ICurve curve, IAxis xAxis, IAxis yAxis);
   }
}