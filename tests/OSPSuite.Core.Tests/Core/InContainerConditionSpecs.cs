using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Descriptors;

namespace OSPSuite.Core
{
   public abstract class concern_for_InContainerCondition : ContextSpecification<InContainerCondition>
   {
      protected EntityDescriptor _entityCriteria;
      protected InContainerCondition _match;
      protected InContainerCondition _doNotMatch;
      protected InContainerCondition _alsoMatch;

      protected override void Context()
      {
         var entity = new Parameter();
         entity.AddTag("Parameter");
         entity.AddTag("Kidney");

         var parentContainer = new Container().WithName("Liver");
         parentContainer.AddTag("Organ");

         parentContainer.Add(entity);


         var grandParentContainer = new Container {parentContainer}.WithName("Organism");
         grandParentContainer.AddTag("DUDE");
         _entityCriteria = new EntityDescriptor(entity);

         _match = new InContainerCondition("Liver");
         _doNotMatch = new InContainerCondition("Kidney");
         _alsoMatch = new InContainerCondition("DUDE");
      }
   }

   public class When_checking_if_a_in_container_condition_matches_a_given_tag_set : concern_for_InContainerCondition
   {
      [Observation]
      public void check_that_string_representation_is_accurate()
      {
         _match.ToString().ShouldBeEqualTo("IN CONTAINER Liver");
      }

      [Observation]
      public void should_return_true_if_the_container_tag_contain_the_matching_element()
      {
         _match.IsSatisfiedBy(_entityCriteria).ShouldBeTrue();
      }

      [Observation]
      public void should_return_true_if_the_grand_parent_container_tag_contain_the_matching_element()
      {
         _alsoMatch.IsSatisfiedBy(_entityCriteria).ShouldBeTrue();
      }

      [Observation]
      public void should_return_false_if_the_container_tag_does_not_contain_the_matching_element()
      {
         _doNotMatch.IsSatisfiedBy(_entityCriteria).ShouldBeFalse();
      }
   }
}	