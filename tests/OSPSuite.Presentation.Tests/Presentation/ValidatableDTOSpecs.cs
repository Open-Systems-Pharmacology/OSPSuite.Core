using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Presentation.DTO;
using OSPSuite.Utility.Reflection;
using OSPSuite.Utility.Validation;

namespace OSPSuite.Presentation.Presentation
{
   internal abstract class concern_for_ValidatableDTO : ContextSpecification<ValidatableDTO<ValidatableForSpecs>>
   {
      protected ValidatableForSpecs _validatable;

      protected override void Context()
      {
         _validatable = new ValidatableForSpecs();
         sut = new ValidatableDTOForSpecs(_validatable);
      }
   }

   internal class When_adding_a_notification_for_a_property_from_the_underlying_object : concern_for_ValidatableDTO
   {
      private bool _propertyChanged;
      private string _propertyName;

      protected override void Context()
      {
         base.Context();
         sut.PropertyChanged += (o, e) =>
         {
            _propertyChanged = true;
            _propertyName = e.PropertyName;
         };
      }

      protected override void Because()
      {
         sut.AddNotifiableFor<ParameterDTO, double>(x => x.Value);
      }

      [Observation]
      public void should_notify_the_property_with_the_given_name()
      {
         _propertyChanged.ShouldBeTrue();
         _propertyName.ShouldBeEqualTo("Value");
      }
   }

   internal class ParameterDTO : ValidatableDTO
   {
      public double Value { get; set; }
   }

   internal class ValidatableForSpecs : Notifier, IValidatable
   {
      private readonly IBusinessRuleSet _rules;

      public ValidatableForSpecs()
      {
         _rules = new BusinessRuleSet();
      }

      public IBusinessRuleSet Rules
      {
         get { return _rules; }
      }
   }

   internal class ValidatableDTOForSpecs : ValidatableDTO<ValidatableForSpecs>
   {
      public ValidatableDTOForSpecs(ValidatableForSpecs parameter)
         : base(parameter)
      {
      }
   }
}