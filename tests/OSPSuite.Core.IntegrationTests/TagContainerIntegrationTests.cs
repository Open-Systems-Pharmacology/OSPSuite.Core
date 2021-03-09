using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core
{
   public class When_loading_a_simulation_with_reaction_tags_defined_as_in_container : ContextForModelConstructorIntegration
   {
      private IModel _model;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _model = CreateFrom("tag_reactions").Model;
      }

      [Observation]
      public void should_create_the_simulation_at_the_expected_location()
      {
         var intracellular = _model.Root.EntityAt<Container>("Organism", "Liver", "Intracellular");
         intracellular.Container("MatchTag").ShouldNotBeNull();
         intracellular.Container("InContainer").ShouldNotBeNull();
      }
   }
}