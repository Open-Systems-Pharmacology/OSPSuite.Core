using System.Threading.Tasks;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Snapshots.Mappers
{
   public class ChartMapper : ObjectBaseSnapshotMapperBase<IChart, Chart, ChartSnapshotContext, Chart>
   {
      public override Task<Chart> MapToSnapshot(IChart chart, Chart snapshot)
      {
         MapModelPropertiesToSnapshot(chart, snapshot);
         snapshot.Settings = chart.ChartSettings;
         snapshot.FontAndSize = chart.FontAndSize;
         snapshot.IncludeOriginData = SnapshotValueFor(chart.IncludeOriginData);
         snapshot.OriginText = SnapshotValueFor(chart.OriginText);
         snapshot.PreviewSettings = SnapshotValueFor(chart.PreviewSettings);
         snapshot.Title = SnapshotValueFor(chart.Title);
         return Task.FromResult(snapshot);
      }

      public override Task<IChart> MapToModel(Chart snapshot, ChartSnapshotContext snapshotContext)
      {
         var chart = snapshotContext.Chart;
         MapSnapshotPropertiesToModel(snapshot, chart);
         chart.ChartSettings.UpdatePropertiesFrom(snapshot.Settings);
         chart.FontAndSize.UpdatePropertiesFrom(snapshot.FontAndSize);
         chart.IncludeOriginData = snapshot.IncludeOriginData.GetValueOrDefault(chart.IncludeOriginData);
         chart.OriginText = snapshot.OriginText;
         chart.PreviewSettings = snapshot.PreviewSettings.GetValueOrDefault(chart.PreviewSettings);
         chart.Title = ModelValueFor(snapshot.Title);
         return Task.FromResult(chart);
      }
   }

   public abstract class NewableCurveChartMapper<TCurveChart> : CurveChartMapper<TCurveChart> where TCurveChart : Core.Chart.CurveChart, new()
   {
      protected NewableCurveChartMapper(ChartMapper chartMapper, AxisMapper axisMapper, CurveMapper curveMapper, IIdGenerator idGenerator) : base(chartMapper, axisMapper, curveMapper, idGenerator)
      {
         ChartFactoryFunc = () => new TCurveChart();
      }
   }
}