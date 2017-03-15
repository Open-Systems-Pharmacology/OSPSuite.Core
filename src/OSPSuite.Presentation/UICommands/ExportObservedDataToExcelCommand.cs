using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Presentation.UICommands
{
   public class ExportObservedDataToExcelCommand : ObjectUICommand<DataRepository>
   {
      private readonly IObservedDataTask _observedDataTask;

      public ExportObservedDataToExcelCommand(IObservedDataTask observedDataTask)
      {
         _observedDataTask = observedDataTask;
      }

      protected override void PerformExecute()
      {
         _observedDataTask.Export(Subject);
      }
   }
}