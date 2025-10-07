using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Services;
using ValidationResult = OSPSuite.Core.Domain.ValidationResult;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_EntityValidationTask : ContextSpecification<IEntityValidationTask>
   {
      private IOSPSuiteExecutionContext _executionContext;
      protected IApplicationController _applicationController;
      protected IEntity _entity;
      protected IEntityValidatorFactory _entityValidatorFactory;

      protected override void Context()
      {
         _executionContext = A.Fake<IOSPSuiteExecutionContext>();
         _entityValidatorFactory = A.Fake<IEntityValidatorFactory>();
         _applicationController = A.Fake<IApplicationController>();

         sut = new EntityValidationTask(_entityValidatorFactory, _applicationController, _executionContext);
      }
   }

   public class When_validating_a_valid_entity : concern_for_EntityValidationTask
   {
      protected override void Context()
      {
         base.Context();
         _entity = new Container().WithName("container");
      }

      [Observation]
      public void should_return_that_the_entity_is_valid()
      {
         sut.Validate(_entity).ShouldBeTrue();
      }
   }


   public class When_validating_a_invalid_entity_and_the_user_does_not_accept_the_error : concern_for_EntityValidationTask
   {
      private IValidationMessagesPresenter _presenter;
      private ValidationResult _validationResult;

      protected override void Context()
      {
         base.Context();
         // containers without names are invalid
         _entity = new Container();
         _validationResult = A.Fake<ValidationResult>();
         _presenter = A.Fake<IValidationMessagesPresenter>();

         A.CallTo(() => _entityValidatorFactory.Validate(_entity)).Returns(_validationResult);
         A.CallTo(() => _applicationController.Start<IValidationMessagesPresenter>()).Returns(_presenter);
         A.CallTo(() => _validationResult.ValidationState).Returns(ValidationState.Invalid);
         A.CallTo(() => _presenter.Display(_validationResult)).Returns(false);
      }

      [Observation]
      public void should_return_that_the_entity_is_invalid()
      {
         sut.Validate(_entity).ShouldBeFalse();
      }
   }

   public class When_validating_a_invalid_entity_and_the_user_accepts_the_error : concern_for_EntityValidationTask
   {
      private IValidationMessagesPresenter _presenter;
      private ValidationResult _validationResult;

      protected override void Context()
      {
         base.Context();
         _entity = new Container();
         _presenter = A.Fake<IValidationMessagesPresenter>();
         _validationResult = A.Fake<ValidationResult>();
         
         A.CallTo(() => _entityValidatorFactory.Validate(_entity)).Returns(_validationResult);
         A.CallTo(() => _validationResult.ValidationState).Returns(ValidationState.Invalid);
         A.CallTo(() => _applicationController.Start<IValidationMessagesPresenter>()).Returns(_presenter);
         A.CallTo(() => _presenter.Display(A<ValidationResult>._)).Returns(true);
      }

      [Observation]
      public void should_return_that_the_entity_is_invalid()
      {
         sut.Validate(_entity).ShouldBeTrue();
      }
   }
}