using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_SpatialStructureFactory : ContextSpecification<ISpatialStructureFactory>
   {
      protected IObjectBaseFactory _objectBaseFactory;

      protected override void Context()
      {
         _objectBaseFactory = new TestObjectBaseFactory();
         sut = new SpatialStructureFactory(_objectBaseFactory);
      }
   }

   public class When_creating_a_new_SpatialStructure : concern_for_SpatialStructureFactory
   {
      private SpatialStructure _result;

      protected override void Because()
      {
         _result = sut.Create();
      }

      [Observation]
      public void the_result_should_have_default_name()
      {
         _result.Name.ShouldBeEqualTo(DefaultNames.SpatialStructure);
      }

      [Observation]
      public void should_return_a_proper_initialized_spatial_structure()
      {
         _result.ShouldNotBeNull();
      }

      [Observation]
      public void should_set_the_global_molecules_container_right()
      {
         _result.GlobalMoleculeDependentProperties.ShouldNotBeNull();
         _result.GlobalMoleculeDependentProperties.Name.ShouldBeEqualTo(Constants.MOLECULE_PROPERTIES);
         _result.GlobalMoleculeDependentProperties.Mode.ShouldBeEqualTo(ContainerMode.Logical);
      }

      [Observation]
      public void should_set_the_neighborhoods_container_right()
      {
         _result.NeighborhoodsContainer.ShouldNotBeNull();
         _result.NeighborhoodsContainer.Name.ShouldBeEqualTo(Constants.NEIGHBORHOODS);
         _result.NeighborhoodsContainer.Mode.ShouldBeEqualTo(ContainerMode.Logical);
      }
   }
}