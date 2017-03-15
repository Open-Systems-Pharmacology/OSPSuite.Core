using System;
using System.Collections.Generic;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Helpers;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Mappers;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_SimulationParametersPresenter : ContextSpecification<ISimulationParametersPresenter>
   {
      protected ParameterIdentification _parameterIdentification;
      protected ISimulationParametersView _view;
      protected ISimulation _simulation;
      protected IEntitiesInSimulationRetriever _entitiesInSimulationRetriever;
      protected PathCache<IParameter> _allParameters;
      protected IQuantityToSimulationParameterSelectionDTOMapper _simulationParameterSelectionDTOMapper;
      protected IEnumerable<SimulationParameterSelectionDTO> _allQuantitySelectionDTO;
      protected IGroupRepository _groupRepository;
      protected IParameterAnalysableParameterSelector _parameterSelector;

      protected override void Context()
      {
         _entitiesInSimulationRetriever = A.Fake<IEntitiesInSimulationRetriever>();
         _view = A.Fake<ISimulationParametersView>();
         _simulationParameterSelectionDTOMapper = A.Fake<IQuantityToSimulationParameterSelectionDTOMapper>();
         _groupRepository= A.Fake<IGroupRepository>();
         _parameterSelector= A.Fake<IParameterAnalysableParameterSelector>();

         sut = new SimulationParametersPresenter(_view, _entitiesInSimulationRetriever, _simulationParameterSelectionDTOMapper,_groupRepository, _parameterSelector);

         _allParameters = new PathCacheForSpecs<IParameter>();
         _parameterIdentification = new ParameterIdentification();
         _simulation = A.Fake<ISimulation>();
         _parameterIdentification.AddSimulation(_simulation);
         A.CallTo(() => _entitiesInSimulationRetriever.ParametersFrom(_simulation, A<Func<IParameter, bool>>._)).Returns(_allParameters);

         A.CallTo(() => _view.BindTo(A<IEnumerable<SimulationParameterSelectionDTO>>._))
            .Invokes(x => _allQuantitySelectionDTO = x.GetArgument<IEnumerable<SimulationParameterSelectionDTO>>(0));
      }
   }

   public class When_the_parameter_identificaiton_simulation_parameters_presenter_is_editing_a_parameter_identification : concern_for_SimulationParametersPresenter
   {
      private IParameter _parameter1;
      private IParameter _parameter2;
      private SimulationParameterSelectionDTO _simulatioQuantitySelectionDTO1;
      private SimulationParameterSelectionDTO _simulatioQuantitySelectionDTO2;

      protected override void Context()
      {
         base.Context();
         _parameter1 = A.Fake<IParameter>();
         _parameter2 = A.Fake<IParameter>();
         _allParameters.Add("PARA1", _parameter1);
         _allParameters.Add("PARA2", _parameter2);
         _simulatioQuantitySelectionDTO1 = new SimulationParameterSelectionDTO(_simulation, new QuantitySelectionDTO(), "PARA1");
         _simulatioQuantitySelectionDTO2 = new SimulationParameterSelectionDTO(_simulation, new QuantitySelectionDTO(), "PARA2");
         A.CallTo(() => _simulationParameterSelectionDTOMapper.MapFrom(_simulation, _parameter1)).Returns(_simulatioQuantitySelectionDTO1);
         A.CallTo(() => _simulationParameterSelectionDTOMapper.MapFrom(_simulation, _parameter2)).Returns(_simulatioQuantitySelectionDTO2);
      }

      protected override void Because()
      {
         sut.EditParameterAnalysable(_parameterIdentification);
      }

      [Observation]
      public void should_retrieve_all_parameters_defined_in_the_simulations_that_should_be_displayed_and_add_them_to_the_view()
      {
         _allQuantitySelectionDTO.ShouldOnlyContain(_simulatioQuantitySelectionDTO1, _simulatioQuantitySelectionDTO2);
      }

      [Observation]
      public void should_group_the_view_by_simulation_and_container()
      {
         A.CallTo(() => _view.GroupBy(PathElement.Container, 0)).MustHaveHappened();
         A.CallTo(() => _view.GroupBy(PathElement.Name, 1)).MustHaveHappened();
      }
   }

   public class When_the_parameter_identificaiton_simulation_parameters_presenter_is_notified_that_a_simulation_was_added_to_the_parameter_identification : concern_for_SimulationParametersPresenter
   {
      private IParameter _parameter1;
      private SimulationParameterSelectionDTO _simulatioQuantitySelectionDTO1;

      protected override void Context()
      {
         base.Context();
         sut.EditParameterAnalysable(_parameterIdentification);
         _parameter1 = A.Fake<IParameter>();
         _allParameters.Add("PARA1", _parameter1);
         _simulatioQuantitySelectionDTO1 = new SimulationParameterSelectionDTO(_simulation, new QuantitySelectionDTO(), "PARA1");
         A.CallTo(() => _simulationParameterSelectionDTOMapper.MapFrom(_simulation, _parameter1)).Returns(_simulatioQuantitySelectionDTO1);
      }

      protected override void Because()
      {
         sut.AddParametersOf(_simulation);
      }

      [Observation]
      public void should_update_the_view_with_the_parameters_of_this_simulation()
      {
         _allQuantitySelectionDTO.ShouldContain(_simulatioQuantitySelectionDTO1);
         A.CallTo(() => _view.Rebind()).MustHaveHappened();
      }
   }

   public class When_the_parameter_identificaiton_simulation_parameters_presenter_is_notified_that_a_simulation_was_removed_from_the_parameter_identification : concern_for_SimulationParametersPresenter
   {
      private IParameter _parameter1;
      private SimulationParameterSelectionDTO _simulatioQuantitySelectionDTO1;

      protected override void Context()
      {
         base.Context();
         _parameter1 = A.Fake<IParameter>();
         _allParameters.Add("PARA1", _parameter1);
         _simulatioQuantitySelectionDTO1 = new SimulationParameterSelectionDTO(_simulation, new QuantitySelectionDTO(), "PARA1");
         A.CallTo(() => _simulationParameterSelectionDTOMapper.MapFrom(_simulation, _parameter1)).Returns(_simulatioQuantitySelectionDTO1);
         sut.EditParameterAnalysable(_parameterIdentification);
      }

      protected override void Because()
      {
         sut.RemoveParametersFrom(_simulation);
      }

      [Observation]
      public void should_update_the_view_and_remove_the_parameters_of_this_simulation()
      {
         _allQuantitySelectionDTO.ShouldNotContain(_simulatioQuantitySelectionDTO1);
         A.CallTo(() => _view.Rebind()).MustHaveHappened();
      }
   }

   public class When_checking_if_a_parameter_should_be_displayed_int_the_view : concern_for_SimulationParametersPresenter
   {
      private IParameter _parameter1;
      private Func<IParameter, bool> _canEditParameter;
      private IParameter _parameter2;

      protected override void Context()
      {
         base.Context();

         _parameter1 = A.Fake<IParameter>();
         _parameter1.GroupName = "Advanced";
         A.CallTo(() => _parameterSelector.CanUseParameter(_parameter1)).Returns(true);

         _parameter2 = A.Fake<IParameter>();
         _parameter2.GroupName = "Simple";
         A.CallTo(() => _parameterSelector.CanUseParameter(_parameter2)).Returns(true);

         var advancedGroup = A.Fake<IGroup>();
         advancedGroup.IsAdvanced = true;

         var simpleGroup = A.Fake<IGroup>();
         simpleGroup.IsAdvanced = false;
         A.CallTo(() => _groupRepository.GroupByName(_parameter1.GroupName)).Returns(advancedGroup);
         A.CallTo(() => _groupRepository.GroupByName(_parameter2.GroupName)).Returns(simpleGroup);

         sut.EditParameterAnalysable(_parameterIdentification);

         A.CallTo(() => _entitiesInSimulationRetriever.ParametersFrom(_simulation, A<Func<IParameter, bool>>._))
            .Invokes(x => _canEditParameter = x.GetArgument<Func<IParameter, bool>>(1))
            .Returns(_allParameters);

      }

      [Observation]
      public void should_return_true_for_a_simple_parameter_in_simple_mode_and_false_for_an_advanced_parameter()
      {
         sut.ParameterGroupingMode = ParameterGroupingModes.Simple;
         _canEditParameter(_parameter1).ShouldBeFalse();
         _canEditParameter(_parameter2).ShouldBeTrue();
      }

      [Observation]
      public void should_return_true_for_a_simple_parameter_and_an_advanced_parameter_in_advanced_mode()
      {
         sut.ParameterGroupingMode = ParameterGroupingModes.Advanced;
         _canEditParameter(_parameter1).ShouldBeTrue();
         _canEditParameter(_parameter2).ShouldBeTrue();
      }
   }


   public class When_retrieving_the_available_grouping_mode_for_the_parameter_identification_simulation_parameters_presenter : concern_for_SimulationParametersPresenter
   {
      [Observation]
      public void should_return_the_execpted_mode()
      {
         sut.AllGroupingModes.ShouldOnlyContainInOrder(ParameterGroupingModes.Simple, ParameterGroupingModes.Advanced);
      }
   }

}