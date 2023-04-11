using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Converters.v9;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core
{
   public abstract class concern_for_SimpleSpecs : ContextWithLoadedSimulation<Converter730To90>
   {
      protected IModelCoreSimulation _simulation;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _simulation = LoadPKMLFile("simple").Simulation;
      }
   }

   public class When_loading_a_simulation_based_on_a_simple_configuration : concern_for_SimpleSpecs
   {
      [Observation]
      public void should_have_converted_the_outdated_dimensions_into_updated_dimensions()
      {
         var reaction = _simulation.Configuration.ModuleConfigurations[0].Module.Reactions.FindByName("R1").Parameter("k1");
         reaction.Dimension.Name.ShouldBeEqualTo("Inversed time");
      }
   }
}