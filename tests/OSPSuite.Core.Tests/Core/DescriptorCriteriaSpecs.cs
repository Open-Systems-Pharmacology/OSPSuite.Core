using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Extensions;
using FakeItEasy;
using NUnit.Framework;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Descriptors;

namespace OSPSuite.Core
{
   public abstract class concern_for_DescriptorCriteria : ContextSpecification<DescriptorCriteria>
   {
      protected Tags _tags;

      protected override void Context()
      {
         _tags = new Tags();
         sut = new DescriptorCriteria();
      }
   }

   public class When_checking_if_a_criteria_satifies_some_tags_that_are_not_satisfied_by_at_least_one_condition : concern_for_DescriptorCriteria
   {
      protected override void Context()
      {
         base.Context();
         var cond1 = A.Fake<IDescriptorCondition>();
         A.CallTo(() => cond1.IsSatisfiedBy(_tags)).Returns(true);
         var cond2 = A.Fake<IDescriptorCondition>();
         A.CallTo(() => cond2.IsSatisfiedBy(_tags)).Returns(false);
         sut.Add(cond1);
         sut.Add(cond2);
      }

      [Observation]
      public void should_return_that_the_criteria_does_not_satisify_the_tags()
      {
         sut.IsSatisfiedBy(_tags).ShouldBeFalse();
      }
   }

   public class When_checking_if_a_criteria_satifies_some_tags_that_are_satisfied_by_all_its_conditions : concern_for_DescriptorCriteria
   {
      protected override void Context()
      {
         base.Context();
         var cond1 = A.Fake<IDescriptorCondition>();
         A.CallTo(() => cond1.IsSatisfiedBy(_tags)).Returns(true);
         var cond2 = A.Fake<IDescriptorCondition>();
         A.CallTo(() => cond2.IsSatisfiedBy(_tags)).Returns(true);
         sut.Add(cond1);
         sut.Add(cond2);
      }

      [Observation]
      public void should_return_that_the_criteria_satisfies_the_tags()
      {
         sut.IsSatisfiedBy(_tags).ShouldBeTrue();
      }
   }

   public class When_checking_if_a_empty_criteria_satisfies_any_condition : concern_for_DescriptorCriteria
   {
      [Observation]
      public void should_return_false()
      {
         sut.IsSatisfiedBy(_tags).ShouldBeFalse();
      }
   }

   public class When_checking_if_an_entity_satisfies_a_criteria_with_the_entity_name : concern_for_DescriptorCriteria
   {
      private IEntity _entity;

      protected override void Context()
      {
         base.Context();
         _entity = new Parameter().WithName("para");
         var cond1 = new MatchTagCondition(_entity.Name);
         sut.Add(cond1);
      }

      [Observation]
      public void should_return_that_the_entity_satisfies_the_criteria()
      {
         sut.IsSatisfiedBy(_entity).ShouldBeTrue();
      }
   }

   public class When_comparing_two_descriptor_criteria_containg_the_same_descriptors : concern_for_DescriptorCriteria
   {
      private DescriptorCriteria _anotherDescriptor;
      private DescriptorCriteria _yetAnotherWithSameConditions;

      protected override void Context()
      {
         base.Context();
         sut.Add(new MatchTagCondition("toto"));
         sut.Add(new NotMatchTagCondition("toto"));
         sut.Add(new MatchTagCondition("titi"));
         sut.Add(new MatchAllCondition());

         _anotherDescriptor = new DescriptorCriteria();
         _anotherDescriptor.Add(new MatchTagCondition("toto"));
         _anotherDescriptor.Add(new NotMatchTagCondition("toto"));
         _anotherDescriptor.Add(new MatchTagCondition("titi"));
         _anotherDescriptor.Add(new MatchAllCondition());

         _yetAnotherWithSameConditions = new DescriptorCriteria();
         sut.Each(_yetAnotherWithSameConditions.Add);
      }

      [Observation]
      public void check_that_string_representation_is_accurate()
      {
         sut.ToString().ShouldBeEqualTo("toto AND NOT toto AND titi AND ALL");
      }

      [Observation]
      public void should_return_that_the_descriptor_criteria_are_equals()
      {
         //We want to check equality not reference. Use Nunit support directly
         Assert.AreEqual(sut, _anotherDescriptor);
         Assert.AreEqual(sut, _yetAnotherWithSameConditions);
      }
   }

   public class When_removing_a_condition_by_tag : concern_for_DescriptorCriteria
   {
      private MatchTagCondition _descriptorCondition;

      protected override void Context()
      {
         base.Context();
         sut.Add(new MatchTagCondition("toto"));
         _descriptorCondition = new MatchTagCondition("titi");
         sut.Add(_descriptorCondition);
      }

      protected override void Because()
      {
         sut.RemoveByTag<MatchTagCondition>("toto");
      }

      [Observation]
      public void should_have_removed_corresponding_condition()
      {
         sut.ShouldOnlyContain(_descriptorCondition);
      }
   }

   public class When_removing_a_match_all_condition : concern_for_DescriptorCriteria
   {
      private MatchTagCondition _descriptorCondition;

      protected override void Context()
      {
         base.Context();
         sut.Add(new MatchAllCondition());
         _descriptorCondition = new MatchTagCondition("tata");
         sut.Add(_descriptorCondition);
      }

      protected override void Because()
      {
         sut.RemoveByTag<MatchAllCondition>(Constants.ALL_TAG);
      }

      [Observation]
      public void should_have_removed_corresponding_condition()
      {
         sut.ShouldOnlyContain(_descriptorCondition);
      }
   }

   public class When_removing_a_condition_by_tag_for_another_type : concern_for_DescriptorCriteria
   {
      private MatchTagCondition _descriptorCondition;

      protected override void Context()
      {
         base.Context();
         _descriptorCondition = new MatchTagCondition("toto");
         sut.Add(_descriptorCondition);
         sut.Add(new MatchTagCondition("titi"));
      }

      protected override void Because()
      {
         sut.RemoveByTag<NotMatchTagCondition>("toto");
      }

      [Observation]
      public void should_not_remvoe_the_condition()
      {
         sut.ShouldContain(_descriptorCondition);
      }
   }
}