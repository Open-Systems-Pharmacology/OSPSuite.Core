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
      public void should_not_create_the_global_molecules_container()
      {
         _result.GlobalMoleculeDependentProperties.ShouldBeNull();
      }

      [Observation]
      public void should_set_the_neighborhoods_container_right()
      {
         _result.NeighborhoodsContainer.ShouldNotBeNull();
         _result.NeighborhoodsContainer.Name.ShouldBeEqualTo(Constants.NEIGHBORHOODS);
         _result.NeighborhoodsContainer.Mode.ShouldBeEqualTo(ContainerMode.Logical);
      }
   }

   public class When_creating_the_global_molecule_dependent_properties_container : concern_for_SpatialStructureFactory
   {
      private IContainer _result;

      protected override void Because()
      {
         _result = sut.CreateGlobalMoleculeDependentProperties();
      }

      [Observation]
      public void should_return_a_logical_molecule_container_named_molecule_properties()
      {
         _result.Name.ShouldBeEqualTo(Constants.MOLECULE_PROPERTIES);
         _result.Mode.ShouldBeEqualTo(ContainerMode.Logical);
         _result.ContainerType.ShouldBeEqualTo(ContainerType.Molecule);
      }
   }
}