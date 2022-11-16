using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Chart
{
   public abstract class PredictedVsObservedChart : AnalysisChart
   {
      private readonly Cache<float, IReadOnlyList<DataRepository>> _deviationRepositoryCache = new Cache<float, IReadOnlyList<DataRepository>>(onMissingKey: x => null);
      public List<float> DeviationFoldValues { get; } = new List<float>();

      protected PredictedVsObservedChart()
      {
         ChartSettings.LegendPosition = LegendPositions.BottomInside;
      }

      /// <summary>
      ///    Modifies the Y axes visibility based on X axis dimension. Y axes which do not share a unit with X axis are not shown
      /// </summary>
      public void UpdateAxesVisibility()
      {
         var visibleAxes = Axes.Where(x => x.Dimension != null && x.Dimension.HasSharedUnitNamesWith(XAxis.Dimension)).ToList();
         visibleAxes.Each(axis => axis.Visible = true);
         Axes.Except(visibleAxes).Each(axis => axis.Visible = false);
      }

      public void AddToDeviationFoldValue(float foldValue)
      {
         if (DeviationFoldValues.Contains(foldValue))
            return;

         DeviationFoldValues.Add(foldValue);
      }

      public override void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         base.UpdatePropertiesFrom(source, cloneManager);
         var sourceChart = source as PredictedVsObservedChart;
         if (sourceChart == null)
            return;

         sourceChart.DeviationFoldValues.Each(AddToDeviationFoldValue);
      }

      public bool HasDeviationCurveFor(float foldValue)
      {
         return _deviationRepositoryCache[foldValue] != null;
      }

      public void AddDeviationCurvesForFoldValue(float foldValue, IDimensionFactory dimensionFactory, List<DataRepository> deviationCurves, Action<DataColumn, Curve> action)
      {
         deviationCurves.Where(repository => repository != null).Each(repository => { this.AddCurvesFor(repository, x => x.Name, dimensionFactory, action); });
         _deviationRepositoryCache[foldValue] = deviationCurves;
      }

      public IEnumerable<float> PlottedFolds()
      {
         return DeviationFoldValues.Where(HasDeviationCurveFor);
      }
   }
}