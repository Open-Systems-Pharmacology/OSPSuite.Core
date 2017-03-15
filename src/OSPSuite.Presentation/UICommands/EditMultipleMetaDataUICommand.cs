using System.Collections.Generic;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Presentation.UICommands
{
   public class EditMultipleMetaDataUICommand : ObjectUICommand<IEnumerable<DataRepository>>
   {
      private readonly IEditObservedDataTask _observedDataTask;

      public EditMultipleMetaDataUICommand(IEditObservedDataTask observedDataTask)
      {
         _observedDataTask = observedDataTask;
      }

      protected override void PerformExecute()
      {
         _observedDataTask.EditMultipleMetaDataFor(Subject);
      }
   }
}