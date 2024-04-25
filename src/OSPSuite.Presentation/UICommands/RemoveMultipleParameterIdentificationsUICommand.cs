using System.Collections.Generic;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;

namespace OSPSuite.Presentation.UICommands
{
   public class RemoveMultipleParameterIdentificationsUICommand : ObjectUICommand<IReadOnlyList<ParameterIdentification>>
   {
      private readonly IParameterIdentificationTask _parameterIdentificationTask;

      public RemoveMultipleParameterIdentificationsUICommand(IParameterIdentificationTask parameterIdentificationTask)
      {
         _parameterIdentificationTask = parameterIdentificationTask;
      }

      protected override void PerformExecute()
      {
         _parameterIdentificationTask.Delete(Subject);
      }
   }
}