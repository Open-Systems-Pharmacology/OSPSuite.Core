using System.Collections.Generic;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Presenters;

namespace OSPSuite.Presentation.Views
{
   public interface ISimulationOutputMappingView : IView<ISimulationOutputMappingPresenter>
   {
      void BindTo(IEnumerable<SimulationOutputMappingDTO> outputMappingList);
      void RefreshGrid();
   }
}