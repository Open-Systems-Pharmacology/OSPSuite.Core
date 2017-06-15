using System;
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
      string CurveNameForColumn(ISimulation simulation, DataColumn dataColumn, bool addSimulationName = true);

      /// <summary>
      ///    Returns the curves from the <paramref name="curveCharts" /> whose name still matches with the curve path and the
      ///    <paramref name="simulation" />
      ///    if the curve was named with this namer, this indicates the name has not changed
      /// </summary>
      IEnumerable<Curve> CurvesWithOriginalName(ISimulation simulation, IEnumerable<CurveChart> curveCharts, bool includingSimulationName = true);

      /// <summary>
      ///    Renames the curves from <paramref name="simulation"/> charts that have the same name as originally created.
      ///    The list of curves to be renamed is created before running <paramref name="renameAction"/> and then after the action
      ///    is run, the list is renamed. If <paramref name="addSimulationName"/> is true, it's expected that the original curve name and
      ///    new curve name should have the simulation name added
      /// </summary>
      void RenameCurvesWithOriginalNames(ISimulation simulation, Action renameAction, bool addSimulationName = true);
   }

   public class CurveNamer : ICurveNamer
   {
      private readonly IQuantityPathToQuantityDisplayPathMapper _quantityPathToQuantityDisplayPathMapper;

      public CurveNamer(IQuantityPathToQuantityDisplayPathMapper quantityPathToQuantityDisplayPathMapper)
      {
         _quantityPathToQuantityDisplayPathMapper = quantityPathToQuantityDisplayPathMapper;
      }

      public void RenameCurvesWithOriginalNames(ISimulation simulation, Action renameAction, bool addSimulationName = true)
      {
         var curveNamesToRename = CurvesWithOriginalName(simulation, simulation.Charts).ToList();

         renameAction();

         curveNamesToRename.Each(curve => { curve.Name = CurveNameForColumn(simulation, curve.yData, addSimulationName); });
      }

      public string CurveNameForColumn(ISimulation simulation, DataColumn dataColumn, bool addSimulationName)
      {
         return _quantityPathToQuantityDisplayPathMapper.DisplayPathAsStringFor(simulation, dataColumn, addSimulationName);
      }

      private bool hasOriginalName(ISimulation simulation, DataColumn dataColumn, Curve curve, bool includingSimulationName)
      {
         var originalCurveName = CurveNameForColumn(simulation, dataColumn, includingSimulationName);
         return string.Equals(curve.Name, originalCurveName);
      }

      public IEnumerable<Curve> CurvesWithOriginalName(ISimulation simulation, IEnumerable<CurveChart> curveCharts, bool includingSimulationName = true)
      {
         return curveCharts.SelectMany(chart => curvesFromChartWithOriginalName(simulation, chart, includingSimulationName));
      }

      private IEnumerable<Curve> curvesFromChartWithOriginalName(ISimulation simulation, CurveChart chart, bool includingSimulationName)
      {
         return chart.Curves.Where(curve =>
         {
            var dataColumn = curve.yData;
            return hasOriginalName(simulation, dataColumn, curve, includingSimulationName);
         });
      }
   }
}