using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core
{
   public abstract class concern_for_neighborhood_extensions : StaticContextSpecification
   {
      protected INeighborhood _neighborhood;

      protected override void Context()
      {
         _neighborhood = A.Fake<INeighborhood>();
      }
   }

   
   public class When_setting_the_first_neighbor_of_a_neighborhood_with_the_extension : concern_for_neighborhood_extensions
   {
      private IContainer _firstContainer;

      protected override void Context()
      {
         base.Context();
         _firstContainer = A.Fake<IContainer>();
      }

      protected override void Because()
      {
         _neighborhood.WithFirstNeighbor(_firstContainer);
      }

      [Observation]
      public void should_set_the_first_neighbor_in_the_neighborhood_to_the_given_container()
      {
         _neighborhood.FirstNeighbor.ShouldBeEqualTo(_firstContainer);
      }
   }

   public class When_setting_the_second_neighbor_of_a_neighborhood_with_the_extension : concern_for_neighborhood_extensions
   {
      private IContainer _firstContainer;

      protected override void Context()
      {
         base.Context();
         _firstContainer = A.Fake<IContainer>();
      }

      protected override void Because()
      {
         _neighborhood.WithSecondNeighbor(_firstContainer);
      }

      [Observation]
      public void should_set_the_second_neighbor_in_the_neighborhood_to_the_given_container()
      {
         _neighborhood.SecondNeighbor.ShouldBeEqualTo(_firstContainer);
      }
   }

}