using OSPSuite.Core.Chart;
using OSPSuite.Presentation.Presenters.Charts;
using OSPSuite.Presentation.Services.Charts;
using OSPSuite.UI.Binders;

namespace OSPSuite.UI.Services
{
   public class CurveAdapterFactory : ICurveAdapterFactory
   {
      public ICurveAdapter CreateFor(ICurve curve, IAxis xAxis, IAxis yAxis)
      {
         var curveAdapter = new CurveAdapter(curve, xAxis, yAxis);
         curveAdapter.ShowAllSeries();
         return curveAdapter;
      }
   }
}