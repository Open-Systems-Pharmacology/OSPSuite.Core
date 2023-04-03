using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Domain
{
   public class When_getting_all_application_parameters_on_educts_products_of_each_other : StaticContextSpecification
   {
      private ModelCoreSimulation _simulation;

      protected override void Context()
      {
         _simulation = new ModelCoreSimulation
         {
            Model = new Model
            {
               Root = new Container { new EventGroup() }
            },
            Configuration = new SimulationConfigurationForSpecs()
         };
         var reactionBuildingBlock = new ReactionBuildingBlock();
         var reactionAtoB = new ReactionBuilder();
         var reactionBtoA = new ReactionBuilder();
         var partnerA = new ReactionPartnerBuilder("A", 0);
         var partnerB = new ReactionPartnerBuilder("B", 0);

         //Making both reactions dependent on each other
         reactionAtoB.AddProduct(partnerA);
         reactionBtoA.AddProduct(partnerB);
         reactionAtoB.AddEduct(partnerB);
         reactionBtoA.AddEduct(partnerA);
         reactionBuildingBlock.Add(reactionAtoB);
         reactionBuildingBlock.Add(reactionBtoA);

         var module = new Module {Reaction = reactionBuildingBlock};
         var moduleConfiguration = new ModuleConfiguration(module);
         _simulation.Configuration.AddModuleConfiguration(moduleConfiguration);
      }

      [Observation]
      public void should_not_infinitely_recurse()
      {
         _simulation.AllApplicationParametersOrderedByStartTimeFor("A").ShouldNotBeNull();
      }
   }
}
