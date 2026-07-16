using System.Threading.Tasks;

namespace OSPSuite.Core.Snapshots.Mappers
{
   public class CurveOptionsMapper : SnapshotMapperBase<Core.Chart.CurveOptions, CurveOptions, SnapshotContext>
   {
      private readonly Core.Chart.CurveOptions _defaultCurveOption = new Core.Chart.CurveOptions();

      public override Task<CurveOptions> MapToSnapshot(Core.Chart.CurveOptions curveOptions)
      {
         return SnapshotFrom(curveOptions, x =>
         {
            x.Visible = SnapshotValueFor(curveOptions.Visible, _defaultCurveOption.Visible);
            x.ShouldShowLLOQ = SnapshotValueFor(curveOptions.ShouldShowLLOQ, _defaultCurveOption.ShouldShowLLOQ);
            x.VisibleInLegend = SnapshotValueFor(curveOptions.VisibleInLegend, _defaultCurveOption.VisibleInLegend);
            x.InterpolationMode = SnapshotValueFor(curveOptions.InterpolationMode, _defaultCurveOption.InterpolationMode);
            x.Color = SnapshotValueFor(curveOptions.Color, _defaultCurveOption.Color);
            x.LegendIndex = curveOptions.LegendIndex;
            x.LineThickness = SnapshotValueFor(curveOptions.LineThickness, _defaultCurveOption.LineThickness);
            x.LineStyle = SnapshotValueFor(curveOptions.LineStyle, _defaultCurveOption.LineStyle);
            x.Symbol = SnapshotValueFor(curveOptions.Symbol, _defaultCurveOption.Symbol);
            x.yAxisType = SnapshotValueFor(curveOptions.yAxisType, _defaultCurveOption.yAxisType);
         });
      }

      public override Task<Core.Chart.CurveOptions> MapToModel(CurveOptions snapshot, SnapshotContext snapshotContext)
      {
         return Task.FromResult(new Core.Chart.CurveOptions
         {
            Visible = ModelValueFor(snapshot.Visible, _defaultCurveOption.Visible),
            ShouldShowLLOQ = ModelValueFor(snapshot.ShouldShowLLOQ, _defaultCurveOption.ShouldShowLLOQ),
            VisibleInLegend = ModelValueFor(snapshot.VisibleInLegend, _defaultCurveOption.VisibleInLegend),
            InterpolationMode = ModelValueFor(snapshot.InterpolationMode, _defaultCurveOption.InterpolationMode),
            Color = ModelValueFor(snapshot.Color, _defaultCurveOption.Color),
            LegendIndex = snapshot.LegendIndex,
            LineThickness = ModelValueFor(snapshot.LineThickness, _defaultCurveOption.LineThickness),
            LineStyle = ModelValueFor(snapshot.LineStyle, _defaultCurveOption.LineStyle),
            Symbol = ModelValueFor(snapshot.Symbol, _defaultCurveOption.Symbol),
            yAxisType = ModelValueFor(snapshot.yAxisType, _defaultCurveOption.yAxisType),
         });
      }
   }
}