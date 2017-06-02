using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Services
{
   public interface ICurveNamer
   {
      string CurveNameForColumn(ISimulation simulation, DataColumn dataColumn);
      IReadOnlyList<ICurve> CurvesWithOriginalName(ISimulation simulation, IEnumerable<ICurveChart> curveCharts);
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

      public IReadOnlyList<ICurve> CurvesWithOriginalName(ISimulation simulation, IEnumerable<ICurveChart> curveCharts)
      {
         var curvesHavingOriginalName = new List<ICurve>();

         curveCharts.Each(chart =>
         {
            curvesHavingOriginalName.AddRange(curvesFromChartWithOriginalName(simulation, chart));
         });
         return curvesHavingOriginalName;
      }

      private IEnumerable<ICurve> curvesFromChartWithOriginalName(ISimulation simulation, ICurveChart chart)
      {
         var curvesWithOriginalName = chart.Curves.Where(curve =>
         {
            var dataColumn = curve.yData;
            return hasOriginalName(simulation, dataColumn, curve);
         });
         return curvesWithOriginalName;
      }
   }
}