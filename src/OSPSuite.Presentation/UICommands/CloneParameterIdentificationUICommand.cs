using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Presentation.Services;

namespace OSPSuite.Presentation.UICommands
{
   public class CloneParameterIdentificationUICommand : ObjectUICommand<ParameterIdentification>
   {
      private readonly ISingleStartPresenterTask _singleStartPresenterTask;
      private readonly IParameterIdentificationTask _parameterIdentificationTask;
      
      public CloneParameterIdentificationUICommand(ISingleStartPresenterTask singleStartPresenterTask, IParameterIdentificationTask parameterIdentificationTask)
      {
         _singleStartPresenterTask = singleStartPresenterTask;
         _parameterIdentificationTask = parameterIdentificationTask;
      }

      protected override void PerformExecute()
      {
         var clone = _parameterIdentificationTask.Clone(Subject);
         if (clone == null)
            return;

         _parameterIdentificationTask.AddToProject(clone);
         _singleStartPresenterTask.StartForSubject(clone);
      }
   }
}
