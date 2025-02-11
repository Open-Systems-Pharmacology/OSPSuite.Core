using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_ValidationResult : ContextSpecification<ValidationResult>
   {
      protected IEnumerable<ValidationMessage> _listToCheck;
      protected ValidationMessage _error;
      protected ValidationMessage _warning;
      protected ValidationMessage _otherError;

      protected override void Context()
      {
         base.Context();
         _error = A.Fake<ValidationMessage>();
         _warning = A.Fake<ValidationMessage>();
         _otherError = A.Fake<ValidationMessage>();
         A.CallTo(()=>_error.NotificationType).Returns(NotificationType.Error);
         A.CallTo(() => _otherError.NotificationType).Returns(NotificationType.Error);
        
      }
   }

   
   public class When_the_validation_results_contains_messages_reprenting_an_error : concern_for_ValidationResult
   {
      protected override void Context()
      {
         base.Context();

         sut = new ValidationResult(new List<ValidationMessage>
                        {
                           _error,
                           _warning,
                           _otherError,
                        });
      }

      [Observation]
      public void should_return_an_invalid_state()
      {
         sut.ValidationState.ShouldBeEqualTo(ValidationState.Invalid);
      }
   }

   
   public class When_the_validation_results_only_contains_messages_reprenting_an_Warning : concern_for_ValidationResult
   {
      protected override void Context()
      {
         base.Context();
         sut = new ValidationResult(new List<ValidationMessage>{_warning});
      }

      [Observation]
      public void should_return_a_valid_with_warnings_state()
      {
         sut.ValidationState.ShouldBeEqualTo(ValidationState.ValidWithWarnings);
      }
   }

   
   public class When_the_validation_results_contains_no_messages: concern_for_ValidationResult
   {
      protected override void Context()
      {
         base.Context();
         sut = new ValidationResult(new List<ValidationMessage>{_warning});
      }

      [Observation]
      public void should_return_a_valid_state()
      {
         sut.ValidationState.ShouldBeEqualTo(ValidationState.ValidWithWarnings);
      }
   }

   public class When_constructing_a_validation_message_with_messages_for_object_from_different_lists : concern_for_ValidationResult
   {
      private ValidationResult _validationResult1, _validationResult2;
      private IObjectBase _invalidObject;

      protected override void Context()
      {
         base.Context();
         _invalidObject = A.Fake<IObjectBase>();
         _validationResult1 = new ValidationResult();
         _validationResult1.AddMessage(NotificationType.Error, _invalidObject, "Object Invalid", details: new[] { "Detail1", "Detail2" });

         _validationResult2 = new ValidationResult();
         _validationResult2.AddMessage(NotificationType.Error, _invalidObject, "It's invalid for another reason", details: new[] { "Detail3", "Detail4" });
      }
      protected override void Because()
      {
         sut = new ValidationResult(_validationResult1, _validationResult2);
      }

      [Observation]
      public void should_add_the_details_of_the_new_message()
      {
         var message = sut.Messages.Find(x => x.Object == _invalidObject);
         message.Details.ShouldOnlyContainInOrder("Detail1", "Detail2", "It's invalid for another reason", "Detail3", "Detail4");
      }
   }

   public class When_adding_a_validation_message_for_an_object_for_which_a_validation_was_already_present : concern_for_ValidationResult
   {
      private ValidationResult _validationResult;
      private IObjectBase _invalidObject;

      protected override void Context()
      {
         base.Context();
         _invalidObject= A.Fake<IObjectBase>();
         _validationResult = new ValidationResult();
         _validationResult.AddMessage(NotificationType.Error, _invalidObject, "Object Invald", details: new [] {"Detail1", "Detail2"});
         sut = new ValidationResult();
         sut.AddMessage(NotificationType.Warning, _invalidObject,  "Warning");
      }
      protected override void Because()
      {
         sut.AddMessagesFrom(_validationResult);
      }

      [Observation]
      public void should_add_the_details_of_the_new_message()
      {
         var message = sut.Messages.Find(x => x.Object == _invalidObject);
         message.Details.ShouldOnlyContainInOrder("Warning", "Object Invald","Detail1", "Detail2");
      }
   }

}