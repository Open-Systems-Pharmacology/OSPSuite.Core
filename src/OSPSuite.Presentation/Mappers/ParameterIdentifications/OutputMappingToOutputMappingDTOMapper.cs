using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.DTO.ParameterIdentifications;

namespace OSPSuite.Presentation.Mappers.ParameterIdentifications
{
   public interface IOutputMappingToOutputMappingDTOMapper
   {
      OutputMappingDTO MapFrom(OutputMapping outputMapping, IEnumerable<SimulationQuantitySelectionDTO> allOutputs);
   }

   public class OutputMappingToOutputMappingDTOMapper : IOutputMappingToOutputMappingDTOMapper
   {
      public OutputMappingDTO MapFrom(OutputMapping outputMapping, IEnumerable<SimulationQuantitySelectionDTO> allOutputs)
      {
         return new OutputMappingDTO(outputMapping)
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