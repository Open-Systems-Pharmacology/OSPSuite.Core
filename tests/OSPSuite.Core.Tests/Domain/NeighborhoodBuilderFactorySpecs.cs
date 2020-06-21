using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_NeighborhoodBuilderFactory : ContextSpecification<INeighborhoodBuilderFactory>
   {
      private IObjectBaseFactory _objectBaseFactory;

      protected override void Context()
      {
         _objectBaseFactory =new TestObjectBaseFactory();
         sut = new NeighborhoodBuilderFactory(_objectBaseFactory);
      }
   }

   

   
   public class When_creating_a_new_neigborhood_builder : concern_for_NeighborhoodBuilderFactory
   {
      private INeighborhoodBuilder _result;

      protected override void Because()
      {
         _result= sut.Create();
      }
      [Observation]
      public void should_create_a_new_neiighborhood_builder()
      {
         _result.ShouldNotBeNull();
      }
      [Observation]
      public void should_have_set_the_right_molecule_properties_container()
      {
         var resultContainer = _result.MoleculeProperties;
         resultContainer.ShouldNotBeNull();
         resultContainer.Mode.ShouldBeEqualTo(ContainerMode.Logical);
         resultContainer.Name.ShouldBeEqualTo(Constants.MOLECULE_PROPERTIES);
      }
   }

   
   class When_creating_a_neighborhood_between_two_containers : concern_for_NeighborhoodBuilderFactory
   {
      private IContainer _secondNeighbor;
      private IContainer _firstNeighbor;
      private INeighborhoodBuilder _result;

      protected override void Context()
      {
         base.Context();
         _firstNeighbor = A.Fake<IContainer>();
         _secondNeighbor = A.Fake<IContainer>();
      }

      protected override void Because()
      {
         _result = sut.CreateBetween(_firstNeighbor, _secondNeighbor);
      }

      [Observation]
      public void should_have_set_the_neighbors_right()
      {
         _result.FirstNeighbor.ShouldBeEqualTo(_firstNeighbor);      
         _result.SecondNeighbor.ShouldBeEqualTo(_secondNeighbor);
      }
   }
}	