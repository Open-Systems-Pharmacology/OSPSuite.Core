using FakeItEasy;
using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.ParameterIdentifications.Algorithms;
using OSPSuite.Core.Domain.Repositories;
using OSPSuite.Presentation.Mappers.ParameterIdentifications;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Views.ParameterIdentifications;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_ParameterIdentificationConfigurationPresenter : ContextSpecification<ParameterIdentificationConfigurationPresenter>
   {
      protected IOptimizationAlgorithmToOptimizationAlgorithmPropertiesMapper _algorithmPropertiesMapper;
      protected IParameterIdentificationConfigurationToParameterIdentificationConfigurationDTOMapper _parameterIdentificationConfigurationDTOMapper;
      protected IOptimizationAlgorithmRepository _optimizationAlgorithmRepository;
      protected IExtendedPropertiesPresenter _parameterIdentificationAlgorithmOptionsPresenter;
      protected IParameterIdentificationConfigurationView _view;
      protected ParameterIdentification _parameterIdentification;
      protected IOptimizationAlgorithm _optimizationAlgorithm1;
      protected IOptimizationAlgorithm _optimizationAlgorithm2;
      private OptimizationAlgorithmProperties _identificationAlgorithm1;
      private OptimizationAlgorithmProperties _identificationAlgorithm2;
      protected IStandardParameterIdentificationRunModePresenter _noOptionsPresenter;
      protected IMultipleParameterIdentificationRunModePresenter _multipleParameterIdentificationRunModePresenter;
      protected ICategorialParameterIdentificationRunModePresenter _categorialParameterIdentificationRunModePresenter;

      protected override void Context()
      {
         _optimizationAlgorithm1 = A.Fake<IOptimizationAlgorithm>();
         _optimizationAlgorithm2 = A.Fake<IOptimizationAlgorithm>();
         A.CallTo(() => _optimizationAlgorithm2.Name).Returns(Constants.OptimizationAlgorithm.DEFAULT);
         _identificationAlgorithm1 = A.Fake<OptimizationAlgorithmProperties>();
         _identificationAlgorithm2 = A.Fake<OptimizationAlgorithmProperties>();
         _algorithmPropertiesMapper = A.Fake<IOptimizationAlgorithmToOptimizationAlgorithmPropertiesMapper>();
         _parameterIdentificationConfigurationDTOMapper = A.Fake<IParameterIdentificationConfigurationToParameterIdentificationConfigurationDTOMapper>();
         _optimizationAlgorithmRepository = A.Fake<IOptimizationAlgorithmRepository>();
         _parameterIdentificationAlgorithmOptionsPresenter = A.Fake<IExtendedPropertiesPresenter>();
         _view = A.Fake<IParameterIdentificationConfigurationView>();
         _parameterIdentification = new ParameterIdentification();
         _noOptionsPresenter = A.Fake<IStandardParameterIdentificationRunModePresenter>();
         _multipleParameterIdentificationRunModePresenter = A.Fake<IMultipleParameterIdentificationRunModePresenter>();
         _categorialParameterIdentificationRunModePresenter = A.Fake<ICategorialParameterIdentificationRunModePresenter>();
         ConfigureCategorialParameterIdentificationRunModePresenter();
         sut = new ParameterIdentificationConfigurationPresenter(_view, _parameterIdentificationAlgorithmOptionsPresenter, _optimizationAlgorithmRepository, _parameterIdentificationConfigurationDTOMapper, _algorithmPropertiesMapper,
            _noOptionsPresenter, _multipleParameterIdentificationRunModePresenter, _categorialParameterIdentificationRunModePresenter);
         A.CallTo(() => _algorithmPropertiesMapper.MapFrom(_optimizationAlgorithm1)).Returns(_identificationAlgorithm1);
         A.CallTo(() => _algorithmPropertiesMapper.MapFrom(_optimizationAlgorithm2)).Returns(_identificationAlgorithm2);

         A.CallTo(() => _noOptionsPresenter.CanEdit(A<ParameterIdentification>.That.Matches(x => x.Configuration.RunMode.IsAnImplementationOf<StandardParameterIdentificationRunMode>()))).Returns(true);
         A.CallTo(() => _categorialParameterIdentificationRunModePresenter.CanEdit(A<ParameterIdentification>.That.Matches(x => x.Configuration.RunMode.IsAnImplementationOf<CategorialParameterIdentificationRunMode>()))).Returns(true);
         A.CallTo(() => _multipleParameterIdentificationRunModePresenter.CanEdit(A<ParameterIdentification>.That.Matches(x => x.Configuration.RunMode.IsAnImplementationOf<MultipleParameterIdentificationRunMode>()))).Returns(true);
      }

      protected virtual void ConfigureCategorialParameterIdentificationRunModePresenter()
      {
         A.CallTo(() => _categorialParameterIdentificationRunModePresenter.HasCategories).Returns(true);
      }
   }

   public class When_changing_the_options_to_none_type : concern_for_ParameterIdentificationConfigurationPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.EditParameterIdentification(_parameterIdentification);
      }

      protected override void Because()
      {
         sut.RunModePresenter = _noOptionsPresenter;
      }

      [Observation]
      public void the_presenter_for_none_options_should_be_used_to_edit_the_options()
      {
         A.CallTo(() => _view.UpdateOptimizationsView(_noOptionsPresenter.BaseView)).MustHaveHappened();
      }
   }

   public class When_changing_the_options_to_multiple_type : concern_for_ParameterIdentificationConfigurationPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.EditParameterIdentification(_parameterIdentification);
      }

      protected override void Because()
      {
         sut.RunModePresenter = _multipleParameterIdentificationRunModePresenter;
      }

      [Observation]
      public void the_presenter_for_multiple_options_should_be_used_to_edit_the_options()
      {
         A.CallTo(() => _view.UpdateOptimizationsView(_multipleParameterIdentificationRunModePresenter.BaseView)).MustHaveHappened();
      }
   }

   public class When_changing_the_options_to_category_type : concern_for_ParameterIdentificationConfigurationPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.EditParameterIdentification(_parameterIdentification);
      }

      protected override void Because()
      {
         sut.RunModePresenter = _categorialParameterIdentificationRunModePresenter;
      }

      [Observation]
      public void the_presenter_for_category_type_must_be_editing_the_options()
      {
         A.CallTo(() => _view.UpdateOptimizationsView(_categorialParameterIdentificationRunModePresenter.BaseView)).MustHaveHappened();
      }
   }

   public class when_changing_the_optimization_algorithm_to_one_that_has_already_been_initialized_in_the_presenter : concern_for_ParameterIdentificationConfigurationPresenter
   {
      protected override void Because()
      {
         sut.EditParameterIdentification(_parameterIdentification);
         sut.ChangeOptimizationAlgorithm(_optimizationAlgorithm1);
         sut.ChangeOptimizationAlgorithm(_optimizationAlgorithm2);
         sut.ChangeOptimizationAlgorithm(_optimizationAlgorithm1);
      }

      [Observation]
      public void should_result_in_a_mapping_of_a_new_identification_algorithm()
      {
         A.CallTo(() => _algorithmPropertiesMapper.MapFrom(_optimizationAlgorithm1)).MustHaveHappenedTwiceExactly();
      }
   }

   public class when_the_categorial_presenter_has_categories_to_change : concern_for_ParameterIdentificationConfigurationPresenter
   {
      protected override void Because()
      {
         sut.EditParameterIdentification(_parameterIdentification);
      }

      [Observation]
      public void the_categorial_presenter_is_one_of_the_options()
      {
         sut.Options().ShouldContain(_categorialParameterIdentificationRunModePresenter);
      }
   }

   public class when_the_categorial_presenter_does_not_have_any_categories_to_change : concern_for_ParameterIdentificationConfigurationPresenter
   {
      protected override void ConfigureCategorialParameterIdentificationRunModePresenter()
      {
         A.CallTo(() => _categorialParameterIdentificationRunModePresenter.HasCategories).Returns(false);
      }

      protected override void Because()
      {
         sut.EditParameterIdentification(_parameterIdentification);
      }

      [Observation]
      public void the_categorial_presenter_is_not_one_of_the_options()
      {
         sut.Options().ShouldNotContain(_categorialParameterIdentificationRunModePresenter);
      }
   }

   public class when_changing_the_optimization_algorithm_to_a_new_instance_of_a_new_algorithm : concern_for_ParameterIdentificationConfigurationPresenter
   {
      protected override void Because()
      {
         sut.EditParameterIdentification(_parameterIdentification);
         sut.ChangeOptimizationAlgorithm(_optimizationAlgorithm1);
      }

      [Observation]
      public void should_result_in_a_mapping_of_a_new_identification_algorithm()
      {
         A.CallTo(() => _algorithmPropertiesMapper.MapFrom(_optimizationAlgorithm1)).MustHaveHappenedOnceExactly();
      }
   }

   public class when_editing_a_parameter_identification : concern_for_ParameterIdentificationConfigurationPresenter
   {
      protected override void Because()
      {
         sut.EditParameterIdentification(_parameterIdentification);
      }

      [Observation]
      public void the_algorithm_options_presenter_should_edit_the_algorithm_options()
      {
         A.CallTo(() => _parameterIdentificationAlgorithmOptionsPresenter.Edit(_parameterIdentification.Configuration.AlgorithmProperties)).MustHaveHappened();
      }
   }

   public class when_editing_a_parameter_identification_that_does_not_have_a_optimization_algorithm_set : concern_for_ParameterIdentificationConfigurationPresenter
   {
      private OptimizationAlgorithmProperties _optimizationAlgoProperties;

      protected override void Context()
      {
         base.Context();
         _optimizationAlgoProperties = new OptimizationAlgorithmProperties("BLA");
         _parameterIdentification.Configuration.AlgorithmProperties = null;
         A.CallTo(() => _algorithmPropertiesMapper.MapFrom(_optimizationAlgorithm2)).Returns((_optimizationAlgoProperties));
      }

      protected override void Because()
      {
         sut.EditParameterIdentification(_parameterIdentification);
      }

      [Observation]
      public void should_set_a_the_default_optimization_algoritm()
      {
         Assert.AreEqual(_parameterIdentification.Configuration.AlgorithmProperties, _optimizationAlgoProperties);
      }
   }

   public class When_the_parameter_idenfication_parameter_presenter_is_notified_that_a_simulation_was_added_to_the_parameter_identification : concern_for_ParameterIdentificationConfigurationPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.EditParameterIdentification(_parameterIdentification);
         sut.RunModePresenter = A.Fake<IParameterIdentificationRunModePresenter>();
         ;
      }

      protected override void Because()
      {
         sut.SimulationAdded(A.Fake<ISimulation>());
      }

      [Observation]
      public void should_refresh_the_selected_run_mode_presenter()
      {
         A.CallTo(() => sut.RunModePresenter.Refresh()).MustHaveHappened();
      }
   }

   public class When_the_parameter_idenfication_parameter_presenter_is_notified_that_a_simulation_was_removed_to_the_parameter_identification : concern_for_ParameterIdentificationConfigurationPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.EditParameterIdentification(_parameterIdentification);
         sut.RunModePresenter = A.Fake<IParameterIdentificationRunModePresenter>();
         ;
      }

      protected override void Because()
      {
         sut.SimulationRemoved(A.Fake<ISimulation>());
      }

      [Observation]
      public void should_refresh_the_selected_run_mode_presenter()
      {
         A.CallTo(() => sut.RunModePresenter.Refresh()).MustHaveHappened();
      }
   }
}