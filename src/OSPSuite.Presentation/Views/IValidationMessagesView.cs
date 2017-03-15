using System.Collections.Generic;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.Presenters;

namespace OSPSuite.Presentation.Views
{
   public interface IValidationMessagesView : IModalView<IValidationMessagesPresenter>
   {
      void BindTo(IEnumerable<ValidationMessageDTO> validationMessageDtos);
   }
}