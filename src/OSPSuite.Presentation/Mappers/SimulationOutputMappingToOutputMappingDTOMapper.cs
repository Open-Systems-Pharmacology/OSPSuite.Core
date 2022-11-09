using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Presentation.DTO;

namespace OSPSuite.Presentation.Mappers
{
   public interface ISimulationOutputMappingToOutputMappingDTOMapper
   {
      SimulationOutputMappingDTO MapFrom(OutputMapping outputMapping, IEnumerable<SimulationQuantitySelectionDTO> allOutputs);
   }

   public class SimulationOutputMappingToOutputMappingDTOMapper : ISimulationOutputMappingToOutputMappingDTOMapper
   {
      public SimulationOutputMappingDTO MapFrom(OutputMapping outputMapping, IEnumerable<SimulationQuantitySelectionDTO> allOutputs)
      {
         return new SimulationOutputMappingDTO(outputMapping)
         {
            Output = allOutputs.FirstOrDefault(x => matches(outputMapping, x))
         };
      }

      private static bool matches(OutputMapping outputMapping, SimulationQuantitySelectionDTO simulationQuantitySelectionDTO)
      {
         return Equals(simulationQuantitySelectionDTO.Simulation, outputMapping.Simulation) &&
                Equals(simulationQuantitySelectionDTO.QuantityPath, outputMapping.OutputPath);
      }
   }
}