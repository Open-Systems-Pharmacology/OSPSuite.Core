using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Snapshots.Mappers
{
   public abstract class CurveChartMapper<TCurveChart, TSnapshot> : ObjectBaseSnapshotMapperBase<TCurveChart, TSnapshot, SimulationAnalysisContext> where TCurveChart : Core.Chart.CurveChart where TSnapshot : CurveChart, new()
   {
      private readonly ChartMapper _chartMapper;
      private readonly AxisMapper _axisMapper;
      private readonly CurveMapper _curveMapper;
      private readonly IIdGenerator _idGenerator;
      public Func<TCurveChart> ChartFactoryFunc { get; set; }

      protected CurveChartMapper(ChartMapper chartMapper, AxisMapper axisMapper, CurveMapper curveMapper, IIdGenerator idGenerator)
      {
         _chartMapper = chartMapper;
         _axisMapper = axisMapper;
         _curveMapper = curveMapper;
         _idGenerator = idGenerator;
      }

      public override async Task<TSnapshot> MapToSnapshot(TCurveChart curveChart)
      {
         var snapshot = await SnapshotFrom(curveChart);
         await _chartMapper.MapToSnapshot(curveChart, snapshot);
         snapshot.Axes = await _axisMapper.MapToSnapshots(curveChart.Axes);
         snapshot.Curves = await _curveMapper.MapToSnapshots(curveChart.Curves);
         return snapshot;
      }

      public override async Task<TCurveChart> MapToModel(TSnapshot snapshot, SimulationAnalysisContext simulationAnalysisContext)
      {
         var curveChart = ChartFactoryFunc().WithId(_idGenerator.NewId());
         MapSnapshotPropertiesToModel(snapshot, curveChart);
         await _chartMapper.MapToModel(snapshot, new ChartSnapshotContext(curveChart, simulationAnalysisContext));
         await updateChartAxes(curveChart, snapshot.Axes, simulationAnalysisContext);
         await updateChartCurves(curveChart, snapshot.Curves, simulationAnalysisContext);
         return curveChart;
      }

      private async Task updateChartAxes(Core.Chart.CurveChart curveChart, IReadOnlyList<Axis> snapshotAxes, SnapshotContext snapshotContext)
      {
         var axes = await _axisMapper.MapToModels(snapshotAxes, snapshotContext);
         axes?.Each(curveChart.AddAxis);
      }

      private async Task updateChartCurves(Core.Chart.CurveChart curveChart, IReadOnlyList<Curve> snapshotCurves, SimulationAnalysisContext simulationAnalysisContext)
      {
         var curves = await _curveMapper.MapToModels(snapshotCurves, simulationAnalysisContext);
         curves?.Each(x => curveChart.AddCurve(x, useAxisDefault: false));
      }
   }

   public abstract class CurveChartMapper<TCurveChart> : CurveChartMapper<TCurveChart, CurveChart> where TCurveChart : Core.Chart.CurveChart
   {
      protected CurveChartMapper(ChartMapper chartMapper, AxisMapper axisMapper, CurveMapper curveMapper, IIdGenerator idGenerator) : base(chartMapper, axisMapper, curveMapper, idGenerator)
      {
      }
   }

   public class CurveChartMapper : NewableCurveChartMapper<Core.Chart.CurveChart>
   {
      public CurveChartMapper(ChartMapper chartMapper, AxisMapper axisMapper, CurveMapper curveMapper, IIdGenerator idGenerator) : base(chartMapper, axisMapper, curveMapper, idGenerator)
      {
      }
   }
}