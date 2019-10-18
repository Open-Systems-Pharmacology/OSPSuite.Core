using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Utility.Exceptions;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_neighborhood : ContextSpecification<INeighborhood>
   {
      protected IContainer _firstNeighbor;
      protected IContainer _secondNeighbor;

      protected override void Context()
      {
         sut = new Neighborhood();
         _firstNeighbor =A.Fake<IContainer>();
         _secondNeighbor=A.Fake<IContainer>();
         sut.FirstNeighbor = _firstNeighbor;
         sut.SecondNeighbor = _secondNeighbor;

      }
   }

   public class When_retrieving_the_neighboor_satisfying_a_criteria  : concern_for_neighborhood
   {
      private DescriptorCriteria _aNonExistingCriteria;
      private DescriptorCriteria _criteriaForSecondNeighbor;
      private DescriptorCriteria _criteriaForBothNeighbors;

      protected override void Context()
      {
         base.Context();
         _aNonExistingCriteria = A.Fake<DescriptorCriteria>();
         _criteriaForSecondNeighbor = A.Fake<DescriptorCriteria>();
         _criteriaForBothNeighbors = A.Fake<DescriptorCriteria>();
         A.CallTo(()=>_aNonExistingCriteria.IsSatisfiedBy(_firstNeighbor)).Returns(false);
         A.CallTo(()=>_aNonExistingCriteria.IsSatisfiedBy(_secondNeighbor)).Returns(false);
         A.CallTo(()=>_criteriaForSecondNeighbor.IsSatisfiedBy(_firstNeighbor)).Returns(false);
         A.CallTo(()=>_criteriaForSecondNeighbor.IsSatisfiedBy(_secondNeighbor)).Returns(true);
         A.CallTo(()=>_criteriaForBothNeighbors.IsSatisfiedBy(_firstNeighbor)).Returns(true);
         A.CallTo(() => _criteriaForBothNeighbors.IsSatisfiedBy(_secondNeighbor)).Returns(true);
      }
      [Observation]
      public void should_return_one_neighbor_satisfying_that_criteria_if_available()
      {
         sut.GetNeighborSatisfying(_criteriaForSecondNeighbor).ShouldBeEqualTo(_secondNeighbor);
      }

      [Observation]
      public void should_return_null_if_none_of_the_neighbor_in_the_neighborhood_matches_the_criteria()
      {
         sut.GetNeighborSatisfying(_aNonExistingCriteria).ShouldBeNull();

      }
      [Observation]
      public void should_throw_an_exception_if_both_neighbors_are_matching_the_criteria()
      {
         The.Action(() => sut.GetNeighborSatisfying(_criteriaForBothNeighbors)).ShouldThrowAn<OSPSuiteException>();
      }

   }

   public class When_checking_if_a_neighborhood_satifies_a_couple_of_criteria : concern_for_neighborhood
   {
      private DescriptorCriteria _aNonExistingCriteria;
      private DescriptorCriteria _criteriaForFirstNeighbor;
      private DescriptorCriteria _criteriaForSecondNeighbor;
      private DescriptorCriteria _criteriaForBothNeighbors;
      protected override void Context()
      {
         base.Context();
         _aNonExistingCriteria = A.Fake<DescriptorCriteria>();
         _criteriaForSecondNeighbor = A.Fake<DescriptorCriteria>();
         _criteriaForFirstNeighbor = A.Fake<DescriptorCriteria>();
         _criteriaForBothNeighbors = A.Fake<DescriptorCriteria>();
         A.CallTo(()=>_aNonExistingCriteria.IsSatisfiedBy(_firstNeighbor)).Returns(false);
         A.CallTo(()=>_aNonExistingCriteria.IsSatisfiedBy(_secondNeighbor)).Returns(false);
         A.CallTo(()=>_criteriaForSecondNeighbor.IsSatisfiedBy(_firstNeighbor)).Returns(false);
         A.CallTo(()=>_criteriaForSecondNeighbor.IsSatisfiedBy(_secondNeighbor)).Returns(true);
         A.CallTo(()=>_criteriaForFirstNeighbor.IsSatisfiedBy(_firstNeighbor)).Returns(true);
         A.CallTo(()=>_criteriaForFirstNeighbor.IsSatisfiedBy(_secondNeighbor)).Returns(false);
         A.CallTo(()=>_criteriaForBothNeighbors.IsSatisfiedBy(_firstNeighbor)).Returns(true);
         A.CallTo(() => _criteriaForBothNeighbors.IsSatisfiedBy(_secondNeighbor)).Returns(true);

      }
      [Observation]
      public void should_return_true_if_each_neigbhors_satisfies_at_least_one_criteria_but_not_the_same_as_the_other_neighbor()
      {
         sut.Satisfies(_criteriaForFirstNeighbor,_criteriaForSecondNeighbor).ShouldBeTrue();
         sut.Satisfies(_criteriaForSecondNeighbor, _criteriaForFirstNeighbor).ShouldBeTrue();
         sut.Satisfies(_criteriaForSecondNeighbor, _criteriaForBothNeighbors).ShouldBeTrue();
         sut.Satisfies(_criteriaForBothNeighbors, _criteriaForSecondNeighbor).ShouldBeTrue();
      }

      [Observation]
      public void should_return_false_if_both_neighbors_match_the_same_criteria_and_one_criteria_reamains_unmatch()
      {
         sut.Satisfies(_criteriaForBothNeighbors, _aNonExistingCriteria).ShouldBeFalse();
  
      }

      [Observation]
      public void should_return_false_if_one_neighbor_does_not_match_any_criteria()
      {
         sut.Satisfies(_aNonExistingCriteria, _criteriaForFirstNeighbor).ShouldBeFalse();
         sut.Satisfies(_aNonExistingCriteria, _criteriaForSecondNeighbor).ShouldBeFalse();

      }

   }


   
   public class When_checking_if_a_neighborhood_strictly_satifies_a_couple_of_criteria : concern_for_neighborhood
   {
      private DescriptorCriteria _aNonExistingCriteria;
      private DescriptorCriteria _criteriaForFirstNeighbor;
      private DescriptorCriteria _criteriaForSecondNeighbor;
      private DescriptorCriteria _criteriaForBothNeighbors;
      protected override void Context()
      {
         base.Context();
         _aNonExistingCriteria = A.Fake<DescriptorCriteria>();
         _criteriaForSecondNeighbor = A.Fake<DescriptorCriteria>();
         _criteriaForFirstNeighbor = A.Fake<DescriptorCriteria>();
         _criteriaForBothNeighbors = A.Fake<DescriptorCriteria>();
         A.CallTo(()=>_aNonExistingCriteria.IsSatisfiedBy(_firstNeighbor)).Returns(false);
         A.CallTo(()=>_aNonExistingCriteria.IsSatisfiedBy(_secondNeighbor)).Returns(false);
         A.CallTo(()=>_criteriaForSecondNeighbor.IsSatisfiedBy(_firstNeighbor)).Returns(false);
         A.CallTo(()=>_criteriaForSecondNeighbor.IsSatisfiedBy(_secondNeighbor)).Returns(true);
         A.CallTo(()=>_criteriaForFirstNeighbor.IsSatisfiedBy(_firstNeighbor)).Returns(true);
         A.CallTo(()=>_criteriaForFirstNeighbor.IsSatisfiedBy(_secondNeighbor)).Returns(false);
         A.CallTo(()=>_criteriaForBothNeighbors.IsSatisfiedBy(_firstNeighbor)).Returns(true);
         A.CallTo(() => _criteriaForBothNeighbors.IsSatisfiedBy(_secondNeighbor)).Returns(true);

      }
      [Observation]
      public void should_return_true_if_the_first_neighbor_satisfies_the_first_criteria_and_the_second_neighboor_the_second_criteria()
      {
         sut.StrictlySatisfies(_criteriaForFirstNeighbor, _criteriaForSecondNeighbor).ShouldBeTrue();
      }

      [Observation]
      public void should_return_false_if_the_first_neighbor_satisfies_the_second_criteria_and_the_second_neighbor_satisfies_the_first_criteria()
      {
         sut.StrictlySatisfies(_criteriaForSecondNeighbor, _criteriaForFirstNeighbor).ShouldBeFalse();

      }

      [Observation]
      public void should_return_false_if_one_neighbor_does_not_match_any_criteria()
      {
         sut.StrictlySatisfies(_criteriaForFirstNeighbor, _aNonExistingCriteria).ShouldBeFalse();
         sut.StrictlySatisfies(_aNonExistingCriteria, _criteriaForSecondNeighbor).ShouldBeFalse();
      }

   }
}	