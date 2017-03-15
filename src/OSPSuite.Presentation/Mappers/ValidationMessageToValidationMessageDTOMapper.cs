using System;
using OSPSuite.Assets;
using OSPSuite.Utility;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.DTO;

namespace OSPSuite.Presentation.Mappers
{
   public interface IValidationMessageToValidationMessageDTOMapper : IMapper<ValidationMessage, ValidationMessageDTO>
   {
   }

   public class ValidationMessageToValidationMessageDTOMapper : IValidationMessageToValidationMessageDTOMapper
   {
      private readonly IFullPathDisplayResolver _fullPathDisplayResolver;

      public ValidationMessageToValidationMessageDTOMapper(IFullPathDisplayResolver fullPathDisplayResolver)
      {
         _fullPathDisplayResolver = fullPathDisplayResolver;
      }

      public ValidationMessageDTO MapFrom(ValidationMessage validationMessage)
      {
         var dto = new ValidationMessageDTO
         {
            Message = validationMessage.Text,
            ObjectDescription = _fullPathDisplayResolver.FullPathFor(validationMessage.Object),
            Status = validationMessage.NotificationType,
            Icon = iconFrom(validationMessage.NotificationType)
         };
         dto.AddDetails(validationMessage.Details);
         return dto;
      }

      private ApplicationIcon iconFrom(NotificationType notificationType)
      {
         switch (notificationType)
         {
            case NotificationType.Error:
               return ApplicationIcons.Error;
            case NotificationType.Warning:
               return ApplicationIcons.Warning;
            case NotificationType.Info:
               return ApplicationIcons.Info;
            case NotificationType.Debug:
               return ApplicationIcons.Debug;
            default:
               throw new ArgumentOutOfRangeException(nameof(notificationType));
         }
      }
   }
}