using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Descriptors;

namespace OSPSuite.Core
{
   public abstract class concern_for_MatchAllCondition : ContextSpecification<MatchAllCondition>
   {
      protected IDescriptorCondition _match;
      protected IDescriptorCondition _doNotMatch;
      protected EntityDescriptor _entityCriteria;

      protected override void Context()
      {
         var entity = new Parameter();
         entity.AddTag("tag1");
         entity.AddTag("tag2");

         _entityCriteria = new EntityDescriptor(entity);
         sut = new MatchAllCondition();
      }
   }

   public class When_checking_if_a_match_all_matches_a_given_tag_set : concern_for_MatchAllCondition
   {
      [Observation]
      public void check_that_string_representation_is_accurate()
      {
         sut.ToString().ShouldBeEqualTo("ALL");
      }

      [Observation]
      public void should_always_return_true_if_the_tags_contain_the_matching_element()
      {
         sut.IsSatisfiedBy(_entityCriteria).ShouldBeTrue();
      }
   }
}