using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Repositories;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Views.ParameterIdentifications;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_SelectionSimulationPresenter : ContextSpecification<ISelectionSimulationPresenter>
   {
      private ISimulationRepository _simulationRepository;
      protected ISelectionSimulationView _view;
      protected List<SimulationSelectionDTO> _allSimulationDTOs;
      protected ISimulation _selectedSimulation1;
      protected ISimulation _simulation1;
      protected ISimulation _simulation2;
      private ISimulationSelector _simulationSelector;

      protected override void Context()
      {
         _simulationRepository = A.Fake<ISimulationRepository>();
         _view = A.Fake<ISelectionSimulationView>();
         _simulationSelector = A.Fake<ISimulationSelector>();
         sut = new SelectionSimulationPresenter(_view, _simulationRepository, _simulationSelector);

         A.CallTo(() => _simulationSelector.SimulationCanBeUsedForIdentification(A<ISimulation>._)).Returns(true);

         A.CallTo(() => _view.BindTo(A<IEnumerable<SimulationSelectionDTO>>._))
            .Invokes(x =>
            {
               _allSimulationDTOs = x.GetArgument<IEnumerable<SimulationSelectionDTO>>(0).ToList();
               _allSimulationDTOs[0].Selected = true;
            });

         _selectedSimulation1 = A.Fake<ISimulation>().WithId("1");
         _simulation1 = A.Fake<ISimulation>().WithId("2");
         _simulation2 = A.Fake<ISimulation>().WithId("3");

         A.CallTo(() => _simulationRepository.All()).Returns(new[] {_simulation1, _selectedSimulation1, _simulation2});
      }
   }

   public class When_starting_the_selection_of_simulation_with__the_selection_simulation_presenter : concern_for_SelectionSimulationPresenter
   {
      private IEnumerable<ISimulation> _results;

      protected override void Context()
      {
         base.Context();
         A.CallTo(() => _view.Canceled).Returns(false);
      }

      protected override void Because()
      {
         _results = sut.StartSelection(new[] {_selectedSimulation1});
      }

      [Observation]
      public void should_bind_the_list_of_available_simulations_to_the_view()
      {
         _allSimulationDTOs.Select(x => x.Simulation).ShouldOnlyContain(_simulation1, _simulation2);
      }

      [Observation]
      public void should_return_the_list_of_selected_simulation()
      {
         _results.ShouldOnlyContain(_simulation1);
      }
   }

   public class When_checking_if_the_simulation_selection_presenter_can_be_closed : concern_for_SelectionSimulationPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.StartSelection(new[] {_selectedSimulation1});
         A.CallTo(() => _view.HasError).Returns(false);
      }

      [Observation]
      public void should_return_true_if_at_least_one_simulation_is_selected()
      {
         _allSimulationDTOs.First().Selected = true;
         sut.CanClose.ShouldBeTrue();
      }

      [Observation]
      public void should_return_false_if_no_simulation_is_selected()
      {
         _allSimulationDTOs.Each(x => x.Selected = false);
         sut.CanClose.ShouldBeFalse();
      }
   }
}