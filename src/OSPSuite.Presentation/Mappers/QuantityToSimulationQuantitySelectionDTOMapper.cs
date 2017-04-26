using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Presentation.DTO;

namespace OSPSuite.Presentation.Mappers
{
   public interface IQuantityToSimulationQuantitySelectionDTOMapper
   {
      SimulationQuantitySelectionDTO MapFrom(ISimulation simulation, IQuantity quantity);
   }

   public class QuantityToSimulationQuantitySelectionDTOMapper : AbstractQuantityToSimulationQuantitySelectionDTOMapper, IQuantityToSimulationQuantitySelectionDTOMapper

   {
      private readonly IQuantityToQuantitySelectionDTOMapper _quantitySelectionDTOMapper;
      private readonly IQuantityPathToQuantityDisplayPathMapper _quantityDisplayPathMapper;

      public QuantityToSimulationQuantitySelectionDTOMapper(IQuantityToQuantitySelectionDTOMapper quantitySelectionDTOMapper, IQuantityPathToQuantityDisplayPathMapper quantityDisplayPathMapper)
      {
         _quantitySelectionDTOMapper = quantitySelectionDTOMapper;
         _quantityDisplayPathMapper = quantityDisplayPathMapper;
      }

      public SimulationQuantitySelectionDTO MapFrom(ISimulation simulation, IQuantity quantity)
      {
         var quantitySelectionDTO = _quantitySelectionDTOMapper.MapFrom(quantity);
         if (quantitySelectionDTO == null)
            return null;

         var displayString = _quantityDisplayPathMapper.DisplayPathAsStringFor(quantity, addSimulationName: true);
         UpdateContainerDisplayNameAndIconsIfEmpty(quantitySelectionDTO, quantity);
         return new SimulationQuantitySelectionDTO(simulation, quantitySelectionDTO, displayString);
      }
   }
}