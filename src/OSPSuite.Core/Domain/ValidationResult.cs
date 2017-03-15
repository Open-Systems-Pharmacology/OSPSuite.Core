using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Domain
{
   /// <summary>
   ///    Represents the results a validation run
   /// </summary>
   public class ValidationResult
   {
      private readonly ICache<IObjectBase, ValidationMessage> _messages;

      public ValidationResult() : this(new List<ValidationMessage>())
      {
      }

      public ValidationResult(IEnumerable<ValidationMessage> messages)
      {
         _messages = new Cache<IObjectBase, ValidationMessage>(x => x.Object);
         _messages.AddRange(messages);
      }

      public void AddMessagesFrom(ValidationResult validationResult)
      {
         validationResult.Messages.Each(message => AddMessage(message.NotificationType, message.Object, message.Text, message.BuildingBlock, message.Details));
      }

      public virtual void  AddMessage(NotificationType notificationType, IObjectBase invalidObject, string notification, IBuildingBlock buildingBlock = null, IEnumerable<string> details = null)
      {
         if (!_messages.Contains(invalidObject))
         {
            var message = new ValidationMessage(notificationType, notification, invalidObject, buildingBlock);
            addDetailsToMessage(message, details);
            addMessage(message);
         }
         else
         {
            var existingNotification = _messages[invalidObject];
            if (!existingNotification.Details.Any())
            {
               var oldNotification = existingNotification.Text;
               existingNotification.Text = Validation.MultipleNotificationsFor(notificationType.ToString(), invalidObject.Name);
               existingNotification.AddDetail(oldNotification);
            }
            existingNotification.AddDetail(notification);
            addDetailsToMessage(existingNotification, details);
         }
      }

      private void addDetailsToMessage(ValidationMessage message, IEnumerable<string> details)
      {
         if (details == null) return;
         message.AddDetails(details);
      }

      private void addMessage(ValidationMessage validationMessage)
      {
         _messages.Add(validationMessage);
      }

      /// <summary>
      ///    Gets the validation messages found during the validation run.
      /// </summary>
      /// <value>The validation messages.</value>
      public virtual IEnumerable<ValidationMessage> Messages => _messages;

      /// <summary>
      ///    Gets a value indicating whether the validation run treats the validated object as valid.
      /// </summary>
      /// <value>
      ///    <c>true</c> if valid; otherwise, <c>false</c>.
      /// </value>
      public virtual ValidationState ValidationState
      {
         get
         {
            if (!_messages.Any())
               return ValidationState.Valid;

            if (hasError())
               return ValidationState.Invalid;

            return ValidationState.ValidWithWarnings;
         }
      }

      private bool hasError()
      {
         return allErrors().Any();
      }

      private IEnumerable<ValidationMessage> allErrors()
      {
         return from messages in _messages
            where messages.NotificationType == NotificationType.Error
            select messages;
      }
   }

   public enum ValidationState
   {
      Valid,
      ValidWithWarnings,
      Invalid
   }
}