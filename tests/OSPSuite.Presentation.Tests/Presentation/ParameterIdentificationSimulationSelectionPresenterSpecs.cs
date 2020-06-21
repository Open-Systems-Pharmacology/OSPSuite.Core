using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Nodes;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Views.ParameterIdentifications;
using IDialogCreator = OSPSuite.Core.Services.IDialogCreator;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_ParameterIdentificationSimulationSelectionPresenter : ContextSpecification<IParameterIdentificationSimulationSelectionPresenter>
   {
      protected IParameterIdentificationSimulationSelectionView _view;
      protected ISimulation _simulation;
      protected ParameterIdentification _parameterIdentification;
      private ITreeNodeFactory _treeNodeFactory;
      protected ITreeNode _simulationNode;
      protected IApplicationController _applicationController;
      protected ILazyLoadTask _lazyLoadTask;
      private ITreeNodeContextMenuFactory _treeNodeContextMenuFactory;
      private IMultipleTreeNodeContextMenuFactory _multipleTreeNodeContextMenuFactory;
      protected IDialogCreator _dialogCreator;
      protected OutputMapping _outputMapping;
      protected IParameterIdentificationTask _parameterIdentificationTask;
      protected ISelectionSimulationPresenter _simulationSelectionPresenter;

      protected override void Context()
      {
         _view = A.Fake<IParameterIdentificationSimulationSelectionView>();
         _treeNodeFactory = A.Fake<ITreeNodeFactory>();
         _applicationController = A.Fake<IApplicationController>();
         _lazyLoadTask = A.Fake<ILazyLoadTask>();
         _treeNodeContextMenuFactory = A.Fake<ITreeNodeContextMenuFactory>();
         _multipleTreeNodeContextMenuFactory = A.Fake<IMultipleTreeNodeContextMenuFactory>();
         _dialogCreator = A.Fake<IDialogCreator>();
         _parameterIdentificationTask= A.Fake<IParameterIdentificationTask>();
         sut = new ParameterIdentificationSimulationSelectionPresenter(_view, _treeNodeFactory, _applicationController, _lazyLoadTask, _treeNodeContextMenuFactory,
            _multipleTreeNodeContextMenuFactory, _dialogCreator, _parameterIdentificationTask);

         _parameterIdentification = new ParameterIdentification();
         _simulation = A.Fake<ISimulation>().WithId("Id").WithName("S");
         _parameterIdentification.AddSimulation(_simulation);
         _outputMapping = new OutputMapping
         {
            OutputSelection = new SimulationQuantitySelection(_simulation, new QuantitySelection("PATH", QuantityType.Drug))
         };
         _parameterIdentification.AddOutputMapping(_outputMapping);

         _simulationNode = A.Fake<ITreeNode>();
         A.CallTo(() => _treeNodeFactory.CreateFor(_simulation)).Returns(_simulationNode);

         _simulationSelectionPresenter = A.Fake<ISelectionSimulationPresenter>();
         A.CallTo(() => _applicationController.Start<ISelectionSimulationPresenter>()).Returns(_simulationSelectionPresenter);

         sut.EditParameterIdentification(_parameterIdentification);
      }
   }

   public class When_the_parameter_identification_simulation_selection_presenter_is_editing_a_parameter_identification : concern_for_ParameterIdentificationSimulationSelectionPresenter
   {
      [Observation]
      public void should_add_all_simulations_defined_in_the_parameter_identification_to_the_view()
      {
         A.CallTo(() => _view.AddNode(_simulationNode)).MustHaveHappened();
      }

      [Observation]
      public void should_load_the_simulation()
      {
         A.CallTo(() => _lazyLoadTask.Load(_simulation)).MustHaveHappened();
      }
   }

   public class When_adding_a_new_simulation_to_a_parameter_identification : concern_for_ParameterIdentificationSimulationSelectionPresenter
   {
      private ISimulation _newSimulation;
      private ISimulation _simulationAdded;

      protected override void Context()
      {
         base.Context();
         _newSimulation = A.Fake<ISimulation>().WithId("New");
         sut.SimulationAdded += (o, e) => { _simulationAdded = e.Simulation; };
      }

      protected override void Because()
      {
         sut.AddSimulation(_newSimulation);
      }

      [Observation]
      public void should_add_the_simulation_to_the_parameter_identification()
      {
         A.CallTo(() => _parameterIdentificationTask.AddSimulationTo(_parameterIdentification, _newSimulation)).MustHaveHappened();
      }

      [Observation]
      public void should_load_the_simulation()
      {
         A.CallTo(() => _lazyLoadTask.Load(_newSimulation)).MustHaveHappened();
      }

      [Observation]
      public void should_raise_the_add_simulation_event()
      {
         _simulationAdded.ShouldBeEqualTo(_newSimulation);
      }
   }

   public class When_adding_a_simulation_that_was_already_added_to_the_parameter_identification : concern_for_ParameterIdentificationSimulationSelectionPresenter
   {
      private ISimulation _simulationAdded;

      protected override void Context()
      {
         base.Context();
         sut.SimulationAdded += (o, e) => { _simulationAdded = e.Simulation; };
      }

      protected override void Because()
      {
         sut.AddSimulation(_simulation);
      }

      [Observation]
      public void should_npt_raise_the_add_simulation_event()
      {
         _simulationAdded.ShouldBeNull();
      }
   }

   public class When_the_user_is_starting_a_swap_of_a_simulation_in_the_parameter_identification_but_none_is_selected : concern_for_ParameterIdentificationSimulationSelectionPresenter
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(_simulationSelectionPresenter).WithReturnType<IEnumerable<ISimulation>>().Returns(Enumerable.Empty<ISimulation>());
      }

      protected override void Because()
      {
         sut.ReplaceSimulation(_simulation);
      }

      [Observation]
      public void the_parameter_identification_is_not_used_for_swapping_simulations()
      {
         A.CallTo(() => _parameterIdentificationTask.SwapSimulations(A<ParameterIdentification>._, A<ISimulation>._, A<ISimulation>._)).MustNotHaveHappened();
      }
   }

   public class When_the_user_is_starting_a_swap_of_a_simulation_in_the_parameter_identification : concern_for_ParameterIdentificationSimulationSelectionPresenter
   {
      private ISimulation _newSimulation;

      protected override void Context()
      {
         base.Context();
         _newSimulation = A.Fake<ISimulation>().WithId("NewSim");
         A.CallTo(_simulationSelectionPresenter).WithReturnType<IEnumerable<ISimulation>>().Returns(new[] { _newSimulation });
      }

      protected override void Because()
      {
         sut.ReplaceSimulation(_simulation);
      }

      [Observation]
      public void the_parameter_identification_task_is_used_to_swap_the_selected_simulations()
      {
         A.CallTo(() => _parameterIdentificationTask.SwapSimulations(_parameterIdentification, _simulation, _newSimulation)).MustHaveHappened();
      }
   }

   public class When_the_user_is_starting_the_selection_of_multiple_simulation_to_add_to_the_parameter_identification : concern_for_ParameterIdentificationSimulationSelectionPresenter
   {
      private ISimulation _newSimulation;

      protected override void Context()
      {
         base.Context();
         _newSimulation = A.Fake<ISimulation>().WithId("NewSim");
         A.CallTo(_simulationSelectionPresenter).WithReturnType<IEnumerable<ISimulation>>().Returns(new[] { _newSimulation });
      }

      protected override void Because()
      {
         sut.SelectSimulationsToAdd();
      }

      [Observation]
      public void should_add_the_selected_simulations_to_the_parameter_identification()
      {
         A.CallTo(() => _parameterIdentificationTask.AddSimulationTo(_parameterIdentification, _newSimulation)).MustHaveHappened();
      }
   }

   public class When_refreshing_the_simulation_selection_presenter_editing_a_given_parameter_identification : concern_for_ParameterIdentificationSimulationSelectionPresenter
   {
      protected override void Because()
      {
         sut.Refresh();
      }

      [Observation]
      public void should_rebind_to_the_parameter_identification()
      {
         A.CallTo(() => _view.AddNode(A<ITreeNode>._)).MustHaveHappenedTwiceExactly();
      }
   }

   public class When_removing_a_simulation_from_a_parameter_identification_whose_outputs_are_not_being_mapped : concern_for_ParameterIdentificationSimulationSelectionPresenter
   {
      private ISimulation _removedSimulation;

      protected override void Context()
      {
         base.Context();
         _parameterIdentification.RemoveOutputMapping(_outputMapping);
         sut.SimulationRemoved += (o, e) => { _removedSimulation = e.Simulation; };
      }

      protected override void Because()
      {
         sut.RemoveSimulation(_simulation);
      }

      [Observation]
      public void should_lazy_load_the_simulation()
      {
         A.CallTo(() => _lazyLoadTask.Load(_simulation)).MustHaveHappened();
      }

      [Observation]
      public void should_remove_the_simulation_from_the_parameter_identification()
      {
         _parameterIdentification.UsesSimulation(_simulation).ShouldBeFalse();
      }

      [Observation]
      public void should_not_warn_the_user()
      {
         A.CallTo(_dialogCreator).WithReturnType<ViewResult>().MustNotHaveHappened();
      }

      [Observation]
      public void should_raise_the_simulation_removed_event()
      {
         _removedSimulation.ShouldBeEqualTo(_simulation);
      }
   }

   public class When_removing_a_simulation_from_a_parameter_identification_whose_outputs_are_being_mapped : concern_for_ParameterIdentificationSimulationSelectionPresenter
   {
      protected override void Because()
      {
         sut.RemoveSimulation(_simulation);
      }

      [Observation]
      public void should_warn_the_user()
      {
         A.CallTo(_dialogCreator).WithReturnType<ViewResult>().MustHaveHappened();
      }
   }

   public class When_removing_a_simulation_from_a_parameter_identification_whose_outputs_are_being_mapped_and_the_user_cancels_deletion : concern_for_ParameterIdentificationSimulationSelectionPresenter
   {
      private ISimulation _removedSimulation;

      protected override void Context()
      {
         base.Context();
         A.CallTo(_dialogCreator).WithReturnType<ViewResult>().Returns(ViewResult.No);
         sut.SimulationRemoved += (o, e) => { _removedSimulation = e.Simulation; };
      }

      protected override void Because()
      {
         sut.RemoveSimulation(_simulation);
      }

      [Observation]
      public void should_not_remove_the_simulation_from_the_parameter_identification()
      {
         _parameterIdentification.UsesSimulation(_simulation).ShouldBeTrue();
      }

      [Observation]
      public void should_not_raise_the_simulation_removed_event()
      {
         _removedSimulation.ShouldBeNull();
      }
   }

   public class When_removing_a_simulation_from_a_parameter_identification_whose_outputs_are_being_mapped_and_the_user_confirms_action : concern_for_ParameterIdentificationSimulationSelectionPresenter
   {
      private ISimulation _removedSimulation;

      protected override void Context()
      {
         base.Context();
         A.CallTo(_dialogCreator).WithReturnType<ViewResult>().Returns(ViewResult.Yes);
         sut.SimulationRemoved += (o, e) => { _removedSimulation = e.Simulation; };
      }

      protected override void Because()
      {
         sut.RemoveSimulation(_simulation);
      }

      [Observation]
      public void should_remove_the_simulation_from_the_parameter_identification()
      {
         _parameterIdentification.UsesSimulation(_simulation).ShouldBeFalse();
      }

      [Observation]
      public void should_remove_the_mapping_belonging_to_this_simulation()
      {
         _parameterIdentification.AllOutputMappings.Any().ShouldBeFalse();
      }

      [Observation]
      public void should_raise_the_simulation_removed_event()
      {
         _removedSimulation.ShouldBeEqualTo(_simulation);
      }
   }
}