using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Presentation.UICommands
{
   public class ReloadAllObservedDataUICommand : ObjectUICommand<DataRepository>
   {
      private readonly IReloadObservedDataTask _reloadObservedDataTask;

      public ReloadAllObservedDataUICommand(IReloadObservedDataTask reloadObservedDataTask)
      {
         _reloadObservedDataTask = reloadObservedDataTask;
      }

      protected override void PerformExecute()
      {
         if (string.IsNullOrEmpty(Subject.ConfigurationId))
            return;

         _reloadObservedDataTask.ReloadAllObservedDataImportedWith(Subject.ConfigurationId);
      }
   }
}
