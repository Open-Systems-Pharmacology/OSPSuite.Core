using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_SimulationQuantitySelection : ContextSpecification<SimulationQuantitySelection>
   {
      private QuantitySelection _quantitySelection;
      private ISimulation _simulation;

      protected override void Context()
      {
         _quantitySelection = new QuantitySelection("A|B", QuantityType.Drug);
         _simulation = A.Fake<ISimulation>().WithName("S");
         sut = new SimulationQuantitySelection(_simulation,_quantitySelection);
      }
   }

   public class When_retrieving_the_full_path_of_a_simulation_quantity_selection : concern_for_SimulationQuantitySelection
   {
      [Observation]
      public void should_return_the_path_of_the_quantity_selection_with_the_name_of_the_simulation_as_the_beginning()
      {
         sut.FullQuantityPath.ShouldBeEqualTo("S|A|B");
      }
   }
}	