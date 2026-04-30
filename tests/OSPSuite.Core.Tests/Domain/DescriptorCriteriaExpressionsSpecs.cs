using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain
{
   public class When_building_a_descriptor_criteria_with_a_compound : StaticContextSpecification
   {
      private DescriptorCriteria _criteria;

      protected override void Because()
      {
         _criteria = Create.Criteria(c => c
            .Compound(g => g.With("VenousBlood").And.With("Plasma"))
            .With(CriteriaOperator.Or)
            .Compound(g => g.With("Muscle").And.With("Interstitial")));
      }

      [Observation]
      public void should_set_the_outer_operator_to_or()
      {
         _criteria.Operator.ShouldBeEqualTo(CriteriaOperator.Or);
      }

      [Observation]
      public void should_contain_two_compound_conditions()
      {
         _criteria.Count.ShouldBeEqualTo(2);
         _criteria[0].ShouldBeAnInstanceOf<CompoundCondition>();
         _criteria[1].ShouldBeAnInstanceOf<CompoundCondition>();
      }

      [Observation]
      public void each_compound_should_default_to_AND_for_its_inner_operator()
      {
         _criteria[0].DowncastTo<CompoundCondition>().Operator.ShouldBeEqualTo(CriteriaOperator.And);
         _criteria[1].DowncastTo<CompoundCondition>().Operator.ShouldBeEqualTo(CriteriaOperator.And);
      }

      [Observation]
      public void should_populate_inner_conditions()
      {
         var firstCompoundInner = _criteria[0].DowncastTo<CompoundCondition>();
         firstCompoundInner.Count.ShouldBeEqualTo(2);
         firstCompoundInner[0].DowncastTo<MatchTagCondition>().Tag.ShouldBeEqualTo("VenousBlood");
         firstCompoundInner[1].DowncastTo<MatchTagCondition>().Tag.ShouldBeEqualTo("Plasma");
      }
   }

   public class When_building_a_compound_with_an_explicit_inner_operator : StaticContextSpecification
   {
      private DescriptorCriteria _criteria;

      protected override void Because()
      {
         _criteria = Create.Criteria(c => c
            .Compound(g => g.With("A").With(CriteriaOperator.Or).With("B")));
      }

      [Observation]
      public void should_set_the_inner_operator()
      {
         _criteria[0].DowncastTo<CompoundCondition>().Operator.ShouldBeEqualTo(CriteriaOperator.Or);
      }
   }

}
