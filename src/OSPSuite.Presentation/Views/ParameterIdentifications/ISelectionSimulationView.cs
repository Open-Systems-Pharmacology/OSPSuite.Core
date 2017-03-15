using System.Collections.Generic;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;

namespace OSPSuite.Presentation.Views.ParameterIdentifications
{
   public interface ISelectionSimulationView : IModalView<ISelectionSimulationPresenter>
   {
      void BindTo(IEnumerable<SimulationSelectionDTO> simulationSelectionDTOs);
      bool AllowMultiSelect { get; set; }
   }
}