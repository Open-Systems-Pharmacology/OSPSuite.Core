﻿using System.Collections.Generic;
using System.Data;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Domain.PKAnalyses;
using OSPSuite.Core.Domain.SensitivityAnalyses;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.Services.SensitivityAnalyses;
using OSPSuite.Core.Events;
using OSPSuite.Helpers;
using OSPSuite.Utility.Events;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_SensitivityAnalysisEngine : ContextSpecification<ISensitivityAnalysisEngine>
   {
      protected IEventPublisher _eventPublisher;
      protected ISensitivityAnalysisVariationDataCreator _sensitivityAnalysisVariationDataCreator;
      protected IPopulationRunner _populationRunner;
      protected ISimulationToModelCoreSimulationMapper _modelCoreSimulationMapper;
      protected ISensitivityAnalysisRunResultCalculator _runResultCalculator;
      protected ISimulationPersistableUpdater _simulationPersistableUpdater;
      protected SensitivityAnalysis _sensitivityAnalysis;
      protected IModelCoreSimulation _modelCoreSimulation;

      protected override void Context()
      {
         _eventPublisher = A.Fake<IEventPublisher>();
         _sensitivityAnalysisVariationDataCreator = A.Fake<ISensitivityAnalysisVariationDataCreator>();
         _populationRunner = A.Fake<IPopulationRunner>();
         _modelCoreSimulationMapper = A.Fake<ISimulationToModelCoreSimulationMapper>();
         _runResultCalculator = A.Fake<ISensitivityAnalysisRunResultCalculator>();
         _simulationPersistableUpdater = A.Fake<ISimulationPersistableUpdater>();
         sut = new SensitivityAnalysisEngine(_eventPublisher, _sensitivityAnalysisVariationDataCreator, _populationRunner, _modelCoreSimulationMapper, _runResultCalculator, _simulationPersistableUpdater);

         _sensitivityAnalysis = A.Fake<SensitivityAnalysis>();
         _modelCoreSimulation = A.Fake<IModelCoreSimulation>();
         A.CallTo(() => _modelCoreSimulationMapper.MapFrom(_sensitivityAnalysis.Simulation, true)).Returns(_modelCoreSimulation);
      }
   }

   public class When_running_a_sensitivity_analysis : concern_for_SensitivityAnalysisEngine
   {
      private List<SensitivityAnalysisEvent> _allEvents;
      private PopulationRunResults _populationRunResult;
      private VariationData _variationData;
      private SensitivityAnalysisRunResult _sensitivityAnalysisResults;
      private DataTable _dataTable;
      private SensitivityAnalysisRunOptions _runOptions;

      protected override void Context()
      {
         base.Context();
         _runOptions = new SensitivityAnalysisRunOptions();
         _allEvents = new List<SensitivityAnalysisEvent>();
         A.CallTo(() => _eventPublisher.PublishEvent(A<SensitivityAnalysisStartedEvent>._))
            .Invokes(x => _allEvents.Add(x.GetArgument<SensitivityAnalysisStartedEvent>(0)));

         A.CallTo(() => _eventPublisher.PublishEvent(A<SensitivityAnalysisTerminatedEvent>._))
            .Invokes(x => _allEvents.Add(x.GetArgument<SensitivityAnalysisTerminatedEvent>(0)));

         A.CallTo(() => _eventPublisher.PublishEvent(A<SensitivityAnalysisResultsUpdatedEvent>._))
            .Invokes(x => _allEvents.Add(x.GetArgument<SensitivityAnalysisResultsUpdatedEvent>(0)));

         _variationData= A.Fake<VariationData>();
         _dataTable=new DataTable();
         A.CallTo(() => _variationData.ToDataTable()).Returns(_dataTable);
         A.CallTo(() => _sensitivityAnalysisVariationDataCreator.CreateForRun(_sensitivityAnalysis)).Returns(_variationData);
         _populationRunResult = new PopulationRunResults {Results = new SimulationResults()};
         A.CallTo(() => _populationRunner.RunPopulationAsync(_modelCoreSimulation, _runOptions,  _dataTable, null, null)).ReturnsAsync(_populationRunResult);

         _sensitivityAnalysisResults=new SensitivityAnalysisRunResult();
         A.CallTo(() => _runResultCalculator.CreateFor(_sensitivityAnalysis, _variationData, _populationRunResult.Results, A<IReadOnlyList<IndividualRunInfo>>._, false)).Returns(_sensitivityAnalysisResults);

      }

      protected override void Because()
      {
         sut.StartAsync(_sensitivityAnalysis, _runOptions).Wait();
      }

      [Observation]
      public void should_notify_the_sensitivity_analysis_started_event_and_terminated_event()
      {
         _allEvents.Count.ShouldBeEqualTo(3);
         _allEvents[0].ShouldBeAnInstanceOf<SensitivityAnalysisStartedEvent>();
         _allEvents[1].ShouldBeAnInstanceOf<SensitivityAnalysisResultsUpdatedEvent>();
         _allEvents[2].ShouldBeAnInstanceOf<SensitivityAnalysisTerminatedEvent>();
      }

      [Observation]
      public void should_update_the_persistable_in_the_clone_of_the_simulation_to_ensure_that_all_parameters_are_also_available_for_calculation()
      {
         A.CallTo(() => _simulationPersistableUpdater.UpdateSimulationPersistable(_modelCoreSimulation)).MustHaveHappened();
      }

      [Observation]
      public void should_update_the_results()
      {
         _sensitivityAnalysis.Results.ShouldBeEqualTo(_sensitivityAnalysisResults);
      }
   }
}