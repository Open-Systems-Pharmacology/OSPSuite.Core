using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;

namespace OSPSuite.Core.Chart
{
   public abstract class ResidualsVsTimeChart : AnalysisChartWithLocalRepositories
   {
      public const string ZERO = "Zero";

      public override Curve CreateCurve(DataColumn columnX, DataColumn columnY, string curveName, IDimensionFactory dimensionFactory)
      {
         var curve = base.CreateCurve(columnX, columnY, curveName, dimensionFactory);
         if (string.Equals(columnY.Name, ZERO))
            curve.UpdateMarkerCurve(ZERO);
         else
         {
            curve.Symbol = Symbols.Circle;
            curve.LineStyle = LineStyles.None;
         }
         return curve;
      }
   }
}