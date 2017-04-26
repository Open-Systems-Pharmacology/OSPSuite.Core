using System.Collections.Generic;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Presenters;

namespace OSPSuite.Presentation.Views
{
   public interface ISimulationParametersView : IView<ISimulationParametersPresenter>
   {
      void BindTo(IEnumerable<SimulationParameterSelectionDTO> allParameterDTOs);
      void Rebind();

      void BindToModeSelection();
      void GroupBy(PathElement pathElement, int groupIndex = 0);
      IEnumerable<SimulationParameterSelectionDTO> SelectedParameters { get; }
   }
}