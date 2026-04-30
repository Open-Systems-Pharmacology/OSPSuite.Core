using FakeItEasy;
using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_DescriptorCriteria : ContextSpecification<DescriptorCriteria>
   {
      protected EntityDescriptor _entityCriteria;
      private Parameter _entity;

      protected override void Context()
      {
         _entity = new Parameter();
         _entityCriteria = new EntityDescriptor(_entity);
         sut = new DescriptorCriteria();
      }
   }

   public class When_checking_if_a_criteria_satisfies_some_tags_that_are_not_satisfied_by_at_least_one_condition : concern_for_DescriptorCriteria
   {
      protected override void Context()
      {
         base.Context();
         var cond1 = A.Fake<ITagCondition>();
         A.CallTo(() => cond1.IsSatisfiedBy(_entityCriteria)).Returns(true);
         var cond2 = A.Fake<ITagCondition>();
         A.CallTo(() => cond2.IsSatisfiedBy(_entityCriteria)).Returns(false);
         sut.Add(cond1);
         sut.Add(cond2);
      }

      [Observation]
      public void should_return_that_the_criteria_does_not_satisfy_the_tags()
      {
         sut.IsSatisfiedBy(_entityCriteria).ShouldBeFalse();
      }
   }

   public class When_cloning_a_descriptor_criteria : concern_for_DescriptorCriteria
   {
      private DescriptorCriteria _clone;

      protected override void Context()
      {
         base.Context();
         var cond1 = new InContainerCondition("TOTO");
         sut.Add(cond1);
         sut.Operator = CriteriaOperator.Or;
      }

      protected override void Because()
      {
         _clone = sut.Clone();
      }

      [Observation]
      public void should_clone_the_operator()
      {
         _clone.Operator.ShouldBeEqualTo(CriteriaOperator.Or);
      }

      [Observation]
      public void should_clone_the_conditions()
      {
         _clone.Count.ShouldBeEqualTo(1);
         _clone[0].ShouldBeAnInstanceOf<InContainerCondition>();
         _clone[0].DowncastTo<InContainerCondition>().Tag.ShouldBeEqualTo("TOTO");
      }
   }

   public class When_checking_if_a_criteria_satisfies_some_tags_that_are_not_satisfied_by_at_least_one_condition_and_the_operator_is_OR : concern_for_DescriptorCriteria
   {
      protected override void Context()
      {
         base.Context();
         var cond1 = A.Fake<ITagCondition>();
         A.CallTo(() => cond1.IsSatisfiedBy(_entityCriteria)).Returns(true);
         var cond2 = A.Fake<ITagCondition>();
         A.CallTo(() => cond2.IsSatisfiedBy(_entityCriteria)).Returns(false);
         sut.Add(cond1);
         sut.Add(cond2);
         sut.Operator = CriteriaOperator.Or;
      }

      [Observation]
      public void should_return_that_the_criteria_does_satisfy_the_tags()
      {
         sut.IsSatisfiedBy(_entityCriteria).ShouldBeTrue();
      }
   }

   public class When_checking_if_a_criteria_satisfies_some_tags_that_are_satisfied_by_all_its_conditions : concern_for_DescriptorCriteria
   {
      protected override void Context()
      {
         base.Context();
         var cond1 = A.Fake<ITagCondition>();
         A.CallTo(() => cond1.IsSatisfiedBy(_entityCriteria)).Returns(true);
         var cond2 = A.Fake<ITagCondition>();
         A.CallTo(() => cond2.IsSatisfiedBy(_entityCriteria)).Returns(true);
         sut.Add(cond1);
         sut.Add(cond2);
      }

      [Observation]
      public void should_return_that_the_criteria_satisfies_the_tags()
      {
         sut.IsSatisfiedBy(_entityCriteria).ShouldBeTrue();
      }
   }

   public class When_checking_if_a_empty_criteria_satisfies_any_condition : concern_for_DescriptorCriteria
   {
      [Observation]
      public void should_return_false()
      {
         sut.IsSatisfiedBy(_entityCriteria).ShouldBeFalse();
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

   public class When_checking_if_an_entity_satisfies_a_criteria_with_the_container_name : concern_for_DescriptorCriteria
   {
      private IEntity _entity;
      private IContainer _container;

      protected override void Context()
      {
         base.Context();
         _container = new Container().WithName("Liver");
         _entity = new Parameter().WithName("para").WithParentContainer(_container);
         var cond1 = new InContainerCondition(_container.Name);
         sut.Add(cond1);
      }

      [Observation]
      public void should_return_that_the_entity_satisfies_the_criteria()
      {
         sut.IsSatisfiedBy(_entity).ShouldBeTrue();
      }
   }

   public class When_checking_if_an_entity_without_a_parent_container_satisfies_a_criteria_with_the_container_name : concern_for_DescriptorCriteria
   {
      private IEntity _entity;

      protected override void Context()
      {
         base.Context();
         _entity = new Parameter().WithName("para");
         var cond1 = new InContainerCondition(_entity.Name);
         sut.Add(cond1);
      }

      [Observation]
      public void should_return_that_the_entity_does_satisfy_the_criteria()
      {
         sut.IsSatisfiedBy(_entity).ShouldBeTrue();
      }
   }

   public class When_comparing_two_descriptor_criteria_containing_the_same_descriptors : concern_for_DescriptorCriteria
   {
      private DescriptorCriteria _anotherDescriptor;
      private DescriptorCriteria _yetAnotherWithSameConditions;

      protected override void Context()
      {
         base.Context();
         sut.Add(new MatchTagCondition("toto"));
         sut.Add(new NotMatchTagCondition("toto"));
         sut.Add(new MatchTagCondition("titi"));
         sut.Add(new InContainerCondition("Liver"));
         sut.Add(new MatchAllCondition());

         _anotherDescriptor = new DescriptorCriteria();
         _anotherDescriptor.Add(new MatchTagCondition("toto"));
         _anotherDescriptor.Add(new NotMatchTagCondition("toto"));
         _anotherDescriptor.Add(new MatchTagCondition("titi"));
         _anotherDescriptor.Add(new InContainerCondition("Liver"));
         _anotherDescriptor.Add(new MatchAllCondition());

         _yetAnotherWithSameConditions = new DescriptorCriteria();
         sut.Each(_yetAnotherWithSameConditions.Add);
      }

      [Observation]
      public void check_that_string_representation_is_accurate()
      {
         sut.ToString().ShouldBeEqualTo("toto AND NOT toto AND titi AND IN CONTAINER Liver AND ALL");
      }

      [Observation]
      public void should_return_that_the_descriptor_criteria_are_equals()
      {
         //We want to check equality not reference. Use Nunit support directly
         Assert.AreEqual(sut, _anotherDescriptor);
         Assert.AreEqual(sut, _yetAnotherWithSameConditions);
      }
   }

   public class When_comparing_two_descriptor_criteria_containing_the_same_descriptors_but_different_operators : concern_for_DescriptorCriteria
   {
      private DescriptorCriteria _anotherDescriptor;

      protected override void Context()
      {
         base.Context();
         sut.Add(new MatchTagCondition("toto"));
         sut.Add(new NotMatchTagCondition("toto"));
         sut.Add(new MatchTagCondition("titi"));
         sut.Add(new InContainerCondition("Liver"));
         sut.Add(new MatchAllCondition());

         _anotherDescriptor = new DescriptorCriteria {Operator = CriteriaOperator.Or};
         _anotherDescriptor.Add(new MatchTagCondition("toto"));
         _anotherDescriptor.Add(new NotMatchTagCondition("toto"));
         _anotherDescriptor.Add(new MatchTagCondition("titi"));
         _anotherDescriptor.Add(new InContainerCondition("Liver"));
         _anotherDescriptor.Add(new MatchAllCondition());
      }

      [Observation]
      public void should_return_that_the_descriptor_criteria_are_not_equals()
      {
         sut.Equals(_anotherDescriptor).ShouldBeFalse();
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
      private InContainerCondition _inContainerCondition;

      protected override void Context()
      {
         base.Context();
         _descriptorCondition = new MatchTagCondition("toto");
         sut.Add(_descriptorCondition);
         _inContainerCondition = new InContainerCondition(_descriptorCondition.Tag);
         sut.Add(_inContainerCondition);
         sut.Add(new MatchTagCondition("titi"));
      }

      protected override void Because()
      {
         sut.RemoveByTag<NotMatchTagCondition>("toto");
      }

      [Observation]
      public void should_not_remove_the_condition()
      {
         sut.ShouldContain(_descriptorCondition);
         sut.ShouldContain(_inContainerCondition);
      }
   }

   public abstract class concern_for_DescriptorCriteria_with_compound : ContextSpecification<DescriptorCriteria>
   {
      protected override void Context()
      {
         //sut models: (VenousBlood AND Plasma) OR (Muscle AND Interstitial)
         sut = new DescriptorCriteria { Operator = CriteriaOperator.Or };
         sut.Add(buildCompound(CriteriaOperator.And, new MatchTagCondition("VenousBlood"), new MatchTagCondition("Plasma")));
         sut.Add(buildCompound(CriteriaOperator.And, new MatchTagCondition("Muscle"), new MatchTagCondition("Interstitial")));
      }

      protected static CompoundCondition buildCompound(CriteriaOperator innerOperator, params ITagCondition[] innerConditions)
      {
         var compound = new CompoundCondition { Operator = innerOperator };
         foreach (var condition in innerConditions)
            compound.Add(condition);
         return compound;
      }

      protected IEntity entityWithTags(params string[] tags)
      {
         var entity = new Parameter().WithName("para");
         foreach (var tag in tags)
            entity.AddTag(tag);
         return entity;
      }
   }

   public class When_outer_OR_of_two_AND_compounds_matches_the_first_branch : concern_for_DescriptorCriteria_with_compound
   {
      [Observation]
      public void should_satisfy_for_an_entity_tagged_VenousBlood_and_Plasma()
      {
         sut.IsSatisfiedBy(entityWithTags("VenousBlood", "Plasma")).ShouldBeTrue();
      }
   }

   public class When_outer_OR_of_two_AND_compounds_matches_the_second_branch : concern_for_DescriptorCriteria_with_compound
   {
      [Observation]
      public void should_satisfy_for_an_entity_tagged_Muscle_and_Interstitial()
      {
         sut.IsSatisfiedBy(entityWithTags("Muscle", "Interstitial")).ShouldBeTrue();
      }
   }

   public class When_outer_OR_of_two_AND_compounds_matches_neither_branch : concern_for_DescriptorCriteria_with_compound
   {
      [Observation]
      public void should_not_satisfy_for_an_entity_with_one_tag_from_each_branch()
      {
         //hybrid: matches one tag from each branch, neither branch fully
         sut.IsSatisfiedBy(entityWithTags("VenousBlood", "Interstitial")).ShouldBeFalse();
      }
   }

   public class When_outer_AND_with_a_compound_OR_branch_satisfied : concern_for_DescriptorCriteria
   {
      protected override void Context()
      {
         base.Context();
         sut.Operator = CriteriaOperator.And;
         sut.Add(A.Fake<ITagCondition>());
         A.CallTo(() => sut[0].IsSatisfiedBy(_entityCriteria)).Returns(true);

         var compound = new CompoundCondition { Operator = CriteriaOperator.Or };
         var firstInnerCondition = A.Fake<ITagCondition>();
         A.CallTo(() => firstInnerCondition.IsSatisfiedBy(_entityCriteria)).Returns(false);
         var secondInnerCondition = A.Fake<ITagCondition>();
         A.CallTo(() => secondInnerCondition.IsSatisfiedBy(_entityCriteria)).Returns(true);
         compound.Add(firstInnerCondition);
         compound.Add(secondInnerCondition);
         sut.Add(compound);
      }

      [Observation]
      public void should_be_satisfied()
      {
         sut.IsSatisfiedBy(_entityCriteria).ShouldBeTrue();
      }
   }

   public class When_outer_AND_with_a_compound_OR_branch_all_unsatisfied : concern_for_DescriptorCriteria
   {
      protected override void Context()
      {
         base.Context();
         sut.Operator = CriteriaOperator.And;
         sut.Add(A.Fake<ITagCondition>());
         A.CallTo(() => sut[0].IsSatisfiedBy(_entityCriteria)).Returns(true);

         var compound = new CompoundCondition { Operator = CriteriaOperator.Or };
         var firstInnerCondition = A.Fake<ITagCondition>();
         A.CallTo(() => firstInnerCondition.IsSatisfiedBy(_entityCriteria)).Returns(false);
         var secondInnerCondition = A.Fake<ITagCondition>();
         A.CallTo(() => secondInnerCondition.IsSatisfiedBy(_entityCriteria)).Returns(false);
         compound.Add(firstInnerCondition);
         compound.Add(secondInnerCondition);
         sut.Add(compound);
      }

      [Observation]
      public void should_not_be_satisfied()
      {
         sut.IsSatisfiedBy(_entityCriteria).ShouldBeFalse();
      }
   }

   public class When_cloning_a_descriptor_criteria_containing_a_compound : concern_for_DescriptorCriteria
   {
      private DescriptorCriteria _clonedCriteria;

      protected override void Context()
      {
         base.Context();
         sut.Operator = CriteriaOperator.Or;
         var compound = new CompoundCondition();
         compound.Add(new MatchTagCondition("VenousBlood"));
         compound.Add(new MatchTagCondition("Plasma"));
         sut.Add(compound);
      }

      protected override void Because()
      {
         _clonedCriteria = sut.Clone();
      }

      [Observation]
      public void should_deep_clone_the_compound()
      {
         _clonedCriteria.Count.ShouldBeEqualTo(1);
         _clonedCriteria[0].ShouldBeAnInstanceOf<CompoundCondition>();
         var clonedCompound = _clonedCriteria[0].DowncastTo<CompoundCondition>();
         clonedCompound.Operator.ShouldBeEqualTo(CriteriaOperator.And);
         clonedCompound.Count.ShouldBeEqualTo(2);
      }

      [Observation]
      public void clone_should_not_share_inner_list()
      {
         _clonedCriteria[0].DowncastTo<CompoundCondition>().Add(new MatchTagCondition("Other"));
         sut[0].DowncastTo<CompoundCondition>().Count.ShouldBeEqualTo(2);
      }
   }

   public class When_replacing_a_keyword_in_a_descriptor_criteria_containing_a_compound : concern_for_DescriptorCriteria
   {
      protected override void Context()
      {
         base.Context();
         var compound = new CompoundCondition();
         compound.Add(new MatchTagCondition("VenousBlood"));
         compound.Add(new MatchTagCondition("Plasma"));
         sut.Add(compound);
         sut.Add(new MatchTagCondition("VenousBlood"));
      }

      protected override void Because()
      {
         sut.Replace("VenousBlood", "ArterialBlood");
      }

      [Observation]
      public void should_replace_inside_the_compound()
      {
         var compound = sut[0].DowncastTo<CompoundCondition>();
         compound[0].DowncastTo<MatchTagCondition>().Tag.ShouldBeEqualTo("ArterialBlood");
      }

      [Observation]
      public void should_replace_at_the_outer_level()
      {
         sut[1].DowncastTo<MatchTagCondition>().Tag.ShouldBeEqualTo("ArterialBlood");
      }
   }

   public class When_rendering_a_descriptor_criteria_with_compound_to_string : concern_for_DescriptorCriteria
   {
      protected override void Context()
      {
         base.Context();
         sut.Operator = CriteriaOperator.Or;

         var venousPlasma = new CompoundCondition();
         venousPlasma.Add(new MatchTagCondition("VenousBlood"));
         venousPlasma.Add(new MatchTagCondition("Plasma"));

         var muscleInterstitial = new CompoundCondition();
         muscleInterstitial.Add(new MatchTagCondition("Muscle"));
         muscleInterstitial.Add(new MatchTagCondition("Interstitial"));

         sut.Add(venousPlasma);
         sut.Add(muscleInterstitial);
      }

      [Observation]
      public void should_render_outer_OR_of_parenthesised_compound_groups()
      {
         sut.ToString().ShouldBeEqualTo("(VenousBlood AND Plasma) OR (Muscle AND Interstitial)");
      }
   }

   public class When_comparing_two_descriptor_criteria_with_equivalent_compounds : concern_for_DescriptorCriteria
   {
      private DescriptorCriteria _equivalentCriteria;

      protected override void Context()
      {
         base.Context();
         var firstCompound = new CompoundCondition();
         firstCompound.Add(new MatchTagCondition("A"));
         firstCompound.Add(new MatchTagCondition("B"));
         sut.Add(firstCompound);

         var secondCompound = new CompoundCondition();
         secondCompound.Add(new MatchTagCondition("A"));
         secondCompound.Add(new MatchTagCondition("B"));
         _equivalentCriteria = new DescriptorCriteria();
         _equivalentCriteria.Add(secondCompound);
      }

      [Observation]
      public void should_be_equal()
      {
         sut.Equals(_equivalentCriteria).ShouldBeTrue();
      }
   }

   public class When_comparing_two_descriptor_criteria_with_compounds_differing_by_inner_operator : concern_for_DescriptorCriteria
   {
      private DescriptorCriteria _criteriaWithDifferentInnerOperator;

      protected override void Context()
      {
         base.Context();
         var compoundWithAnd = new CompoundCondition();
         compoundWithAnd.Add(new MatchTagCondition("A"));
         sut.Add(compoundWithAnd);

         var compoundWithOr = new CompoundCondition { Operator = CriteriaOperator.Or };
         compoundWithOr.Add(new MatchTagCondition("A"));
         _criteriaWithDifferentInnerOperator = new DescriptorCriteria();
         _criteriaWithDifferentInnerOperator.Add(compoundWithOr);
      }

      [Observation]
      public void should_not_be_equal()
      {
         sut.Equals(_criteriaWithDifferentInnerOperator).ShouldBeFalse();
      }
   }
}