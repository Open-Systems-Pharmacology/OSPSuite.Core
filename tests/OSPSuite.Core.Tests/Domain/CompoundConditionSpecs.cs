using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_CompoundCondition : ContextSpecification<CompoundCondition>
   {
      protected EntityDescriptor _entityDescriptor;

      protected override void Context()
      {
         _entityDescriptor = new EntityDescriptor(new Parameter());
         sut = new CompoundCondition();
      }
   }

   public class When_evaluating_an_empty_compound_condition : concern_for_CompoundCondition
   {
      [Observation]
      public void should_not_be_satisfied()
      {
         //empty compound has no conditions; mirrors DescriptorCriteria's empty-criteria behaviour
         sut.IsSatisfiedBy(_entityDescriptor).ShouldBeFalse();
      }
   }

   public class When_evaluating_an_AND_compound_with_all_inner_satisfied : concern_for_CompoundCondition
   {
      protected override void Context()
      {
         base.Context();
         sut.Operator = CriteriaOperator.And;
         var firstInnerCondition = A.Fake<ITagCondition>();
         A.CallTo(() => firstInnerCondition.IsSatisfiedBy(_entityDescriptor)).Returns(true);
         var secondInnerCondition = A.Fake<ITagCondition>();
         A.CallTo(() => secondInnerCondition.IsSatisfiedBy(_entityDescriptor)).Returns(true);
         sut.Add(firstInnerCondition);
         sut.Add(secondInnerCondition);
      }

      [Observation]
      public void should_be_satisfied()
      {
         sut.IsSatisfiedBy(_entityDescriptor).ShouldBeTrue();
      }
   }

   public class When_evaluating_an_AND_compound_with_one_inner_unsatisfied : concern_for_CompoundCondition
   {
      protected override void Context()
      {
         base.Context();
         sut.Operator = CriteriaOperator.And;
         var firstInnerCondition = A.Fake<ITagCondition>();
         A.CallTo(() => firstInnerCondition.IsSatisfiedBy(_entityDescriptor)).Returns(true);
         var secondInnerCondition = A.Fake<ITagCondition>();
         A.CallTo(() => secondInnerCondition.IsSatisfiedBy(_entityDescriptor)).Returns(false);
         sut.Add(firstInnerCondition);
         sut.Add(secondInnerCondition);
      }

      [Observation]
      public void should_not_be_satisfied()
      {
         sut.IsSatisfiedBy(_entityDescriptor).ShouldBeFalse();
      }
   }

   public class When_evaluating_an_OR_compound_with_one_inner_satisfied : concern_for_CompoundCondition
   {
      protected override void Context()
      {
         base.Context();
         sut.Operator = CriteriaOperator.Or;
         var firstInnerCondition = A.Fake<ITagCondition>();
         A.CallTo(() => firstInnerCondition.IsSatisfiedBy(_entityDescriptor)).Returns(false);
         var secondInnerCondition = A.Fake<ITagCondition>();
         A.CallTo(() => secondInnerCondition.IsSatisfiedBy(_entityDescriptor)).Returns(true);
         sut.Add(firstInnerCondition);
         sut.Add(secondInnerCondition);
      }

      [Observation]
      public void should_be_satisfied()
      {
         sut.IsSatisfiedBy(_entityDescriptor).ShouldBeTrue();
      }
   }

   public class When_cloning_a_compound_condition : concern_for_CompoundCondition
   {
      private CompoundCondition _clonedCompound;

      protected override void Context()
      {
         base.Context();
         sut.Operator = CriteriaOperator.Or;
         sut.Add(new MatchTagCondition("VenousBlood"));
         sut.Add(new MatchTagCondition("Plasma"));
      }

      protected override void Because()
      {
         _clonedCompound = sut.CloneCondition().DowncastTo<CompoundCondition>();
      }

      [Observation]
      public void should_clone_the_operator()
      {
         _clonedCompound.Operator.ShouldBeEqualTo(CriteriaOperator.Or);
      }

      [Observation]
      public void should_deep_clone_the_inner_conditions()
      {
         _clonedCompound.Count.ShouldBeEqualTo(2);
         _clonedCompound[0].ShouldBeAnInstanceOf<MatchTagCondition>();
         _clonedCompound[0].DowncastTo<MatchTagCondition>().Tag.ShouldBeEqualTo("VenousBlood");
      }

      [Observation]
      public void clone_should_not_share_inner_list_with_original()
      {
         _clonedCompound.Add(new MatchTagCondition("Other"));
         sut.Count.ShouldBeEqualTo(2);
      }
   }

   public class When_replacing_a_keyword_in_a_compound_condition : concern_for_CompoundCondition
   {
      protected override void Context()
      {
         base.Context();
         sut.Add(new MatchTagCondition("VenousBlood"));
         sut.Add(new MatchTagCondition("Plasma"));
      }

      protected override void Because()
      {
         sut.Replace("VenousBlood", "ArterialBlood");
      }

      [Observation]
      public void should_replace_the_keyword_in_inner_conditions()
      {
         sut[0].DowncastTo<MatchTagCondition>().Tag.ShouldBeEqualTo("ArterialBlood");
         sut[1].DowncastTo<MatchTagCondition>().Tag.ShouldBeEqualTo("Plasma");
      }
   }

   public class When_rendering_a_compound_to_string : concern_for_CompoundCondition
   {
      protected override void Context()
      {
         base.Context();
         sut.Operator = CriteriaOperator.And;
         sut.Add(new MatchTagCondition("VenousBlood"));
         sut.Add(new MatchTagCondition("Plasma"));
      }

      [Observation]
      public void should_render_with_parentheses_around_the_inner_expression()
      {
         sut.Condition.ShouldBeEqualTo("(VenousBlood AND Plasma)");
      }
   }

   public class When_comparing_two_equivalent_compound_conditions : concern_for_CompoundCondition
   {
      private CompoundCondition _equivalentCompound;

      protected override void Context()
      {
         base.Context();
         sut.Operator = CriteriaOperator.Or;
         sut.Add(new MatchTagCondition("A"));
         sut.Add(new MatchTagCondition("B"));

         _equivalentCompound = new CompoundCondition { Operator = CriteriaOperator.Or };
         _equivalentCompound.Add(new MatchTagCondition("A"));
         _equivalentCompound.Add(new MatchTagCondition("B"));
      }

      [Observation]
      public void should_be_equal()
      {
         sut.Equals(_equivalentCompound).ShouldBeTrue();
      }

      [Observation]
      public void should_have_the_same_hash_code()
      {
         sut.GetHashCode().ShouldBeEqualTo(_equivalentCompound.GetHashCode());
      }
   }

   public class When_comparing_two_compound_conditions_with_different_operator : concern_for_CompoundCondition
   {
      private CompoundCondition _compoundWithDifferentOperator;

      protected override void Context()
      {
         base.Context();
         sut.Operator = CriteriaOperator.And;
         sut.Add(new MatchTagCondition("A"));

         _compoundWithDifferentOperator = new CompoundCondition { Operator = CriteriaOperator.Or };
         _compoundWithDifferentOperator.Add(new MatchTagCondition("A"));
      }

      [Observation]
      public void should_not_be_equal()
      {
         sut.Equals(_compoundWithDifferentOperator).ShouldBeFalse();
      }
   }

   public class When_comparing_a_compound_condition_to_a_plain_descriptor_criteria_with_same_contents : concern_for_CompoundCondition
   {
      private DescriptorCriteria _plainCriteriaWithSameContents;

      protected override void Context()
      {
         base.Context();
         sut.Operator = CriteriaOperator.And;
         sut.Add(new MatchTagCondition("A"));

         _plainCriteriaWithSameContents = new DescriptorCriteria { Operator = CriteriaOperator.And };
         _plainCriteriaWithSameContents.Add(new MatchTagCondition("A"));
      }

      [Observation]
      public void should_be_equal_because_they_match_the_same_criteria()
      {
         sut.Equals(_plainCriteriaWithSameContents).ShouldBeTrue();
         _plainCriteriaWithSameContents.Equals(sut).ShouldBeTrue();
      }
   }

   public class When_inspecting_the_tag_of_a_compound_condition : concern_for_CompoundCondition
   {
      [Observation]
      public void should_return_empty_string()
      {
         sut.Tag.ShouldBeEqualTo(string.Empty);
      }
   }
}
