using System.Collections.Generic;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.DTO.ParameterIdentifications; //this have to be moved out of the parameter identifocation and into a different file
//possibly we need a separate DTO for the Simulation
using OSPSuite.Presentation.Presenters;

namespace OSPSuite.Presentation.Views
{
   public interface ISimulationOutputMappingView : IView<ISimulationOutputMappingPresenter>
   {
      void BindTo(IEnumerable<SimulationOutputMappingDTO> outputMappingList);
      //void CloseEditor();
      void RefreshGrid();
   }
}