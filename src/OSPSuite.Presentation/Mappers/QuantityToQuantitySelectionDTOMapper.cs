using OSPSuite.Utility;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.DTO;

namespace OSPSuite.Presentation.Mappers
{
   public interface IQuantityToQuantitySelectionDTOMapper : IMapper<IQuantity, QuantitySelectionDTO>
   {
      /// <summary>
      ///    Maps the quantity to a <see cref="QuantitySelectionDTO" />. The sequence is used to specifiy a custom sort when
      ///    binding the DTO
      /// </summary>
      QuantitySelectionDTO MapFrom(IQuantity quantity, int sequence);
   }

   public class QuantityToQuantitySelectionDTOMapper : IQuantityToQuantitySelectionDTOMapper
   {
      protected readonly IEntityPathResolver _entityPathResolver;
      protected readonly IPathToPathElementsMapper _pathToPathElementsMapper;

      public QuantityToQuantitySelectionDTOMapper(IEntityPathResolver entityPathResolver, IPathToPathElementsMapper pathToPathElementsMapper)
      {
         _entityPathResolver = entityPathResolver;
         _pathToPathElementsMapper = pathToPathElementsMapper;
      }

      public QuantitySelectionDTO MapFrom(IQuantity quantity)
      {
         //sequence does not play a roll in that case
         return MapFrom(quantity, 0);
      }

      public virtual QuantitySelectionDTO MapFrom(IQuantity quantity, int sequence)
      {
         if (quantity == null)
            return null;

         var quantityPath = _entityPathResolver.ObjectPathFor(quantity);
         var pathElements = _pathToPathElementsMapper.MapFrom(quantity);

         return new QuantitySelectionDTO
         {
            QuantityPath = quantityPath.PathAsString,
            QuantityName = quantity.Name,
            QuantityType = quantity.QuantityType,
            Dimension = quantity.Dimension,
            Quantity = quantity,
            Sequence = sequence,
            PathElements = pathElements
         };
      }
   }
}