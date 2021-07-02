using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OSPSuite.Core.Domain
{
   public class When_getting_all_application_parameters_on_educts_products_of_each_other : StaticContextSpecification
   {
      private ModelCoreSimulation _simulation;

      protected override void Context()
      {
         _simulation = new ModelCoreSimulation();
         var root = A.Fake<IContainer>();
         A.CallTo(() => root.GetChildren<IEventGroup>()).Returns(new List<IEventGroup>() { A.Fake<IEventGroup>() });
         _simulation.Model = A.Fake<IModel>();
         A.CallTo(() => _simulation.Model.Root).Returns(root);
         _simulation.BuildConfiguration = A.Fake<IBuildConfiguration>();
         var reactionBuildingBlock = new ReactionBuildingBlock();
         var reactionA = A.Fake<IReactionBuilder>();
         var reactionB = A.Fake<IReactionBuilder>();
         var partnerA = A.Fake<IReactionPartnerBuilder>();
         A.CallTo(() => partnerA.MoleculeName).Returns("A");
         var partnerB = A.Fake<IReactionPartnerBuilder>();
         A.CallTo(() => partnerB.MoleculeName).Returns("B");
         IEnumerable<IReactionPartnerBuilder> enumContainingA = new List<IReactionPartnerBuilder>() { partnerB };
         IEnumerable<IReactionPartnerBuilder> enumContainingB = new List<IReactionPartnerBuilder>() { partnerA };
         A.CallTo(() => reactionA.Products).Returns(enumContainingB);
         A.CallTo(() => reactionB.Products).Returns(enumContainingA);
         A.CallTo(() => reactionA.Educts).Returns(enumContainingA);
         A.CallTo(() => reactionB.Educts).Returns(enumContainingB);
         reactionBuildingBlock.Add(reactionA);
         reactionBuildingBlock.Add(reactionB);
         A.CallTo(() => _simulation.BuildConfiguration.Reactions).Returns(reactionBuildingBlock);
      }

      [Observation]
      public void should_not_infinitely_recurse()
      {
         _simulation.AllApplicationParametersOrderedByStartTimeFor("A").ShouldNotBeNull();
      }
   }
}
