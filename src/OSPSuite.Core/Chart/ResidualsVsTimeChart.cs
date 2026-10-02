using System;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;

namespace OSPSuite.Core.Chart
{
   public abstract class ResidualsVsTimeChart : AnalysisChartWithLocalRepositories
   {
      public const string ZERO = "Zero";

      public override CurveChartTypes CurveChartType => CurveChartTypes.ResidualVsTime;

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

      protected override DataRepository CloneRepository(DataRepository sourceRepository, AnalysisChartWithLocalRepositories sourceChart, ICloneManager cloneManager)
      {
         var clonedRepository = base.CloneRepository(sourceRepository, sourceChart, cloneManager);
         //scatter repositories are found by an id starting with the chart id (see ResidualsVsTimeChartService.GetOrCreateScatterDataRepositoryInChart)
         var sourceIdPrefix = $"{sourceChart.Id}-";
         if (sourceRepository.Id.StartsWith(sourceIdPrefix, StringComparison.Ordinal))
            clonedRepository.Id = $"{Id}-{sourceRepository.Id.Substring(sourceIdPrefix.Length)}";

         return clonedRepository;
      }

      //the zero marker is recreated by the presenter each time the chart is displayed
      protected override bool ShouldCloneCurve(Curve curve) => !string.Equals(curve.yData.Name, ZERO);
   }
}