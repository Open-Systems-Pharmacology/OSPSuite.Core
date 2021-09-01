using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Import;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using Command = OSPSuite.Assets.Command;

namespace OSPSuite.Core.Domain.Services
{
   public interface IObservedDataTask
   {
      void Rename(DataRepository observedData);

      /// <summary>
      ///    Deletes the <paramref name="observedData" /> from the project.
      /// </summary>
      bool Delete(DataRepository observedData);

      /// <summary>
      ///    Deletes the <paramref name="observedDataEnumerable" /> from the project. User prompt can be turned off (<paramref name="silent"/> set to <c>true</c>).
      ///    Returns <c>true</c> if the deletion was confirm by the user otherwise <c>false</c> (only if the <paramref name="silent"/> flag is set to <c>false</c> which is the default)
      /// </summary>
      bool Delete(IEnumerable<DataRepository> observedDataEnumerable, bool silent = false);

      void Export(DataRepository observedData);

      /// <summary>
      ///    Delete all observed data defined in the project
      ///    Returns <c>true</c> if the deletion was confirm by the user otherwise <c>false</c>
      /// </summary>
      bool DeleteAll();

      /// <summary>
      ///    Update the molecular weight for an observed data set from the value of the
      ///    corresponding molecule in the project.
      ///    When f.e. the selected Molecule of a data set gets changed,
      ///    the MolWeight of the data set can get updated by calling this function.
      /// </summary>
      void UpdateMolWeight(DataRepository observedData);

      void AddObservedDataToProject(DataRepository observedData);
      void AddImporterConfigurationToProject(ImporterConfiguration configuration);
   }

   public abstract class ObservedDataTask : IObservedDataTask
   {
      protected readonly IDialogCreator _dialogCreator;
      private readonly IOSPSuiteExecutionContext _executionContext;
      private readonly IDataRepositoryExportTask _dataRepositoryExportTask;
      private readonly IContainerTask _containerTask;
      private readonly IObjectTypeResolver _objectTypeResolver;

      protected ObservedDataTask(IDialogCreator dialogCreator, IOSPSuiteExecutionContext executionContext, IDataRepositoryExportTask dataRepositoryExportTask, IContainerTask containerTask, IObjectTypeResolver objectTypeResolver)
      {
         _dialogCreator = dialogCreator;
         _executionContext = executionContext;
         _dataRepositoryExportTask = dataRepositoryExportTask;
         _containerTask = containerTask;
         _objectTypeResolver = objectTypeResolver;
      }

      public bool Delete(DataRepository observedData)
      {
         return Delete(new [] { observedData });
      }

      public bool Delete(IEnumerable<DataRepository> observedDataToBeRemoved, bool silent = false)
      {
         var observedDataToRemoveList = observedDataToBeRemoved.ToList();

         var usedInAnalyzablesCache = new Cache<DataRepository, IEnumerable<IUsesObservedData>>();

         observedDataToRemoveList.Each(x => { usedInAnalyzablesCache[x] = allUsersOfObservedData(x); });

         var observedDataThatCanBeRemoved = observedDataToRemoveList.Where(x => !usedInAnalyzablesCache[x].Any()).ToList();
         var observedDataNotDeleted = observedDataToRemoveList.Except(observedDataThatCanBeRemoved);
         var observedDataNotDeletedMessage = observedDataNotDeleted.Select(x => messageForObservedDataUsedByAnalysable(x, usedInAnalyzablesCache));

         //not one observed data can be deleted because all of them are used
         if (!observedDataThatCanBeRemoved.Any())
            throw new CannotDeleteObservedDataException(observedDataNotDeletedMessage);

         if (!silent)
         {
            var viewResult = _dialogCreator.MessageBoxYesNo(Captions.ReallyDeleteAllObservedData(observedDataNotDeletedMessage));
            if (viewResult == ViewResult.No)
               return false;
         }

         return deleteAll(observedDataThatCanBeRemoved);
      }

      private string messageForObservedDataUsedByAnalysable(DataRepository observedData, Cache<DataRepository, IEnumerable<IUsesObservedData>> usersOfObservedDataCache)
      {
         var typeNamesUsingObservedData = usersOfObservedDataCache[observedData].Select(typeNamed).ToList();
         return Error.CannotDeleteObservedData(observedData.Name, typeNamesUsingObservedData);
      }

      private bool deleteAll(IEnumerable<DataRepository> observedDataToDelete)
      {
         var macroCommand = new OSPSuiteMacroCommand<IOSPSuiteExecutionContext>
         {
            CommandType = Command.CommandTypeDelete,
            ObjectType = ObjectTypes.ObservedData,
            Description = Command.ObservedDataDeletedFromProject,
         };

         observedDataToDelete.Each(x => macroCommand.Add(new RemoveObservedDataFromProjectCommand(x)));
         _executionContext.AddToHistory(macroCommand.Run(_executionContext));
         return true;
      }

      public bool DeleteAll()
      {
         return Delete(_executionContext.Project.AllObservedData);
      }

      public abstract void UpdateMolWeight(DataRepository observedData);

      private string typeNamed(IUsesObservedData usesObservedData)
      {
         return $"{_objectTypeResolver.TypeFor(usesObservedData).ToLowerInvariant()} '{usesObservedData.Name}'";
      }

      public void Export(DataRepository observedData)
      {
         var file = _dialogCreator.AskForFileToSave(Captions.ExportObservedDataToExcel, Constants.Filter.EXCEL_SAVE_FILE_FILTER, Constants.DirectoryKey.OBSERVED_DATA, observedData.Name);
         if (string.IsNullOrEmpty(file)) return;

         _dataRepositoryExportTask.ExportToExcel(observedData, file, launchExcel: true);
      }

      private bool observedDataAlreadyExistsInProject(DataRepository observedData)
      {
         return _executionContext.Project.AllObservedData.ExistsById(observedData.Id);
      }

      private bool importerConfigurationAlreadyExistsInProject(ImporterConfiguration configuration)
      {
         return _executionContext.Project.AllImporterConfigurations.ExistsById(configuration.Id);
      }

      public void AddObservedDataToProject(DataRepository observedData)
      {
         if (observedDataAlreadyExistsInProject(observedData))
            return;

         observedData.Name = _containerTask.CreateUniqueName(_executionContext.Project.AllObservedData, observedData.Name, canUseBaseName: true);
         _executionContext.AddToHistory(new AddObservedDataToProjectCommand(observedData).Run(_executionContext));
      }

      public void AddImporterConfigurationToProject(ImporterConfiguration configuration)
      {
         if (importerConfigurationAlreadyExistsInProject(configuration))
            return;

         _executionContext.AddToHistory(new AddImporterConfigurationToProjectCommand(configuration).Run(_executionContext));
      }

      private IEnumerable<IUsesObservedData> allUsersOfObservedData(DataRepository observedData)
      {
         return _executionContext.Project.AllUsersOfObservedData.Where(x => x.UsesObservedData(observedData));
      }

      //TODO See if code can be merged between APPS
      public abstract void Rename(DataRepository observedData);
   }
}