using OSPSuite.BDDHelper;
using FakeItEasy;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Events;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Views.ParameterIdentifications;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_ParameterIdentificationDataSelectionPresenter : ContextSpecification<IParameterIdentificationDataSelectionPresenter>
   {
      protected IParameterIdentificationSimulationSelectionPresenter _simulationSelectionPresenter;
      private IParameterIdentificationDataSelectionView _view;
      protected ParameterIdentification _parameterIdentification;
      protected ISimulation _simulation;
      protected IParameterIdentificationOutputMappingPresenter _outputMappingPresenter;
      protected IParameterIdentificationWeightedObservedDataCollectorPresenter _weightedObservedDataCollectorPresenter;

      protected override void Context()
      {
         _view = A.Fake<IParameterIdentificationDataSelectionView>();
         _simulationSelectionPresenter = A.Fake<IParameterIdentificationSimulationSelectionPresenter>();
         _outputMappingPresenter = A.Fake<IParameterIdentificationOutputMappingPresenter>();
         _weightedObservedDataCollectorPresenter= A.Fake<IParameterIdentificationWeightedObservedDataCollectorPresenter>();
         sut = new ParameterIdentificationDataSelectionPresenter(_view, _simulationSelectionPresenter, _outputMappingPresenter, _weightedObservedDataCollectorPresenter);

         _simulation = A.Fake<ISimulation>().WithId("Sim");
         _parameterIdentification = new ParameterIdentification();
         _parameterIdentification.AddSimulation(_simulation);

         sut.EditParameterIdentification(_parameterIdentification);
      }
   }

   public class When_the_parameter_identification_data_selection_presenter_is_editing_a_parameter_identification : concern_for_ParameterIdentificationDataSelectionPresenter
   {
      [Observation]
      public void should_initialize_the_simulation_selection_presenter_with_the_parameter_identification()
      {
         A.CallTo(() => _simulationSelectionPresenter.EditParameterIdentification(_parameterIdentification)).MustHaveHappened();
      }

      [Observation]
      public void should_initialize_the_output_mapping_presenter_with_the_parameter_identification()
      {
         A.CallTo(() => _outputMappingPresenter.EditParameterIdentification(_parameterIdentification)).MustHaveHappened();
      }

      [Observation]
      public void should_initialize_the_weighted_observed_data_collector_presenter_with_the_parameter_identification()
      {
         A.CallTo(() => _weightedObservedDataCollectorPresenter.EditParameterIdentification(_parameterIdentification)).MustHaveHappened();
      }
   }

   public class When_the_parameter_identification_data_presenter_is_being_notified_that_a_simulation_was_added_to_the_parameter_identification : concern_for_ParameterIdentificationDataSelectionPresenter
   {
      protected override void Because()
      {
         _simulationSelectionPresenter.SimulationAdded += Raise.With(new SimulationEventArgs(A.Fake<ISimulation>()));
      }

      [Observation]
      public void should_refresh_the_output_mapping()
      {
         A.CallTo(() => _outputMappingPresenter.Refresh()).MustHaveHappened();
      }

      [Observation]
      public void should_refresh_the_simulation_selection_presenter()
      {
         A.CallTo(() => _weightedObservedDataCollectorPresenter.Refresh()).MustHaveHappened();
      }
   }

   public class When_the_parameter_identification_data_presenter_is_notififed_that_a_weighted_observed_data_was_selected : concern_for_ParameterIdentificationDataSelectionPresenter
   {
      private WeightedObservedData _weightedObservedData;

      protected override void Context()
      {
         base.Context();
         _weightedObservedData = A.Fake<WeightedObservedData>();
      }

      protected override void Because()
      {
         _outputMappingPresenter.ObservedDataSelected += Raise.With(new ObservedDataEventArgs(_weightedObservedData));
      }

      [Observation]
      public void should_select_the_corresponding_observed_data()
      {
         A.CallTo(() => _weightedObservedDataCollectorPresenter.SelectObservedData(_weightedObservedData)).MustHaveHappened();
      }
   }

   public class When_the_parameter_identification_data_selection_presenter_is_notified_that_a_simulation_was_replaced : concern_for_ParameterIdentificationDataSelectionPresenter
   {
      protected override void Because()
      {
         sut.Handle(new SimulationReplacedInParameterAnalyzableEvent(_parameterIdentification, _simulation, A.Fake<ISimulation>()));
      }

      [Observation]
      public void should_refresh_the_observed_data_presenter()
      {
         A.CallTo(() => _weightedObservedDataCollectorPresenter.Refresh()).MustHaveHappened();
      }

      [Observation]
      public void should_refresh_the_simulation_selection_presenter()
      {
         A.CallTo(() => _simulationSelectionPresenter.Refresh()).MustHaveHappened();
      }

      [Observation]
      public void should_refresh_the_output_mapping_presenter()
      {
         A.CallTo(() => _outputMappingPresenter.Refresh()).MustHaveHappened();
      }
   }

   public class When_the_parameter_identification_data_selection_presenter_is_notified_that_a_simulation_was_removed : concern_for_ParameterIdentificationDataSelectionPresenter
   {
      protected override void Because()
      {
         sut.Handle(new SimulationRemovedEvent(_simulation));
      }

      [Observation]
      public void should_refresh_the_observed_data_presenter()
      {
         A.CallTo(() => _weightedObservedDataCollectorPresenter.Refresh()).MustHaveHappened();
      }

      [Observation]
      public void should_refresh_the_simulation_selection_presenter()
      {
         A.CallTo(() => _simulationSelectionPresenter.Refresh()).MustHaveHappened();
      }

      [Observation]
      public void should_refresh_the_output_mapping_presenter()
      {
         A.CallTo(() => _outputMappingPresenter.Refresh()).MustHaveHappened();
      }
   }

   public class When_the_parameter_identification_data_selection_presenter_is_notified_that_a_simulation_was_renamed : concern_for_ParameterIdentificationDataSelectionPresenter
   {
      protected override void Because()
      {
         base.Because();
         sut.Handle(new RenamedEvent(_simulation));
      }

      [Observation]
      public void should_refresh_the_simulation_selection_presenter()
      {
         A.CallTo(() => _simulationSelectionPresenter.Refresh()).MustHaveHappened();
      }

      [Observation]
      public void should_refresh_the_output_mapping_presenter()
      {
         A.CallTo(() => _outputMappingPresenter.Refresh()).MustHaveHappened();
      }
   }

   public class When_the_parameter_identification_data_selection_presenter_is_notified_that_some_observed_data_were_added_to_a_simulation_used_in_the_edited_parameter_identification : concern_for_ParameterIdentificationDataSelectionPresenter
   {
      protected override void Because()
      {
         sut.Handle(new ObservedDataAddedToAnalysableEvent(_simulation, new DataRepository(), true));
      }

      [Observation]
      public void should_refresh_the_simulation_selection_presenter()
      {
         A.CallTo(() => _simulationSelectionPresenter.Refresh()).MustHaveHappened();
      }

      [Observation]
      public void should_refresh_the_output_mapping_presenter()
      {
         A.CallTo(() => _outputMappingPresenter.UpdateCache()).MustHaveHappened();
      }
   }

   public class When_the_parameter_identification_data_selection_presenter_is_notified_that_some_observed_data_were_added_to_a_simulation_not_used_in_the_edited_parameter_identification : concern_for_ParameterIdentificationDataSelectionPresenter
   {
      protected override void Because()
      {
         sut.Handle(new ObservedDataAddedToAnalysableEvent(A.Fake<IAnalysable>(), new DataRepository(), true));
      }

      [Observation]
      public void should_not_refresh_the_simulation_selection_presenter()
      {
         A.CallTo(() => _simulationSelectionPresenter.Refresh()).MustNotHaveHappened();
      }

      [Observation]
      public void should_not_refresh_the_output_mapping_presenter()
      {
         A.CallTo(() => _outputMappingPresenter.UpdateCache()).MustNotHaveHappened();
      }
   }
}