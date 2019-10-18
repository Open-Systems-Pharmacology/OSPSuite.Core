using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Presentation.DTO;
using OSPSuite.Utility.Validation;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_ObjectBaseDTO : ContextSpecification<ObjectBaseDTO>
   {
      protected override void Context()
      {
         sut = new ObjectBaseDTO();
      }
   }

   public class When_assessing_if_the_user_inputs_for_the_individual_properties_are_valid : concern_for_ObjectBaseDTO
   {
      private string _existingName;

      protected override void Context()
      {
         base.Context();
         _existingName = "tralala";
         sut.AddUsedNames(new[] {_existingName});
      }

      [Observation]
      public void should_return_true_if_the_name_given_to_the_individual_does_not_exist_already()
      {
         sut.Name = "toto";
         sut.IsValid().ShouldBeTrue();
      }

      [Observation]
      public void should_return_false_if_the_name_given_to_the_individual_has_already_been_defined()
      {
         sut.Name = _existingName;
         sut.IsValid().ShouldBeFalse();
      }
   }
}