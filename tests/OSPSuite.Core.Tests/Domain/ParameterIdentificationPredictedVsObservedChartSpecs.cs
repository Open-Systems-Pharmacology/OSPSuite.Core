using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Chart.ParameterIdentifications;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_ParameterIdentificationPredictedVsObservedChart : ContextSpecification<ParameterIdentificationPredictedVsObservedChart>
   {
      protected override void Context()
      {
         sut = new ParameterIdentificationPredictedVsObservedChart().WithAxes();
         sut.AddNewAxis();
         sut.AddNewAxis();
      }
   }

   public class When_updating_axis_visibility : concern_for_ParameterIdentificationPredictedVsObservedChart
   {
      protected override void Context()
      {
         base.Context();
         sut.YAxis.Dimension = DomainHelperForSpecs.ConcentrationDimensionForSpecs();
         sut.AxisBy(AxisTypes.Y2).Dimension = DomainHelperForSpecs.FractionDimensionForSpecs();
         sut.AxisBy(AxisTypes.Y3).Dimension = DomainHelperForSpecs.LengthDimensionForSpecs();
         sut.XAxis.Dimension = sut.AxisBy(AxisTypes.Y3).Dimension;
      }

      protected override void Because()
      {
         sut.UpdateAxesVisibility();
      }

      [Observation]
      public void the_x_axis_should_be_visible()
      {
         sut.XAxis.Visible.ShouldBeTrue();
      }

      [Observation]
      public void the_y_axes_with_same_dimension_is_visible()
      {
         sut.AxisBy(AxisTypes.Y3).Visible.ShouldBeTrue();
      }

      [Observation]
      public void axes_with_other_dimensions_are_not_visible()
      {
         sut.AxisBy(AxisTypes.Y2).Visible.ShouldBeFalse();
         sut.YAxis.Visible.ShouldBeFalse();
      }
   }

   public class When_cloning_a_predicted_vs_observed_chart_with_identity_and_deviation_curves : concern_for_ParameterIdentificationPredictedVsObservedChart
   {
      private static readonly Color CALCULATION_COLOR = Color.Magenta;
      private const string X_AXIS_CAPTION = "Observed";
      private ICloneManager _cloneManager;
      private DataRepository _observedData;
      private DataRepository _simulationResults;
      private DataColumn _observationColumn;
      private DataColumn _calculationColumn;
      private ParameterIdentificationPredictedVsObservedChart _clone;

      protected override void Context()
      {
         base.Context();
         var dimensionFactory = DimensionFactoryForSpecs.Factory;
         var objectBaseFactory = new ObjectBaseFactory(A.Fake<Utility.Container.IContainer>(), dimensionFactory, A.Fake<IIdGenerator>(), A.Fake<ICreationMetaDataFactory>());
         _cloneManager = new CloneManagerForModel(objectBaseFactory, new DataRepositoryTask(), A.Fake<IModelFinalizer>());
         var predictedVsObservedChartService = new PredictedVsObservedChartService(dimensionFactory, A.Fake<IDisplayUnitRetriever>());

         _observedData = DomainHelperForSpecs.ObservedData("OBS");
         _observationColumn = _observedData.FirstDataColumn();
         _simulationResults = DomainHelperForSpecs.IndividualSimulationDataRepositoryFor("Sim");
         _calculationColumn = _simulationResults.FirstDataColumn();
         var observationColumns = new List<DataColumn> { _observationColumn };

         //curves as created by the presenter when displaying the chart
         predictedVsObservedChartService.AddCurvesFor(observationColumns, _calculationColumn, sut, (column, curve) => curve.Color = CALCULATION_COLOR);
         predictedVsObservedChartService.AddIdentityCurves(observationColumns, sut);
         predictedVsObservedChartService.AddDeviationLine(2, observationColumns, sut);
         sut.XAxis.Caption = X_AXIS_CAPTION;
      }

      protected override void Because()
      {
         _clone = _cloneManager.Clone(sut);
      }

      [Observation]
      public void should_not_copy_identity_and_deviation_curves()
      {
         sut.Curves.Count(x => x.yData.IsDeviation()).ShouldBeEqualTo(3);
         _clone.Curves.Any(x => x.yData.IsDeviation()).ShouldBeFalse();
      }

      [Observation]
      public void should_keep_the_calculation_curve_and_the_deviation_fold_values()
      {
         var clonedCurve = _clone.Curves.Single();
         clonedCurve.xData.ShouldBeEqualTo(_observationColumn);
         clonedCurve.yData.ShouldBeEqualTo(_calculationColumn);
         clonedCurve.Color.ShouldBeEqualTo(CALCULATION_COLOR);
         _clone.DeviationFoldValues.ShouldOnlyContain(2f);
      }

      [Observation]
      public void should_keep_the_axes_settings()
      {
         _clone.XAxis.Caption.ShouldBeEqualTo(X_AXIS_CAPTION);
         _clone.XAxis.Dimension.ShouldBeEqualTo(sut.XAxis.Dimension);
         _clone.YAxis.Dimension.ShouldBeEqualTo(sut.YAxis.Dimension);
         _clone.YAxis.UnitName.ShouldBeEqualTo(sut.YAxis.UnitName);
      }
   }
}
