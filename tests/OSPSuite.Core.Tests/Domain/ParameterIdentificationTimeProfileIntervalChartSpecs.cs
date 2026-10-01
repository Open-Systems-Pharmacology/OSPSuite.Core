using System.Drawing;
using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Chart.ParameterIdentifications;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_ParameterIdentificationTimeProfileIntervalChart<TChart> : ContextSpecification<TChart> where TChart : AnalysisChartWithLocalRepositories, new()
   {
      protected const string CLONED_ID = "CLONED_ID";
      protected const string INTERVAL_NAME = "Interval";
      protected static readonly Color INTERVAL_COLOR = Color.Magenta;
      protected ICloneManager _cloneManager;
      protected DataRepository _intervalRepository;
      protected DataColumn _intervalColumn;
      protected DataRepository _observedData;

      protected override void Context()
      {
         var dimensionFactory = DimensionFactoryForSpecs.Factory;
         var idGenerator = A.Fake<IIdGenerator>();
         A.CallTo(() => idGenerator.NewId()).Returns(CLONED_ID);
         var objectBaseFactory = new ObjectBaseFactory(A.Fake<Utility.Container.IContainer>(), dimensionFactory, idGenerator, A.Fake<ICreationMetaDataFactory>());
         _cloneManager = new CloneManagerForModel(objectBaseFactory, new DataRepositoryTask(), A.Fake<IModelFinalizer>());

         //interval repository as created for the confidence, prediction and VPC interval charts
         var outputMapping = A.Fake<OutputMapping>();
         A.CallTo(() => outputMapping.FullOutputPath).Returns("Sim|Comp|Liver|Cell|Concentration");
         outputMapping.Scaling = Scalings.Log;
         var runResult = A.Fake<OptimizationRunResult>();
         var simulationResults = DomainHelperForSpecs.IndividualSimulationDataRepositoryFor("Sim");
         A.CallTo(() => runResult.SimulationResultFor(outputMapping.FullOutputPath)).Returns(simulationResults.FirstDataColumn());
         _intervalRepository = new ConfidenceIntervalDataRepositoryCreator().CreateFor(INTERVAL_NAME, new[] { 1d, 2d, 3d }, outputMapping, runResult);
         _intervalColumn = IntervalColumnIn(_intervalRepository);

         _observedData = DomainHelperForSpecs.ObservedData("OBS");

         sut = new TChart().WithAxes().WithId("SOURCE_ID");
         sut.AddRepository(_intervalRepository);
         var intervalCurve = addCurve(_intervalColumn, dimensionFactory);
         intervalCurve.Color = INTERVAL_COLOR;
         intervalCurve.LineStyle = LineStyles.Dash;
         addCurve(_observedData.FirstDataColumn(), dimensionFactory);
      }

      private Curve addCurve(DataColumn column, IDimensionFactory dimensionFactory)
      {
         var curve = new Curve { Name = column.Name };
         curve.SetxData(column.BaseGrid, dimensionFactory);
         curve.SetyData(column, dimensionFactory);
         sut.AddCurve(curve, useAxisDefault: false);
         return curve;
      }

      protected static DataColumn IntervalColumnIn(DataRepository intervalRepository)
      {
         return intervalRepository.AllButBaseGrid().Single(x => !x.IsInternal);
      }

      protected static string[] AllIdsIn(AnalysisChartWithLocalRepositories chart)
      {
         return chart.DataRepositories.Select(x => x.Id)
            .Concat(chart.DataRepositories.SelectMany(x => x.Columns).Select(x => x.Id))
            .ToArray();
      }
   }

   public abstract class When_cloning_a_parameter_identification_time_profile_interval_chart<TChart> : concern_for_ParameterIdentificationTimeProfileIntervalChart<TChart> where TChart : AnalysisChartWithLocalRepositories, new()
   {
      private TChart _clone;

      protected override void Because()
      {
         _clone = _cloneManager.Clone(sut);
      }

      private DataRepository clonedRepository() => _clone.DataRepositories.Single();

      [Observation]
      public void should_return_a_chart_of_the_same_type_with_a_new_id()
      {
         _clone.ShouldBeAnInstanceOf<TChart>();
         _clone.Id.ShouldBeEqualTo(CLONED_ID);
      }

      [Observation]
      public void should_copy_every_local_repository_with_new_ids()
      {
         clonedRepository().ShouldNotBeEqualTo(_intervalRepository);
         clonedRepository().Columns.Count().ShouldBeEqualTo(_intervalRepository.Columns.Count());
         AllIdsIn(_clone).Intersect(AllIdsIn(sut)).ShouldBeEmpty();
         AllIdsIn(_clone).Distinct().Count().ShouldBeEqualTo(AllIdsIn(_clone).Length);
      }

      [Observation]
      public void should_plot_the_interval_curve_from_the_copied_interval_column_and_base_grid_with_the_source_color_and_style()
      {
         var clonedCurve = _clone.FindCurveWithSameData(clonedRepository().BaseGrid, IntervalColumnIn(clonedRepository()));
         clonedCurve.ShouldNotBeNull();
         clonedCurve.Color.ShouldBeEqualTo(INTERVAL_COLOR);
         clonedCurve.LineStyle.ShouldBeEqualTo(LineStyles.Dash);
      }

      [Observation]
      public void should_relate_the_copied_interval_column_to_the_copied_internal_mean_column()
      {
         var clonedMeanColumn = IntervalColumnIn(clonedRepository()).GetRelatedColumn(AuxiliaryType.GeometricMeanPop);
         clonedMeanColumn.Repository.ShouldBeEqualTo(clonedRepository());
         clonedMeanColumn.IsInternal.ShouldBeTrue();
      }

      [Observation]
      public void should_keep_the_observed_data_curve_on_the_shared_observed_data_columns()
      {
         _clone.FindCurveWithSameData(_observedData.BaseGrid, _observedData.FirstDataColumn()).ShouldNotBeNull();
         _clone.Curves.Count.ShouldBeEqualTo(2);
      }

      [Observation]
      public void should_leave_the_source_chart_unchanged()
      {
         sut.DataRepositories.ShouldOnlyContain(_intervalRepository);
         sut.FindCurveWithSameData(_intervalRepository.BaseGrid, _intervalColumn).ShouldNotBeNull();
      }
   }

   public class When_cloning_a_parameter_identification_time_profile_confidence_interval_chart : When_cloning_a_parameter_identification_time_profile_interval_chart<ParameterIdentificationTimeProfileConfidenceIntervalChart>
   {
   }

   public class When_cloning_a_parameter_identification_time_profile_prediction_interval_chart : When_cloning_a_parameter_identification_time_profile_interval_chart<ParameterIdentificationTimeProfilePredictionIntervalChart>
   {
   }

   public class When_cloning_a_parameter_identification_time_profile_vpc_interval_chart : When_cloning_a_parameter_identification_time_profile_interval_chart<ParameterIdentificationTimeProfileVPCIntervalChart>
   {
   }

   public class When_updating_a_chart_with_local_repositories_from_another_chart : concern_for_ParameterIdentificationTimeProfileIntervalChart<ParameterIdentificationTimeProfileConfidenceIntervalChart>
   {
      private ParameterIdentificationTimeProfileConfidenceIntervalChart _chartToUpdate;
      private DataRepository _previousRepository;

      protected override void Context()
      {
         base.Context();
         _previousRepository = new DataRepository("PREVIOUS_ID");
         _chartToUpdate = new ParameterIdentificationTimeProfileConfidenceIntervalChart();
         _chartToUpdate.AddRepository(_previousRepository);
      }

      protected override void Because()
      {
         _chartToUpdate.UpdatePropertiesFrom(sut, _cloneManager);
      }

      [Observation]
      public void should_replace_its_repositories_with_a_copy_of_each_repository_of_the_other_chart()
      {
         var copiedRepository = _chartToUpdate.DataRepositories.Single();
         copiedRepository.ShouldNotBeEqualTo(_previousRepository);
         copiedRepository.ShouldNotBeEqualTo(_intervalRepository);
         copiedRepository.Name.ShouldBeEqualTo(_intervalRepository.Name);
      }
   }
}
