using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Descriptors;

namespace OSPSuite.Core
{
   public abstract class concern_for_MatchTagCondition : ContextSpecification<IDescriptorCondition>
   {
      protected Tags _tags;
      protected IDescriptorCondition _match;
      protected IDescriptorCondition _doNotMatch;

      protected override void Context()
      {
         _tags = new Tags();
         _tags.Add(new Tag {Value = "tag1"});
         _tags.Add(new Tag {Value = "tag2"});
         _match = new MatchTagCondition("tag1");
         _doNotMatch = new MatchTagCondition("do not match");
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
         _match.IsSatisfiedBy(_tags).ShouldBeTrue();
      }

      [Observation]
      public void should_return_false_if_the_tags_do_not_contain_the_matching_element()
      {
         _doNotMatch.IsSatisfiedBy(_tags).ShouldBeFalse();
      }
   }
}