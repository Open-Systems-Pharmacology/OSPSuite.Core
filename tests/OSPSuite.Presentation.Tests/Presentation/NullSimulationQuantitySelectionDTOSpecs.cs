using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Presentation.DTO;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_NullSimulationQuantitySelectionDTO : ContextSpecification<NullSimulationQuantitySelectionDTO>
   {
      protected override void Context()
      {
         sut = new NullSimulationQuantitySelectionDTO();
      }
   }

   public class When_retrieving_the_simulation_and_name_of_a_null_simulation_quantity_selection_dto : concern_for_NullSimulationQuantitySelectionDTO
   {
      [Observation]
      public void should_return_an_invalid_object()
      {
         sut.SimulationPathElement.DisplayName.ShouldBeEqualTo(Captions.InvalidObject);
         sut.NamePathElement.DisplayName.ShouldBeEqualTo(Captions.InvalidObject);
      }
   }
}	