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

   public class When_checking_the_neighbor_paths_of_a_neighborhood_builder_without_neighbors : concern_for_NeighborhoodBuilder
   {
      [Observation]
      public void should_have_no_neighbors()
      {
         sut.HasNoNeighbors.ShouldBeTrue();
      }

      [Observation]
      public void should_not_have_defined_neighbor_paths()
      {
         sut.HasDefinedNeighborPaths.ShouldBeFalse();
      }

      [Observation]
      public void the_neighbor_paths_should_never_be_null()
      {
         sut.FirstNeighborPath.ShouldNotBeNull();
         sut.SecondNeighborPath.ShouldNotBeNull();
      }
   }

   public class When_setting_a_neighbor_path_to_null : concern_for_NeighborhoodBuilder
   {
      protected override void Because()
      {
         sut.FirstNeighborPath = null;
         sut.SecondNeighborPath = null;
      }

      [Observation]
      public void the_neighbor_paths_should_be_replaced_by_empty_paths()
      {
         sut.FirstNeighborPath.ShouldNotBeNull();
         sut.SecondNeighborPath.ShouldNotBeNull();
      }

      [Observation]
      public void the_neighborhood_builder_should_have_no_neighbors()
      {
         sut.HasNoNeighbors.ShouldBeTrue();
      }
   }

   public class When_checking_the_neighbor_paths_of_a_neighborhood_builder_with_empty_neighbor_paths : concern_for_NeighborhoodBuilder
   {
      protected override void Context()
      {
         base.Context();
         sut.FirstNeighborPath = new ObjectPath();
         sut.SecondNeighborPath = new ObjectPath();
      }

      [Observation]
      public void should_have_no_neighbors()
      {
         sut.HasNoNeighbors.ShouldBeTrue();
      }

      [Observation]
      public void should_not_have_defined_neighbor_paths()
      {
         sut.HasDefinedNeighborPaths.ShouldBeFalse();
      }
   }

   public class When_checking_the_neighbor_paths_of_a_neighborhood_builder_with_only_one_neighbor : concern_for_NeighborhoodBuilder
   {
      protected override void Context()
      {
         base.Context();
         sut.FirstNeighborPath = new ObjectPath("root", "first");
      }

      [Observation]
      public void should_not_have_no_neighbors()
      {
         sut.HasNoNeighbors.ShouldBeFalse();
      }

      [Observation]
      public void should_not_have_defined_neighbor_paths()
      {
         sut.HasDefinedNeighborPaths.ShouldBeFalse();
      }
   }

   public class When_checking_the_neighbor_paths_of_a_neighborhood_builder_with_two_neighbors : concern_for_NeighborhoodBuilder
   {
      protected override void Context()
      {
         base.Context();
         sut.FirstNeighborPath = new ObjectPath("root", "first");
         sut.SecondNeighborPath = new ObjectPath("root", "second");
      }

      [Observation]
      public void should_not_have_no_neighbors()
      {
         sut.HasNoNeighbors.ShouldBeFalse();
      }

      [Observation]
      public void should_have_defined_neighbor_paths()
      {
         sut.HasDefinedNeighborPaths.ShouldBeTrue();
      }
   }

   public class When_resolving_the_references_of_a_neighborhood_builder_without_neighbors : concern_for_NeighborhoodBuilder
   {
      private Container _root;

      protected override void Context()
      {
         base.Context();
         _root = new Container {Name = "root"};
      }

      protected override void Because()
      {
         sut.ResolveReference(new[] {_root});
      }

      [Observation]
      public void should_not_resolve_any_neighbor()
      {
         sut.FirstNeighbor.ShouldBeNull();
         sut.SecondNeighbor.ShouldBeNull();
      }
   }

   public class When_resolving_the_references_of_a_neighborhood_builder_with_empty_neighbor_paths : concern_for_NeighborhoodBuilder
   {
      private Container _root;

      protected override void Context()
      {
         base.Context();
         sut.FirstNeighborPath = new ObjectPath();
         sut.SecondNeighborPath = new ObjectPath();
         _root = new Container {Name = "root"};
      }

      protected override void Because()
      {
         sut.ResolveReference(new[] {_root});
      }

      [Observation]
      public void should_not_resolve_the_neighbors_to_the_container_itself()
      {
         sut.FirstNeighbor.ShouldBeNull();
         sut.SecondNeighbor.ShouldBeNull();
      }
   }

   public class When_updating_a_neighborhood_builder_from_a_neighborhood_builder_without_neighbors : concern_for_NeighborhoodBuilder
   {
      private NeighborhoodBuilder _sourceNeighborhoodBuilder;

      protected override void Context()
      {
         base.Context();
         _sourceNeighborhoodBuilder = new NeighborhoodBuilder();
      }

      protected override void Because()
      {
         sut.UpdatePropertiesFrom(_sourceNeighborhoodBuilder, null);
      }

      [Observation]
      public void should_also_have_no_neighbors()
      {
         sut.HasNoNeighbors.ShouldBeTrue();
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