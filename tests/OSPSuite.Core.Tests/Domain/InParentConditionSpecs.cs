﻿using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Descriptors;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_InParentCondition : ContextSpecification<InParentCondition>
   {
      protected EntityDescriptor _entityCriteria;

      protected override void Context()
      {
         var entity = new Parameter();
         entity.AddTag("tag1");
         entity.AddTag("tag2");

         _entityCriteria = new EntityDescriptor(entity);
         sut = new InParentCondition();
      }
   }

   public class When_checking_if_a_match_parent_matches_a_given_tag_set : concern_for_InParentCondition
   {
      [Observation]
      public void check_that_string_representation_is_accurate()
      {
         sut.ToString().ShouldBeEqualTo("IN PARENT");
      }

      [Observation]
      public void should_always_return_false_if_the_tags_contain_the_matching_element()
      {
         sut.IsSatisfiedBy(_entityCriteria).ShouldBeFalse();
      }
   }
}