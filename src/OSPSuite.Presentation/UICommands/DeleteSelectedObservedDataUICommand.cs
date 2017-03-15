using System.Collections.Generic;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Presentation.UICommands
{
   public class DeleteSelectedObservedDataUICommand : ObjectUICommand<IEnumerable<DataRepository>>
   {
      private readonly IObservedDataTask _observedDataTask;

      public DeleteSelectedObservedDataUICommand(IObservedDataTask observedDataTask)
      {
         _observedDataTask = observedDataTask;
      }

      protected override void PerformExecute()
      {
         _observedDataTask.Delete(Subject);
      }
   }
}