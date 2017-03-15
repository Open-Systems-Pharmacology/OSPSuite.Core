using OSPSuite.Utility;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.DTO.ParameterIdentifications;

namespace OSPSuite.Presentation.Mappers.ParameterIdentifications
{
   public interface ISimulationQuantitySelectionToLinkedParameterDTOMapper : IMapper<SimulationQuantitySelection, LinkedParameterDTO>
   {
   }

   public class SimulationQuantitySelectionToLinkedParameterDTOMapper : ISimulationQuantitySelectionToLinkedParameterDTOMapper
   {
      private readonly IQuantityToSimulationQuantitySelectionDTOMapper _simulationQuantitySelectionMapper;

      public SimulationQuantitySelectionToLinkedParameterDTOMapper(IQuantityToSimulationQuantitySelectionDTOMapper simulationQuantitySelectionMapper)
      {
         _simulationQuantitySelectionMapper = simulationQuantitySelectionMapper;
      }

      public LinkedParameterDTO MapFrom(SimulationQuantitySelection simulationQuantitySelection)
      {
         var simulationQuantitySelectionDTO = _simulationQuantitySelectionMapper.MapFrom(simulationQuantitySelection.Simulation, simulationQuantitySelection.Quantity);
         if (simulationQuantitySelectionDTO == null)
            return new NullLinkedParameterDTO();

         return new LinkedParameterDTO(simulationQuantitySelectionDTO);
      }
   }
}