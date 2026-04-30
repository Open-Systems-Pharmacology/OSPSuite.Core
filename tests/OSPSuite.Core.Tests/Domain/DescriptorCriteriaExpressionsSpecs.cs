using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain
{
   public class When_building_a_descriptor_criteria_with_a_condition_group : StaticContextSpecification
   {
      private DescriptorCriteria _criteria;

      protected override void Because()
      {
         _criteria = Create.Criteria(c => c
            .ConditionGroup(g => g.With("VenousBlood").And.With("Plasma"))
            .With(CriteriaOperator.Or)
            .ConditionGroup(g => g.With("Muscle").And.With("Interstitial")));
      }

      [Observation]
      public void should_set_the_outer_operator_to_or()
      {
         _criteria.Operator.ShouldBeEqualTo(CriteriaOperator.Or);
      }

      [Observation]
      public void should_contain_two_condition_groups()
      {
         _criteria.Count.ShouldBeEqualTo(2);
         _criteria[0].ShouldBeAnInstanceOf<ConditionGroup>();
         _criteria[1].ShouldBeAnInstanceOf<ConditionGroup>();
      }

      [Observation]
      public void each_group_should_default_to_AND_for_its_inner_operator()
      {
         _criteria[0].DowncastTo<ConditionGroup>().Operator.ShouldBeEqualTo(CriteriaOperator.And);
         _criteria[1].DowncastTo<ConditionGroup>().Operator.ShouldBeEqualTo(CriteriaOperator.And);
      }

      [Observation]
      public void should_populate_inner_conditions()
      {
         var firstGroupInner = _criteria[0].DowncastTo<ConditionGroup>();
         firstGroupInner.Count.ShouldBeEqualTo(2);
         firstGroupInner[0].DowncastTo<MatchTagCondition>().Tag.ShouldBeEqualTo("VenousBlood");
         firstGroupInner[1].DowncastTo<MatchTagCondition>().Tag.ShouldBeEqualTo("Plasma");
      }
   }

   public class When_building_a_condition_group_with_an_explicit_inner_operator : StaticContextSpecification
   {
      private DescriptorCriteria _criteria;

      protected override void Because()
      {
         _criteria = Create.Criteria(c => c
            .ConditionGroup(g => g.With("A").With(CriteriaOperator.Or).With("B")));
      }

      [Observation]
      public void should_set_the_inner_operator()
      {
         _criteria[0].DowncastTo<ConditionGroup>().Operator.ShouldBeEqualTo(CriteriaOperator.Or);
      }
   }

}
