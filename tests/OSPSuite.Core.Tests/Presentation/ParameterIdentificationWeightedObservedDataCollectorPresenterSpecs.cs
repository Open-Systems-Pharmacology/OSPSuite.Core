using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Helpers;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Views.ParameterIdentifications;
using OSPSuite.Utility.Events;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_ParameterIdentificationWeightedObservedDataCollectorPresenter : ContextSpecification<IParameterIdentificationWeightedObservedDataCollectorPresenter>
   {
      protected IEventPublisher _eventPublisher;
      protected IParameterIdentificationWeightedObservedDataCollectorView _view;
      protected IApplicationController _applicationController;
      protected ParameterIdentification _parameterIdentification;
      protected OutputMapping _outputMapping;
      protected WeightedObservedData _weightedObservedData;
      protected IParameterIdentificationWeightedObservedDataPresenter _presenter;

      protected override void Context()
      {
         _eventPublisher = A.Fake<IEventPublisher>();
         _view = A.Fake<IParameterIdentificationWeightedObservedDataCollectorView>();
         _applicationController = A.Fake<IApplicationController>();
         sut = new ParameterIdentificationWeightedObservedDataCollectorPresenter(_view, _applicationController, _eventPublisher);

         _parameterIdentification = new ParameterIdentification();
         _weightedObservedData = new WeightedObservedData(DomainHelperForSpecs.ObservedData());
         _outputMapping = new OutputMapping {WeightedObservedData = _weightedObservedData};
         _parameterIdentification.AddOutputMapping(_outputMapping);

         _presenter = A.Fake<IParameterIdentificationWeightedObservedDataPresenter>();
         A.CallTo(() => _applicationController.Start<IParameterIdentificationWeightedObservedDataPresenter>()).Returns(_presenter);
      }
   }

   public class When_the_weighted_observed_data_collector_presenter_is_edtiting_a_parameter_identification : concern_for_ParameterIdentificationWeightedObservedDataCollectorPresenter
   {
      protected override void Because()
      {
         sut.EditParameterIdentification(_parameterIdentification);
      }

      [Observation]
      public void should_add_a_sub_view_for_each_mapped_observed_data()
      {
         A.CallTo(() => _view.AddObservedDataView(_presenter.View)).MustHaveHappened();
      }

      [Observation]
      public void should_edit_the_weighted_observed_data()
      {
         A.CallTo(() => _presenter.Edit(_weightedObservedData)).MustHaveHappened();
      }
   }

   public class When_the_weighted_observed_data_collector_presenter_is_editing_a_parmaeter_identification_with_an_output_using_an_invalid_weighted_observed_data : concern_for_ParameterIdentificationWeightedObservedDataCollectorPresenter
   {
      protected override void Context()
      {
         base.Context();
#pragma warning disable 618
         _weightedObservedData = new WeightedObservedData();
#pragma warning restore 618
         _outputMapping.WeightedObservedData = _weightedObservedData;
      }

      protected override void Because()
      {
         sut.EditParameterIdentification(_parameterIdentification);
      }

      [Observation]
      public void should_not_add_a_view_for_this_observed_data()
      {
         A.CallTo(() => _presenter.Edit(_weightedObservedData)).MustNotHaveHappened();
      }
   }

   public class When_the_weighted_observed_data_collector_presenter_is_told_to_remove_some_observed_data_from_its_view : concern_for_ParameterIdentificationWeightedObservedDataCollectorPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.EditParameterIdentification(_parameterIdentification);
      }

      protected override void Because()
      {
         sut.RemoveObservedData(_weightedObservedData);
      }

      [Observation]
      public void should_remove_the_underlying_presenter_from_the_view()
      {
         A.CallTo(() => _view.RemoveObservedDataView(_presenter.View)).MustHaveHappened();
      }

      [Observation]
      public void should_release_the_presenter_from_the_event_publisher()
      {
         A.CallTo(() => _presenter.ReleaseFrom(_eventPublisher)).MustHaveHappened();
      }
   }

   public class When_the_observed_data_collector_is_told_that_a_view_displayed_weighted_observed_data_was_selected : concern_for_ParameterIdentificationWeightedObservedDataCollectorPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.EditParameterIdentification(_parameterIdentification);
      }

      protected override void Because()
      {
         sut.ObservedDataViewSelected(_presenter.View);
      }

      [Observation]
      public void should_edit_the_corresponding_presenter()
      {
         A.CallTo(() => _presenter.Edit(_weightedObservedData)).MustHaveHappened();
      }
   }

   public class When_the_weighted_observed_data_collector_presenter_is_told_to_select_some_given_weighted_observed_data : concern_for_ParameterIdentificationWeightedObservedDataCollectorPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.EditParameterIdentification(_parameterIdentification);
      }

      protected override void Because()
      {
         sut.SelectObservedData(_weightedObservedData);
      }

      [Observation]
      public void should_activate_the_view_registered_for_those_observed_data()
      {
         A.CallTo(() => _view.SelectObservedDataView(_presenter.View)).MustHaveHappened();
      }

      [Observation]
      public void should_edit_the_corresponding_presenter()
      {
         A.CallTo(() => _presenter.Edit(_weightedObservedData)).MustHaveHappened();
      }
   }

   public class When_the_weighted_observed_data_presenter_is_being_released : concern_for_ParameterIdentificationWeightedObservedDataCollectorPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.EditParameterIdentification(_parameterIdentification);
      }

      protected override void Because()
      {
         sut.ReleaseFrom(_eventPublisher);
      }

      [Observation]
      public void should_also_release_all_sub_presenters()
      {
         A.CallTo(() => _presenter.ReleaseFrom(_eventPublisher)).MustHaveHappened();
      }
   }
}