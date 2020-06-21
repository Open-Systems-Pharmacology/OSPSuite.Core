using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Presentation.DTO;
using OSPSuite.Utility.Validation;

namespace OSPSuite.Presentation.Presentation
{
   public class When_creating_a_validatable_dto_for_a_validatable_object : StaticContextSpecification
   {
      private IValidatable _validatableObject;
      private IBusinessRuleSet _businessRuleSet;
      private OneValidatableDTO _validtableDTO;

      protected override void Context()
      {
         _validatableObject = A.Fake<IValidatable>();
         _businessRuleSet = A.Fake<IBusinessRuleSet>();
         A.CallTo(() => _validatableObject.Rules).Returns(_businessRuleSet);
      }

      protected override void Because()
      {
         _validtableDTO = ValidatableDTOExtensions.AddRulesFrom(new OneValidatableDTO(), _validatableObject);
      }

      [Observation]
      public void should_add_a_validatable_rule_to_the_dto_object_for_the_validatable_object_()
      {
         _validtableDTO.Rules.Count.ShouldBeEqualTo(1);
         _validtableDTO.Rules.All().First().ShouldBeAnInstanceOf<ValidatableBusinessRule>();
      }
   }

   public class OneValidatableDTO : ValidatableDTO
   {

   }
}	