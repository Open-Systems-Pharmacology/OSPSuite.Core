using System.Collections.Generic;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Presentation.UICommands
{
   public class RemoveUsedObservedDataUICommand : ObjectUICommand<IReadOnlyList<UsedObservedData>>
   {
      private readonly IObservedDataTask _observedDataTask;

      public RemoveUsedObservedDataUICommand(IObservedDataTask observedDataTask)
      {
         _observedDataTask = observedDataTask;
      }

      protected override void PerformExecute()
      {
         _observedDataTask.RemoveUsedObservedDataFromSimulation(Subject);
      }

      public RemoveUsedObservedDataUICommand For(UsedObservedData usedObservedData)
      {
         For(new[] {usedObservedData});
         return this;
      }
   }
}
