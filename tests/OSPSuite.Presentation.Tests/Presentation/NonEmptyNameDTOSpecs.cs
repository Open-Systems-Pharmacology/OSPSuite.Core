using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Presentation.DTO;
using OSPSuite.Utility.Validation;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_NonEmptyNameDTO : ContextSpecification<NonEmptyNameDTO>
   {
      protected override void Context()
      {
         sut = new NonEmptyNameDTO();
      }
   }

   public class When_validating_a_nom_empty_name_dto : concern_for_NonEmptyNameDTO
   {
      [Observation]
      public void should_return_that_the_object_is_valid_if_its_name_is_not_empty()
      {
         sut.Name = "toto";
         sut.Validate().IsEmpty.ShouldBeTrue();
      }

      [Observation]
      public void should_return_that_the_object_is_invalid_if_its_name_is_empty()
      {
         sut.Name = string.Empty;
         sut.Validate().IsEmpty.ShouldBeFalse();
      }

      [Observation]
      public void should_return_that_the_object_is_valid_if_its_name_contains_quote()
      {
         sut.Name = "toto\"";
         sut.Validate().IsEmpty.ShouldBeFalse();
      }
   }
}