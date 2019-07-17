using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Descriptors;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_MatchTagCondition : ContextSpecification<MatchTagCondition>
   {
      protected IDescriptorCondition _match;
      protected IDescriptorCondition _doNotMatch;
      protected EntityDescriptor _entityCriteria;

      protected override void Context()
      {
         var entity = new Parameter();
         entity.AddTag("tag1");
         entity.AddTag("tag2");
          _match = new MatchTagCondition("tag1");
         _doNotMatch = new MatchTagCondition("do not match");
         _entityCriteria = new EntityDescriptor(entity);
      }
   }

   public class When_checking_if_a_match_condition_matches_a_given_tag_set : concern_for_MatchTagCondition
   {
      [Observation]
      public void check_that_string_representation_is_accurate()
      {
         _match.ToString().ShouldBeEqualTo("tag1");
      }

      [Observation]
      public void should_return_true_if_the_tags_contain_the_matching_element()
      {
         _match.IsSatisfiedBy(_entityCriteria).ShouldBeTrue();
      }

      [Observation]
      public void should_return_false_if_the_tags_do_not_contain_the_matching_element()
      {
         _doNotMatch.IsSatisfiedBy(_entityCriteria).ShouldBeFalse();
      }
   }
}