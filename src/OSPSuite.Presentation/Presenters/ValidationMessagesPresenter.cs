using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Mappers;
using OSPSuite.Presentation.Views;

namespace OSPSuite.Presentation.Presenters
{
   public interface IValidationMessagesPresenter : IPresenter<IValidationMessagesView>, IDisposablePresenter
   {
      /// <summary>
      ///    Display the validation results and return true if the user confirms the error othewise false
      /// </summary>
      bool Display(ValidationResult validationResult);

      /// <summary>
      ///    set the caption of the view
      /// </summary>
      string Caption { set; }
   }

   public class ValidationMessagesPresenter : AbstractDisposablePresenter<IValidationMessagesView, IValidationMessagesPresenter>, IValidationMessagesPresenter
   {
      private readonly IValidationMessageToValidationMessageDTOMapper _messageDTOMapper;
      private IEnumerable<ValidationMessageDTO> _allMessagesDTO;

      public ValidationMessagesPresenter(IValidationMessagesView view, IValidationMessageToValidationMessageDTOMapper messageDTOMapper) : base(view)
      {
         _messageDTOMapper = messageDTOMapper;
      }

      public bool Display(ValidationResult validationResult)
      {
         _allMessagesDTO = validationResult.Messages.MapAllUsing(_messageDTOMapper);
         _view.BindTo(_allMessagesDTO);
         _view.OkEnabled = _allMessagesDTO.All(x => x.Status != NotificationType.Error);
         _view.Display();
         return !_view.Canceled;
      }

      public string Caption
      {
         set { _view.Caption = value; }
      }
   }
}