using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Domain.Repositories;
using OSPSuite.Presentation.DTO;

namespace OSPSuite.Presentation.Mappers
{
   public interface IQuantityToSimulationParameterSelectionDTOMapper
   {
      SimulationParameterSelectionDTO MapFrom(ISimulation simulation, IParameter parameter);
   }

   public class QuantityToSimulationParameterSelectionDTOMapper : AbstractQuantityToSimulationQuantitySelectionDTOMapper, IQuantityToSimulationParameterSelectionDTOMapper

   {
      private readonly IQuantityToQuantitySelectionDTOMapper _quantitySelectionDTOMapper;
      private readonly IQuantityPathToQuantityDisplayPathMapper _quantityDisplayPathMapper;
      private readonly IFavoriteRepository _favoriteRepository;
      private readonly IParameterToParameterDTOMapper _parameterDTOMapper;

      public QuantityToSimulationParameterSelectionDTOMapper(IQuantityToQuantitySelectionDTOMapper quantitySelectionDTOMapper, IQuantityPathToQuantityDisplayPathMapper quantityDisplayPathMapper, IFavoriteRepository favoriteRepository, IParameterToParameterDTOMapper parameterDTOMapper)
      {
         _quantitySelectionDTOMapper = quantitySelectionDTOMapper;
         _quantityDisplayPathMapper = quantityDisplayPathMapper;
         _favoriteRepository = favoriteRepository;
         _parameterDTOMapper = parameterDTOMapper;
      }

      public SimulationParameterSelectionDTO MapFrom(ISimulation simulation, IParameter parameter)
      {
         var quantitySelectionDTO = _quantitySelectionDTOMapper.MapFrom(parameter);
         var displayString = _quantityDisplayPathMapper.DisplayPathAsStringFor(parameter, addSimulationName: true);
         UpdateContainerDisplayNameAndIconsIfEmpty(quantitySelectionDTO, parameter);
         return new SimulationParameterSelectionDTO(simulation, quantitySelectionDTO, displayString)
         {
            IsFavorite = _favoriteRepository.All().Contains(quantitySelectionDTO.QuantityPath),
            ValueParameter = _parameterDTOMapper.MapFrom(parameter)
         };
      }
   }
}