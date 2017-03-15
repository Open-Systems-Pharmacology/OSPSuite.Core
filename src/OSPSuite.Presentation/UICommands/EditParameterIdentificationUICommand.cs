using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Presentation.Services;

namespace OSPSuite.Presentation.UICommands
{
   public class EditParameterIdentificationUICommand : ObjectUICommand<ParameterIdentification>
   {
      private readonly ISingleStartPresenterTask _singleStartPresenterTask;

      public EditParameterIdentificationUICommand(ISingleStartPresenterTask singleStartPresenterTask)
      {
         _singleStartPresenterTask = singleStartPresenterTask;
      }

      protected override void PerformExecute()
      {
         _singleStartPresenterTask.StartForSubject(Subject);
      }
   }
}