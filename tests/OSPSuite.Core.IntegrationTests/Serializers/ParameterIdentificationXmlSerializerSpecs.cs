using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Chart.ParameterIdentifications;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Serialization;
using OSPSuite.Core.Services;
using OSPSuite.Helpers;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace OSPSuite.Core.Serializers
{
   public abstract class concern_for_ParameterIdentificationXmlSerializer : ModelingXmlSerializerBaseSpecs
   {
      protected const string OUTPUT_PATH = "Sim|Organism|PeripheralVenousBlood|Drug|Plasma (Peripheral Venous Blood)";
      protected const int RUN_INDEX = 1;
      protected IIdGenerator _idGenerator;
      protected DataRepository _observedData;
      protected ParameterIdentificationAnalysisCreator _analysisCreator;
      private IContainer _container;

      protected override void Context()
      {
         base.Context();
         _idGenerator = IoC.Resolve<IIdGenerator>();
         _observedData = DomainHelperForSpecs.ObservedData("OBS", DimensionTime, DimensionMolarConcentration);

         //loading a parameter identification resolves the lazy load task of the application, only used for referenced simulations
         _container = A.Fake<IContainer>(x => x.Wrapping(IoC.Container));
         A.CallTo(() => _container.Resolve<ILazyLoadTask>()).Returns(A.Fake<ILazyLoadTask>());

         //as in the applications, the execution context copies an analysis with the xml serializers
         var executionContext = A.Fake<IOSPSuiteExecutionContext>();
         A.CallTo(() => executionContext.Serialize(A<ISimulationAnalysis>._)).ReturnsLazily((ISimulationAnalysis analysis) => Encoding.UTF8.GetBytes(Serialize(analysis).ToString()));
         A.CallTo(() => executionContext.Deserialize<ISimulationAnalysis>(A<byte[]>._)).ReturnsLazily((byte[] bytes) => Deserialize<ISimulationAnalysis>(XElement.Parse(Encoding.UTF8.GetString(bytes))));

         _analysisCreator = new ParameterIdentificationAnalysisCreator(A.Fake<IChartFactory>(), executionContext, IoC.Resolve<IContainerTask>(), _idGenerator, new ObjectIdResetter(_idGenerator), IoC.Resolve<ICloneManagerForModel>());
      }

      protected XElement Serialize(object objectToSerialize)
      {
         using (var serializationContext = SerializationTransaction.Create(_container))
         {
            return SerializerRepository.SerializerFor(objectToSerialize).Serialize(objectToSerialize, serializationContext);
         }
      }

      /// <summary>
      ///    Deserializes the <paramref name="element" /> in a new context, as the applications do when loading an object. The
      ///    observed data is available in the context, like the observed data of a project.
      /// </summary>
      protected T Deserialize<T>(XElement element)
      {
         var withIdRepository = new WithIdRepository();
         withIdRepository.Register(_observedData);
         using (var deserializationContext = SerializationTransaction.Create(_container, IoC.Resolve<IDimensionFactory>(), IoC.Resolve<IObjectBaseFactory>(), withIdRepository, IoC.Resolve<ICloneManagerForModel>(), new[] { _observedData }))
         {
            try
            {
               return SerializerRepository.SerializerFor(element).Deserialize<T>(element, deserializationContext);
            }
            catch (Exception)
            {
               //as in the applications, references are not resolved when the deserialization failed
               deserializationContext.SkipResolveStep = true;
               throw;
            }
         }
      }

      /// <summary>
      ///    Residual vs time chart as displayed by the residual presenter
      /// </summary>
      protected ParameterIdentificationResidualVsTimeChart ResidualVsTimeChart()
      {
         var chart = new ParameterIdentificationResidualVsTimeChart().WithId(_idGenerator.NewId()).WithName("Residuals vs. Time");
         ShowResidualsIn(chart);
         new ResidualsVsTimeChartService(IoC.Resolve<IDimensionFactory>()).AddZeroMarkerCurveToChart(chart, 1, 3);
         return chart;
      }

      /// <summary>
      ///    Adds the scatter repository and curve of the residuals to the <paramref name="chart" /> unless the chart already
      ///    has them, as the residual presenter does when displaying the chart
      /// </summary>
      protected void ShowResidualsIn(AnalysisChartWithLocalRepositories chart)
      {
         var residualsVsTimeChartService = new ResidualsVsTimeChartService(IoC.Resolve<IDimensionFactory>());
         var outputResiduals = new OutputResiduals(OUTPUT_PATH, _observedData, new[] { new Residual(1, 0.1, 1), new Residual(2, -0.2, 1), new Residual(3, 0.05, 1) });
         var scatterRepository = residualsVsTimeChartService.GetOrCreateScatterDataRepositoryInChart(chart, outputResiduals, RUN_INDEX);
         residualsVsTimeChartService.AddCurvesFor(scatterRepository, (column, curve) => { }, chart);
      }

      protected string ScatterRepositoryIdFor(ISimulationAnalysis chart) => $"{chart.Id}-{OUTPUT_PATH}-{_observedData.Id}-{RUN_INDEX}";

      /// <summary>
      ///    Confidence interval chart as displayed by the confidence interval presenter, with a curve for the observed data
      /// </summary>
      protected ParameterIdentificationTimeProfileConfidenceIntervalChart ConfidenceIntervalChart()
      {
         var chart = new ParameterIdentificationTimeProfileConfidenceIntervalChart().WithId(_idGenerator.NewId()).WithName("Time Profile Confidence Interval");
         var simulationBaseGrid = new BaseGrid("Time", DimensionTime) { Values = new[] { 1f, 2f, 3f } };
         var simulationColumn = new DataColumn("Drug", DimensionMolarConcentration, simulationBaseGrid)
         {
            Values = new[] { 5f, 4f, 3f },
            DataInfo = { Origin = ColumnOrigins.Calculation },
            QuantityInfo = { Path = OUTPUT_PATH.ToPathArray() }
         };
         var simulationResults = new DataRepository { simulationColumn };
         var runResult = new OptimizationRunResult();
         runResult.AddResult(simulationResults);
         var outputMapping = A.Fake<OutputMapping>();
         A.CallTo(() => outputMapping.FullOutputPath).Returns(OUTPUT_PATH);
         A.CallTo(() => outputMapping.Scaling).Returns(Scalings.Linear);

         var intervalRepository = new ConfidenceIntervalDataRepositoryCreator().CreateFor("Confidence Interval", new[] { 0.5, 0.4, 0.3 }, outputMapping, runResult);
         var dimensionFactory = IoC.Resolve<IDimensionFactory>();
         chart.AddRepository(intervalRepository);
         chart.AddCurvesFor(intervalRepository.AllButBaseGrid().Where(x => !x.IsInternal), x => x.Name, dimensionFactory);
         chart.AddCurvesFor(new[] { _observedData.FirstDataColumn() }, x => x.Name, dimensionFactory);
         return chart;
      }

      protected static string[] AllIdsIn(AnalysisChartWithLocalRepositories chart)
      {
         return chart.DataRepositories.Select(x => x.Id)
            .Concat(chart.DataRepositories.SelectMany(x => x.Columns).Select(x => x.Id))
            .ToArray();
      }
   }

   public class When_loading_a_parameter_identification_holding_charts_with_local_repositories_and_their_clones : concern_for_ParameterIdentificationXmlSerializer
   {
      private readonly List<(AnalysisChartWithLocalRepositories source, AnalysisChartWithLocalRepositories clone)> _chartsAndClones = new List<(AnalysisChartWithLocalRepositories, AnalysisChartWithLocalRepositories)>();
      private ParameterIdentification _parameterIdentification;
      private XElement _savedParameterIdentification;
      private ParameterIdentification _loadedParameterIdentification;

      protected override void Context()
      {
         base.Context();
         _chartsAndClones.Clear();
         _parameterIdentification = new ParameterIdentification().WithId(_idGenerator.NewId()).WithName("PI");
         addChartAndItsClone(ResidualVsTimeChart());
         addChartAndItsClone(ConfidenceIntervalChart());
         _savedParameterIdentification = Serialize(_parameterIdentification);
      }

      //as the parameter identification presenter does when the user clones a chart
      private void addChartAndItsClone(AnalysisChartWithLocalRepositories chart)
      {
         _analysisCreator.AddSimulationAnalysisTo(_parameterIdentification, chart);
         var clone = _analysisCreator.CreateAnalysisBasedOn(chart).DowncastTo<AnalysisChartWithLocalRepositories>();
         _analysisCreator.AddSimulationAnalysisTo(_parameterIdentification, clone);
         _chartsAndClones.Add((chart, clone));
      }

      protected override void Because()
      {
         _loadedParameterIdentification = Deserialize<ParameterIdentification>(_savedParameterIdentification);
      }

      private AnalysisChartWithLocalRepositories loaded(AnalysisChartWithLocalRepositories chart)
      {
         return _loadedParameterIdentification.Analyses.FindById(chart.Id).DowncastTo<AnalysisChartWithLocalRepositories>();
      }

      private IEnumerable<AnalysisChartWithLocalRepositories> allLoadedCharts()
      {
         return _chartsAndClones.SelectMany(x => new[] { loaded(x.source), loaded(x.clone) });
      }

      [Observation]
      public void should_load_every_chart_and_its_clone()
      {
         _loadedParameterIdentification.Analyses.Select(x => x.Id).ShouldOnlyContain(_parameterIdentification.Analyses.Select(x => x.Id).ToArray());
      }

      [Observation]
      public void should_keep_the_curves_of_the_clones()
      {
         _chartsAndClones.Each(x => loaded(x.clone).Curves.Count.ShouldBeEqualTo(x.clone.Curves.Count));
      }

      [Observation]
      public void should_plot_every_curve_from_the_repositories_of_its_own_chart_or_from_the_shared_observed_data()
      {
         allLoadedCharts().Each(chart =>
         {
            var columnsOfChart = chart.DataRepositories.SelectMany(x => x.Columns).Concat(_observedData.Columns).ToList();
            chart.Curves.Each(curve =>
            {
               columnsOfChart.ShouldContain(curve.xData);
               columnsOfChart.ShouldContain(curve.yData);
            });
         });
      }

      [Observation]
      public void should_not_share_any_repository_or_column_id_between_a_chart_and_its_clone()
      {
         _chartsAndClones.Each(x => AllIdsIn(loaded(x.clone)).Intersect(AllIdsIn(loaded(x.source))).ShouldBeEmpty());
      }
   }

   public class When_loading_a_parameter_identification_holding_a_clone_of_a_loaded_clone : concern_for_ParameterIdentificationXmlSerializer
   {
      private AnalysisChartWithLocalRepositories _loadedClone;
      private ISimulationAnalysis _cloneOfLoadedClone;
      private XElement _savedParameterIdentification;
      private ParameterIdentification _reloadedParameterIdentification;

      protected override void Context()
      {
         base.Context();
         var parameterIdentification = new ParameterIdentification().WithId(_idGenerator.NewId()).WithName("PI");
         var chart = ResidualVsTimeChart();
         _analysisCreator.AddSimulationAnalysisTo(parameterIdentification, chart);
         var clone = _analysisCreator.CreateAnalysisBasedOn(chart);
         _analysisCreator.AddSimulationAnalysisTo(parameterIdentification, clone);

         var loadedParameterIdentification = Deserialize<ParameterIdentification>(Serialize(parameterIdentification));
         _loadedClone = loadedParameterIdentification.Analyses.FindById(clone.Id).DowncastTo<AnalysisChartWithLocalRepositories>();
         _cloneOfLoadedClone = _analysisCreator.CreateAnalysisBasedOn(_loadedClone);
         _analysisCreator.AddSimulationAnalysisTo(loadedParameterIdentification, _cloneOfLoadedClone);
         _savedParameterIdentification = Serialize(loadedParameterIdentification);
      }

      protected override void Because()
      {
         _reloadedParameterIdentification = Deserialize<ParameterIdentification>(_savedParameterIdentification);
      }

      private AnalysisChartWithLocalRepositories reloaded(ISimulationAnalysis chart)
      {
         return _reloadedParameterIdentification.Analyses.FindById(chart.Id).DowncastTo<AnalysisChartWithLocalRepositories>();
      }

      [Observation]
      public void should_key_the_scatter_repository_of_the_clone_of_the_loaded_clone_with_its_own_id()
      {
         reloaded(_cloneOfLoadedClone).DataRepositories.Single().Id.ShouldBeEqualTo(ScatterRepositoryIdFor(_cloneOfLoadedClone));
      }

      [Observation]
      public void should_not_share_any_repository_or_column_id_between_the_loaded_clone_and_its_clone()
      {
         AllIdsIn(reloaded(_cloneOfLoadedClone)).Intersect(AllIdsIn(reloaded(_loadedClone))).ShouldBeEmpty();
      }
   }
}
