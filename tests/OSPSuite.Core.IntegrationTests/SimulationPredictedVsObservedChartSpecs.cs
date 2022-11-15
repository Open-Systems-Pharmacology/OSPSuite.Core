using System.Collections.Generic;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart.Simulations;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Utility.Container;

namespace OSPSuite.Core
{
   public abstract class concern_for_SimulationPredictedVsObservedChart : ContextForIntegration<SimulationPredictedVsObservedChart>
   {
      protected override void Context()
      {
         sut = new SimulationPredictedVsObservedChart();
      }
   }

   public class When_counting_the_deviations_curves : concern_for_SimulationPredictedVsObservedChart
   {
      private readonly List<DataRepository> _deviationCurves = new List<DataRepository>();

      protected override void Context()
      {
         base.Context();
         _deviationCurves.Add(new NullDataRepository());
         sut.AddToDeviationFoldValue(1);
         sut.AddToDeviationFoldValue(2);
         sut.AddToDeviationFoldValue(3);
         sut.AddToDeviationFoldValue(4);
         sut.AddToDeviationFoldValue(5);
         var dimensionFactory = IoC.Resolve<IDimensionFactory>();
         sut.AddDeviationCurvesForFoldValue(1, dimensionFactory, _deviationCurves, (column, curve) => { });
         sut.AddDeviationCurvesForFoldValue(2, dimensionFactory, _deviationCurves, (column, curve) => { });
         sut.AddDeviationCurvesForFoldValue(3, dimensionFactory, _deviationCurves, (column, curve) => { });
      }

      [Observation]
      public void the_count_should_only_include_plotted_folds()
      {
         sut.PlottedFolds().Count().ShouldBeEqualTo(3);
      }
   }

   public class When_updating_properties_from_source_chart : concern_for_SimulationPredictedVsObservedChart
   {
      private ICloneManager _cloneManager;
      private SimulationPredictedVsObservedChart _sourceChart;

      protected override void Context()
      {
         base.Context();
         _cloneManager = IoC.Resolve<ICloneManagerForModel>();
         _sourceChart = new SimulationPredictedVsObservedChart
         {
            Description = "The Description",
            Icon = "The Icon",
            Name = "The Name",
            PreviewSettings = true,
            Title = "The Title"
         };
         _sourceChart.AddToDeviationFoldValue(2);
      }

      protected override void Because()
      {
         sut.UpdatePropertiesFrom(_sourceChart, _cloneManager);
      }

      [Observation]
      public void the_properties_should_be_updated_in_the_target_chart()
      {
         sut.Name.ShouldBeEqualTo(_sourceChart.Name);
         sut.Icon.ShouldBeEqualTo(_sourceChart.Icon);
         sut.Description.ShouldBeEqualTo(_sourceChart.Description);
         sut.PreviewSettings.ShouldBeEqualTo(_sourceChart.PreviewSettings);
         sut.Title.ShouldBeEqualTo(_sourceChart.Title);
         sut.DeviationFoldValues.ShouldOnlyContain(_sourceChart.DeviationFoldValues);
      }

      [Observation]
      public void the_count_of_plotted_deviations_should_not_include_unplotted_folds()
      {
         sut.PlottedFolds().Count().ShouldBeEqualTo(0);
      }

      [Observation]
      public void the_updated_chart_should_not_indicate_it_has_curves_for_the_folds()
      {
         sut.DeviationFoldValues.ShouldContain(2);
         sut.DeviationFoldValues.Any(foldValue => sut.HasDeviationCurveFor(foldValue)).ShouldBeFalse();
      }
   }
}