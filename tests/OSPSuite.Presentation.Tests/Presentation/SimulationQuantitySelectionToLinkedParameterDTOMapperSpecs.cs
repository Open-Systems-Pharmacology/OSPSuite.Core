using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.DTO.ParameterIdentifications;
using OSPSuite.Presentation.Mappers;
using OSPSuite.Presentation.Mappers.ParameterIdentifications;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_SimulationQuantitySelectionToLinkedParameterDTOMapper : ContextSpecification<ISimulationQuantitySelectionToLinkedParameterDTOMapper>
   {
      protected IQuantityToSimulationQuantitySelectionDTOMapper _simulationQuantitySelectionMapper;

      protected override void Context()
      {
         _simulationQuantitySelectionMapper = A.Fake<IQuantityToSimulationQuantitySelectionDTOMapper>();
         sut = new SimulationQuantitySelectionToLinkedParameterDTOMapper(_simulationQuantitySelectionMapper);
      }
   }

   public class When_mapping_an_invalid_simulation_quantity_selection_to_linked_parameter_dto : concern_for_SimulationQuantitySelectionToLinkedParameterDTOMapper
   {
      private SimulationQuantitySelection _simulationQuantitySelection;

      protected override void Context()
      {
         base.Context();
         _simulationQuantitySelection = A.Fake<SimulationQuantitySelection>();
         A.CallTo(_simulationQuantitySelectionMapper).WithReturnType<SimulationQuantitySelectionDTO>().Returns(null);
      }

      [Observation]
      public void should_return_an_instance_of_null_linked_parameter_dto()
      {
         sut.MapFrom(_simulationQuantitySelection).ShouldBeAnInstanceOf<NullLinkedParameterDTO>();
      }
   }
}