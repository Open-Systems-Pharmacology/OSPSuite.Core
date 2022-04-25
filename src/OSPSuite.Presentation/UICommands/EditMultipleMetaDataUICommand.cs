using System.Collections.Generic;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Presentation.UICommands
{
   public class EditMultipleMetaDataUICommand : ObjectUICommand<IEnumerable<DataRepository>>
   {
      private readonly IObservedDataMetaDataTask _observedDataMetaDataTask;

      public EditMultipleMetaDataUICommand(IObservedDataMetaDataTask observedDataMetaDataTask)
      {
         _observedDataMetaDataTask = observedDataMetaDataTask;
      }

      protected override void PerformExecute()
      {
         _observedDataMetaDataTask.EditMultipleMetaDataFor(Subject);
      }
   }
}