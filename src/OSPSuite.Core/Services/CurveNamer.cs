using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Mappers;

namespace OSPSuite.Core.Services
{
   public interface ICurveNamer
   {
      string CurveNameForColumn(ISimulation simulation, DataColumn dataColumn);

      /// <summary>
      /// Returns the curves from the <paramref name="curveCharts"/> whose name still matches with the curve path and the <paramref name="simulation"/>
      /// if the curve was named with this namer, this indicates the name has not changed
      /// </summary>
      IEnumerable<ICurve> CurvesWithOriginalName(ISimulation simulation, IEnumerable<ICurveChart> curveCharts);
   }

   public class CurveNamer : ICurveNamer
   {
      private readonly IQuantityPathToQuantityDisplayPathMapper _quantityPathToQuantityDisplayPathMapper;

      public CurveNamer(IQuantityPathToQuantityDisplayPathMapper quantityPathToQuantityDisplayPathMapper)
      {
         _quantityPathToQuantityDisplayPathMapper = quantityPathToQuantityDisplayPathMapper;
      }

      public string CurveNameForColumn(ISimulation simulation, DataColumn dataColumn)
      {
         return curveNameForDataColumn(simulation, dataColumn);
      }

      private string curveNameForDataColumn(ISimulation simulation, DataColumn dataColumn)
      {
         return _quantityPathToQuantityDisplayPathMapper.DisplayPathAsStringFor(simulation, dataColumn, addSimulationName: true);
      }

      private bool hasOriginalName(ISimulation simulation, DataColumn dataColumn, ICurve curve)
      {
         var originalCurveName = curveNameForDataColumn(simulation, dataColumn);
         return string.Equals(curve.Name, originalCurveName);
      }

      public IEnumerable<ICurve> CurvesWithOriginalName(ISimulation simulation, IEnumerable<ICurveChart> curveCharts)
      {
         return curveCharts.SelectMany(chart => curvesFromChartWithOriginalName(simulation, chart));
      }

      private IEnumerable<ICurve> curvesFromChartWithOriginalName(ISimulation simulation, ICurveChart chart)
      {
         return chart.Curves.Where(curve =>
         {
            var dataColumn = curve.yData;
            return hasOriginalName(simulation, dataColumn, curve);
         });
      }
   }
}