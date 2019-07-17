using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Services;
using Command = OSPSuite.Assets.Command;

namespace OSPSuite.Core.Domain.Services
{
   public interface IObservedDataTask
   {
      void Rename(DataRepository observedData);

      /// <summary>
      ///    Deletes the <paramref name="observedData" /> from the project.
      ///    Returns <c>true</c> if the deletion was confirm by the user otherwise <c>false</c>
      /// </summary>
      bool Delete(DataRepository observedData);

      /// <summary>
      ///    Deletes the <paramref name="observedDataEnumerable" /> from the project.
      ///    Returns <c>true</c> if the deletion was confirm by the user otherwise <c>false</c>
      /// </summary>
      bool Delete(IEnumerable<DataRepository> observedDataEnumerable);

      void Export(DataRepository observedData);

      /// <summary>
      ///    Delete all observed data defined in the project
      ///    Returns <c>true</c> if the deletion was confirm by the user otherwise <c>false</c>
      /// </summary>
      bool DeleteAll();

      void AddObservedDataToProject(DataRepository observedData);
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

      public bool Delete(IEnumerable<DataRepository> observedDataToBeRemoved)
      {
         var observedDataToRemoveList = observedDataToBeRemoved.ToList();

         var usedInAnalyzablesCache = new Cache<DataRepository, IEnumerable<IUsesObservedData>>();

         observedDataToRemoveList.Each(x =>
         {
            usedInAnalyzablesCache[x] = allUsersOfObservedData(x);
         });

         var observedDataThatCanBeRemoved = observedDataToRemoveList.Where(x => !usedInAnalyzablesCache[x].Any()).ToList();
         var observedDataNotDeleted = observedDataToRemoveList.Except(observedDataThatCanBeRemoved);
         var observedDataNotDeletedMessage = observedDataNotDeleted.Select(x => messageForObservedDataUsedByAnalysable(x, usedInAnalyzablesCache));

         //not one observed data can be deleted because all of them are used
         if (!observedDataThatCanBeRemoved.Any())
            throw new CannotDeleteObservedDataException(observedDataNotDeletedMessage);


         var viewResult = _dialogCreator.MessageBoxYesNo(Captions.ReallyDeleteAllObservedData(observedDataNotDeletedMessage));

         if (viewResult == ViewResult.No)
            return false;

         return deleteAll(observedDataThatCanBeRemoved);
      }

      private string messageForObservedDataUsedByAnalysable(DataRepository observedData, Cache<DataRepository, IEnumerable<IUsesObservedData>> usersOfObservedDataCache)
      {
         var typeNamesUsingObservedData = usersOfObservedDataCache[observedData].Select(typeNamed).ToList();
         return Error.CannotDeleteObservedData(observedData.Name, typeNamesUsingObservedData);
      }

      private bool deleteAll(IEnumerable<DataRepository> observedDataToDelete)
      {
         var macoCommand = new OSPSuiteMacroCommand<IOSPSuiteExecutionContext>
         {
            CommandType = Command.CommandTypeDelete,
            ObjectType = ObjectTypes.ObservedData,
            Description = Command.ObservedDataDeletedFromProject,
         };

         observedDataToDelete.Each(x => macoCommand.Add(new RemoveObservedDataFromProjectCommand(x)));
         _executionContext.AddToHistory(macoCommand.Run(_executionContext));
         return true;
      }

      public bool DeleteAll()
      {
         return Delete(_executionContext.Project.AllObservedData);
      }

      public bool Delete(DataRepository observedData)
      {
         var usersOfObservedData = allUsersOfObservedData(observedData).ToList();

         if (usersOfObservedData.Any())
            throw new CannotDeleteObservedDataException(observedData.Name, usersOfObservedData.Select(typeNamed).ToList());

         var viewResult = _dialogCreator.MessageBoxYesNo(Captions.ReallyDeleteObservedData(observedData.Name));
         if (viewResult == ViewResult.No)
            return false;

         var removeCommand = new RemoveObservedDataFromProjectCommand(observedData).Run(_executionContext);
         _executionContext.AddToHistory(removeCommand);
         return true;
      }

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

      public void AddObservedDataToProject(DataRepository observedData)
      {
         if (observedDataAlreadyExistsInProject(observedData))
            return;

         observedData.Name = _containerTask.CreateUniqueName(_executionContext.Project.AllObservedData, observedData.Name, canUseBaseName: true);
         _executionContext.AddToHistory(new AddObservedDataToProjectCommand(observedData).Run(_executionContext));
      }

      private IEnumerable<IUsesObservedData> allUsersOfObservedData(DataRepository observedData)
      {
         return _executionContext.Project.AllUsersOfObservedData.Where(x => x.UsesObservedData(observedData));
      }

      //TODO See if code can be merged between APPS
      public abstract void Rename(DataRepository observedData);
   }

}