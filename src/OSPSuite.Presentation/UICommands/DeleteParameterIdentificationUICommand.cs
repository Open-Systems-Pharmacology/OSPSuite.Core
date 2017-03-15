using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Core.Services;

namespace OSPSuite.Presentation.UICommands
{
   public class DeleteParameterIdentificationUICommand : ObjectUICommand<ParameterIdentification>
   {
      private readonly IParameterIdentificationTask _parameterIdentificationTask;

      public DeleteParameterIdentificationUICommand(IDialogCreator dialogCreator, IParameterIdentificationTask parameterIdentificationTask)
      {
         _parameterIdentificationTask = parameterIdentificationTask;
      }

      protected override void PerformExecute()
      {
         _parameterIdentificationTask.Delete(new[] {Subject});
      }
   }
}