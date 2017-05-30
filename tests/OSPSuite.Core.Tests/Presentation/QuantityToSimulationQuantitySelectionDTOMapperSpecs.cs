using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Mappers;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_QuantityToSimulationQuantitySelectionDTOMapper : ContextSpecification<IQuantityToSimulationQuantitySelectionDTOMapper>
   {
      protected IQuantityToQuantitySelectionDTOMapper _quantitySelectionDTOMapper;
      protected IQuantityPathToQuantityDisplayPathMapper _quantityDisplayPathMapper;

      protected override void Context()
      {
         _quantitySelectionDTOMapper = A.Fake<IQuantityToQuantitySelectionDTOMapper>();
         _quantityDisplayPathMapper = A.Fake<IQuantityPathToQuantityDisplayPathMapper>();
         sut = new QuantityToSimulationQuantitySelectionDTOMapper(_quantitySelectionDTOMapper, _quantityDisplayPathMapper);
      }
   }

   public class When_mapping_a_null_quantity_to_a_simulation_quantity_selection_dto : concern_for_QuantityToSimulationQuantitySelectionDTOMapper
   {
      protected override void Context()
      {
         base.Context();
         A.CallTo(_quantitySelectionDTOMapper).WithReturnType<QuantitySelectionDTO>().Returns(null);
      }

      [Observation]
      public void should_return_null()
      {
         sut.MapFrom(A.Fake<ISimulation>(), null).ShouldBeNull();
      }
   }
}