using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_NeighborhoodBuilder : ContextSpecification<NeighborhoodBuilder>
   {
      protected override void Context()
      {
         sut = new NeighborhoodBuilder();
      }
   }

   public class When_setting_the_first_neighbor_and_second_neighbor_path : concern_for_NeighborhoodBuilder
   {
      protected override void Context()
      {
         base.Context();
         sut.FirstNeighborPath = new ObjectPath("root", "first");
         sut.SecondNeighborPath = new ObjectPath("root", "second");

         var root = new Container {Name = "root"};
         var first = new Container {Name = "first"}.Under(root);
         var second = new Container {Name = "second"}.Under(root);
         sut.ResolveReference(new[] {root});
         sut.FirstNeighbor.ShouldNotBeNull();
         sut.SecondNeighbor.ShouldNotBeNull();
      }

      [Observation]
      public void should_reset_the_reference_to_first_and_second_neighbor()
      {
         sut.FirstNeighborPath = new ObjectPath("root", "xxx");
         sut.FirstNeighbor.ShouldBeNull();
         sut.SecondNeighbor.ShouldNotBeNull();

         sut.SecondNeighborPath = new ObjectPath("root", "xxx");
         sut.SecondNeighbor.ShouldBeNull();
      }
   }
}