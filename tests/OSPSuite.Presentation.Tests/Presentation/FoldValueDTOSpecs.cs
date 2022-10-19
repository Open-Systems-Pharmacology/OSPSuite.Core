using FakeItEasy;
using NUnit.Framework;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Presentation.DTO.Charts;
using OSPSuite.Utility.Validation;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_FoldValueDTO : ContextSpecification<FoldValueDTO>
   {
      protected override void Context()
      {
         sut = new FoldValueDTO();
      }
   }

   public class When_checking_for_the_rule_validity_of_a_folder_value_dto : concern_for_FoldValueDTO
   {
      [Observation]
      [TestCase(0.2f, false)]
      [TestCase(1.2f, true)]
      public void should_return_the_expected_validation_rule(float value, bool isValid)
      {
         sut.FoldValue = value;
         sut.IsValid().ShouldBeEqualTo(isValid);
      }
   }
}