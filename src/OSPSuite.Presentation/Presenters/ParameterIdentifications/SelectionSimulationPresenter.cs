using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Repositories;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Views.ParameterIdentifications;

namespace OSPSuite.Presentation.Presenters.ParameterIdentifications
{
   public interface ISelectionSimulationPresenter : IDisposablePresenter
   {
      IEnumerable<ISimulation> StartSelection(IEnumerable<ISimulation> simulationsAlreadySelected, bool allowMultiSelect = true);
   }

   public class SelectionSimulationPresenter : AbstractDisposablePresenter<ISelectionSimulationView, ISelectionSimulationPresenter>, ISelectionSimulationPresenter
   {
      private readonly ISimulationRepository _simulationRepository;
      private readonly ISimulationSelector _simulationSelector;
      private readonly List<SimulationSelectionDTO> _simulationSelectionDTOs;
      private IEnumerable<ISimulation> _simulationsAlreadySelected;

      public SelectionSimulationPresenter(ISelectionSimulationView view, ISimulationRepository simulationRepository, ISimulationSelector simulationSelector) : base(view)
      {
         _simulationRepository = simulationRepository;
         _simulationSelector = simulationSelector;
         _simulationSelectionDTOs = new List<SimulationSelectionDTO>();
      }

      public IEnumerable<ISimulation> StartSelection(IEnumerable<ISimulation> simulationsAlreadySelected, bool allowMultiSelect = true)
      {
         _simulationsAlreadySelected = simulationsAlreadySelected;
         availableSimulations.Each(s => _simulationSelectionDTOs.Add(mapFrom(s)));
         _view.BindTo(_simulationSelectionDTOs);
         _view.AllowMultiSelect = allowMultiSelect;
         _view.Display();

         if (_view.Canceled)
            return Enumerable.Empty<ISimulation>();

         return selectedSimulations;
      }

      private IEnumerable<ISimulation> selectedSimulations
      {
         get { return _simulationSelectionDTOs.Where(x => x.Selected).Select(x => x.Simulation); }
      }

      private SimulationSelectionDTO mapFrom(ISimulation simulation)
      {
         return new SimulationSelectionDTO(simulation);
      }

      private IEnumerable<ISimulation> availableSimulations
      {
         get
         {
            var allSimulationsInProject = _simulationRepository.All().Where(x => _simulationSelector.SimulationCanBeUsedForIdentification(x)).OrderBy(x => x.Name).ToList();
            _simulationsAlreadySelected.Each(s => allSimulationsInProject.Remove(s));
            return allSimulationsInProject;
         }
      }

      public override void ViewChanged()
      {
         base.ViewChanged();
         _view.OkEnabled = CanClose;
      }

      public override bool CanClose => base.CanClose && selectedSimulations.Any();
   }
}