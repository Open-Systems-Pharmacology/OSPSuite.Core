using System.Drawing;
using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Chart.ParameterIdentifications;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Helpers;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_ParameterIdentificationAnalysisCreator : ContextSpecification<ParameterIdentificationAnalysisCreator>
   {
      protected const string CLONED_ID = "CLONED_ID";
      protected static readonly Color CALCULATION_COLOR = Color.Magenta;
      protected IOSPSuiteExecutionContext _executionContext;
      protected DataColumn _calculationColumn;
      private IIdGenerator _idGenerator;
      private IDimensionFactory _dimensionFactory;

      protected override void Context()
      {
         _executionContext = A.Fake<IOSPSuiteExecutionContext>();
         _idGenerator = A.Fake<IIdGenerator>();
         A.CallTo(() => _idGenerator.NewId()).Returns(CLONED_ID);
         _dimensionFactory = A.Fake<IDimensionFactory>();

         var objectBaseFactory = new ObjectBaseFactory(A.Fake<Utility.Container.IContainer>(), _dimensionFactory, _idGenerator, A.Fake<ICreationMetaDataFactory>());
         var cloneManager = new CloneManagerForModel(objectBaseFactory, new DataRepositoryTask(), A.Fake<IModelFinalizer>());

         sut = new ParameterIdentificationAnalysisCreator(A.Fake<IChartFactory>(), _executionContext, A.Fake<IContainerTask>(), _idGenerator, A.Fake<IObjectIdResetter>(), cloneManager);

         //column of a run result: only available through the parameter identification, not in the serialization context
         var baseGrid = new BaseGrid("Time", DomainHelperForSpecs.TimeDimensionForSpecs());
         _calculationColumn = new DataColumn("Concentration", DomainHelperForSpecs.NoDimension(), baseGrid)
         {
            QuantityInfo = new QuantityInfo(new[] { "Sim", "Liver", "Cell", "Concentration" }, QuantityType.Drug)
         };
      }

      protected T SourceChartWithCalculationCurve<T>() where T : AnalysisChart, new()
      {
         var chart = new T().WithId("SOURCE_ID").WithName("Source");
         var curve = new Curve { Name = "Calculation" };
         curve.SetxData(_calculationColumn.BaseGrid, _dimensionFactory);
         curve.SetyData(_calculationColumn, _dimensionFactory);
         curve.Color = CALCULATION_COLOR;
         curve.LineStyle = LineStyles.Dash;
         chart.AddCurve(curve, useAxisDefault: false);
         return chart;
      }
   }

   public class When_creating_an_analysis_based_on_a_parameter_identification_time_profile_chart : concern_for_ParameterIdentificationAnalysisCreator
   {
      private ParameterIdentificationTimeProfileChart _sourceChart;
      private ISimulationAnalysis _result;

      protected override void Context()
      {
         base.Context();
         _sourceChart = SourceChartWithCalculationCurve<ParameterIdentificationTimeProfileChart>();
      }

      protected override void Because()
      {
         _result = sut.CreateAnalysisBasedOn(_sourceChart);
      }

      [Observation]
      public void should_return_a_new_time_profile_chart_with_a_new_id()
      {
         _result.ShouldBeAnInstanceOf<ParameterIdentificationTimeProfileChart>();
         _result.ShouldNotBeEqualTo(_sourceChart);
         _result.Id.ShouldBeEqualTo(CLONED_ID);
      }

      [Observation]
      public void should_keep_the_calculation_curves_with_their_color_and_style()
      {
         var clonedCurve = _result.DowncastTo<CurveChart>().Curves.Single();
         clonedCurve.yData.ShouldBeEqualTo(_calculationColumn);
         clonedCurve.Color.ShouldBeEqualTo(CALCULATION_COLOR);
         clonedCurve.LineStyle.ShouldBeEqualTo(LineStyles.Dash);
      }

      [Observation]
      public void should_not_clone_the_chart_through_serialization()
      {
         A.CallTo(() => _executionContext.Serialize(A<ISimulationAnalysis>._)).MustNotHaveHappened();
      }
   }

   public class When_creating_an_analysis_based_on_a_parameter_identification_predicted_vs_observed_chart : concern_for_ParameterIdentificationAnalysisCreator
   {
      private ParameterIdentificationPredictedVsObservedChart _sourceChart;
      private ISimulationAnalysis _result;

      protected override void Context()
      {
         base.Context();
         _sourceChart = SourceChartWithCalculationCurve<ParameterIdentificationPredictedVsObservedChart>();
         _sourceChart.AddToDeviationFoldValue(2);
      }

      protected override void Because()
      {
         _result = sut.CreateAnalysisBasedOn(_sourceChart);
      }

      [Observation]
      public void should_keep_the_calculation_curves_and_the_deviation_fold_values()
      {
         var clonedChart = _result.DowncastTo<ParameterIdentificationPredictedVsObservedChart>();
         clonedChart.Curves.Single().Color.ShouldBeEqualTo(CALCULATION_COLOR);
         clonedChart.DeviationFoldValues.ShouldOnlyContain(2f);
      }
   }

   public class When_creating_an_analysis_based_on_a_parameter_identification_chart_with_local_repositories : concern_for_ParameterIdentificationAnalysisCreator
   {
      private ParameterIdentificationResidualVsTimeChart _sourceChart;
      private ParameterIdentificationResidualVsTimeChart _deserializedChart;
      private ISimulationAnalysis _result;

      protected override void Context()
      {
         base.Context();
         _sourceChart = new ParameterIdentificationResidualVsTimeChart();
         _deserializedChart = new ParameterIdentificationResidualVsTimeChart();
         var serializedBytes = new byte[] { 1 };
         A.CallTo(() => _executionContext.Serialize<ISimulationAnalysis>(_sourceChart)).Returns(serializedBytes);
         A.CallTo(() => _executionContext.Deserialize<ISimulationAnalysis>(serializedBytes)).Returns(_deserializedChart);
      }

      protected override void Because()
      {
         _result = sut.CreateAnalysisBasedOn(_sourceChart);
      }

      [Observation]
      public void should_clone_the_chart_through_serialization()
      {
         _result.ShouldBeEqualTo(_deserializedChart);
      }
   }
}
