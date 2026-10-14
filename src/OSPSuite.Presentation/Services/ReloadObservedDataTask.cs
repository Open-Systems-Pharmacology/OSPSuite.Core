using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Core.Events;
using OSPSuite.Core.Services;
using OSPSuite.Infrastructure.Import.Services;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Services
{
   public class ReloadObservedDataTask : IReloadObservedDataTask
   {
      private readonly IOSPSuiteExecutionContext _executionContext;
      private readonly IObservedDataImporter _observedDataImporter;
      private readonly IDataImporter _dataImporter;
      private readonly IObservedDataTask _observedDataTask;
      private readonly IParameterIdentificationTask _parameterIdentificationTask;
      private readonly IDialogCreator _dialogCreator;

      public ReloadObservedDataTask(
         IOSPSuiteExecutionContext executionContext,
         IObservedDataImporter observedDataImporter,
         IDataImporter dataImporter,
         IObservedDataTask observedDataTask,
         IParameterIdentificationTask parameterIdentificationTask,
         IDialogCreator dialogCreator)
      {
         _executionContext = executionContext;
         _observedDataImporter = observedDataImporter;
         _dataImporter = dataImporter;
         _observedDataTask = observedDataTask;
         _parameterIdentificationTask = parameterIdentificationTask;
         _dialogCreator = dialogCreator;
      }

      public void ReloadAllObservedDataImportedWith(string configurationId)
      {
         var project = _executionContext.Project;
         var configuration = project.ImporterConfigurationBy(configurationId);
         if (configuration == null)
         {
            _dialogCreator.MessageBoxError(Captions.Importer.ImporterConfigurationNotFoundInProject);
            return;
         }

         var existingObservedData = project.AllObservedData.Where(x => x.ConfigurationId == configurationId).ToList();
         var reloadDataSets = _dataImporter.CalculateReloadDataSetsFromConfiguration(_observedDataImporter.ImportFrom(configuration), existingObservedData);

         reloadDataSets.NewDataSets.Each(_observedDataImporter.AddImportedObservedDataToProject);
         reloadDataSets.DataSetsToBeDeleted.Each(x => _observedDataTask.Delete(x));

         var overwrittenObservedData = reloadDataSets.OverwrittenDataSets.Select(x => overwrite(existingObservedData, x)).ToList();
         if (!overwrittenObservedData.Any())
            return;

         _executionContext.ProjectChanged();
         _parameterIdentificationTask.UpdateParameterIdentificationsUsing(overwrittenObservedData);
         overwrittenObservedData.Each(x => _executionContext.PublishEvent(new ObservedDataValueChangedEvent(x)));
      }

      private DataRepository overwrite(IReadOnlyList<DataRepository> existingObservedData, DataRepository reloadedObservedData)
      {
         var observedData = existingObservedData.First(x => _dataImporter.AreFromSameMetaDataCombination(x, reloadedObservedData));
         observedData.BaseGrid.Values = reloadedObservedData.BaseGrid.Values;
         reloadedObservedData.AllButBaseGrid().Each(column => overwriteColumn(observedData, column));
         return observedData;
      }

      private void overwriteColumn(DataRepository observedData, DataColumn reloadedColumn) =>
         observedData.First(x => x.Name == reloadedColumn.Name).Values = reloadedColumn.Values;
   }
}
