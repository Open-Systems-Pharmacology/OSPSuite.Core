using OSPSuite.Assets;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Events;
using OSPSuite.Core.Import;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Command = OSPSuite.Assets.Command;
using DataColumn = OSPSuite.Core.Domain.Data.DataColumn;

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
      ///    Deletes the <paramref name="observedDataEnumerable" /> from the project. User prompt can be turned off (
      ///    <paramref name="silent" /> set to <c>true</c>).
      ///    Returns <c>true</c> if the deletion was confirm by the user otherwise <c>false</c> (only if the
      ///    <paramref name="silent" /> flag is set to <c>false</c> which is the default)
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

      /// <summary>
      ///    Removes from the simulations all the observed data as defined in the pairs of the
      ///    <paramref name="usedObservedDataList" />, while first checking that they are not used
      ///    in Parameter Identifications and also requiring confirmation by the user.
      /// </summary>
      void RemoveUsedObservedDataFromSimulation(IReadOnlyList<UsedObservedData> usedObservedDataList);
   }

   public abstract class ObservedDataTask : IObservedDataTask
   {
      protected readonly IDialogCreator _dialogCreator;
      private readonly IOSPSuiteExecutionContext _executionContext;
      private readonly IDataRepositoryExportTask _dataRepositoryExportTask;
      private readonly IContainerTask _containerTask;
      private readonly IObjectTypeResolver _objectTypeResolver;
      private readonly IConfirmationManager _confirmationManager;

      protected ObservedDataTask(IDialogCreator dialogCreator, IOSPSuiteExecutionContext executionContext,
         IDataRepositoryExportTask dataRepositoryExportTask, IContainerTask containerTask,
         IObjectTypeResolver objectTypeResolver, IConfirmationManager confirmationManager)
      {
         _dialogCreator = dialogCreator;
         _executionContext = executionContext;
         _dataRepositoryExportTask = dataRepositoryExportTask;
         _containerTask = containerTask;
         _objectTypeResolver = objectTypeResolver;
         _confirmationManager = confirmationManager;
      }

      public bool Delete(DataRepository observedData)
      {
         return Delete(new[] { observedData });
      }

      public bool Delete(IEnumerable<DataRepository> observedDataToBeRemoved, bool silent = false)
      {
         var observedDataToRemoveList = observedDataToBeRemoved.ToList();
         if (!observedDataToRemoveList.Any())
            return true;

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

      private string messageForObservedDataUsedByAnalysable(DataRepository observedData,
         Cache<DataRepository, IEnumerable<IUsesObservedData>> usersOfObservedDataCache)
      {
         var typeNamesUsingObservedData = usersOfObservedDataCache[observedData].Select(typeNamed).ToList();
         return Error.CannotDeleteObservedData(observedData.Name, typeNamesUsingObservedData);
      }

      private bool deleteAll(IReadOnlyList<DataRepository> observedDataToDelete)
      {
         var macroCommand = new OSPSuiteMacroCommand<IOSPSuiteExecutionContext>
         {
            CommandType = Command.CommandTypeDelete,
            ObjectType = ObjectTypes.ObservedData,
            Description = Command.ObservedDataDeletedFromProject,
         };

         macroCommand.Add(new RemoveObservedDataFromProjectCommand(observedDataToDelete));
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
         var file = _dialogCreator.AskForFileToSave(Captions.ExportObservedDataToExcel, Constants.Filter.EXCEL_SAVE_FILE_FILTER,
            Constants.DirectoryKey.OBSERVED_DATA, observedData.Name);
         if (string.IsNullOrEmpty(file)) return;

         var lloqColumns = createLloqColumns(observedData);
            
         _dataRepositoryExportTask.ExportToExcel(observedData.Columns.Concat(lloqColumns), file, launchExcel: true);
      }

      private IEnumerable<DataColumn> createLloqColumns(DataRepository observedData)
      {
         return observedData.Columns
            .Where(c => c.DataInfo.LLOQ != null)
            .Select(c => new DataColumn($"LLOQ_{c.Name}", c.Dimension, c.BaseGrid) { Value = Convert.ToDouble(c.DataInfo.LLOQ) });
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
         _executionContext.AddToHistory(new AddObservedDataToProjectCommand(new[] { observedData }).Run(_executionContext));
      }

      public void AddImporterConfigurationToProject(ImporterConfiguration configuration)
      {
         if (importerConfigurationAlreadyExistsInProject(configuration))
            return;

         _executionContext.AddToHistory(new AddImporterConfigurationToProjectCommand(configuration).Run(_executionContext));
      }

      public void SuppressWarningOnRemovingObservedDataEntryFromSimulation()
      {
         _confirmationManager.SuppressConfirmation(ConfirmationFlags.ObservedDataEntryRemoved);
      }

      public void RemoveUsedObservedDataFromSimulation(IReadOnlyList<UsedObservedData> usedObservedDataList)
      {
         if (!usedObservedDataList.Any())
            return;

         foreach (var usedObservedData in usedObservedDataList)
         {
            var parameterIdentifications = findParameterIdentificationsUsing(usedObservedData).ToList();
            if (!parameterIdentifications.Any())
               continue;

            _dialogCreator.MessageBoxInfo(
               Captions.ParameterIdentification.CannotRemoveObservedDataBeingUsedByParameterIdentification(observedDataFrom(usedObservedData).Name,
                  parameterIdentifications.AllNames().ToList()));
            return;
         }

         if (!_confirmationManager.IsConfirmationSuppressed(ConfirmationFlags.ObservedDataEntryRemoved))
         {
            var viewResult = _dialogCreator.MessageBoxConfirm(Captions.ReallyRemoveObservedDataFromSimulation,
               SuppressWarningOnRemovingObservedDataEntryFromSimulation);
            if (viewResult != ViewResult.Yes)
               return; 
         }

         usedObservedDataList.GroupBy(x => x.Simulation).Each(x => removeUsedObservedDataFromSimulation(x, x.Key));
      }

      private IEnumerable<ParameterIdentification> findParameterIdentificationsUsing(UsedObservedData usedObservedData)
      {
         var observedData = observedDataFrom(usedObservedData);
         var simulation = usedObservedData.Simulation;

         return from parameterIdentification in allParameterIdentifications()
            let outputMappings = parameterIdentification.AllOutputMappingsFor(simulation)
            where outputMappings.Any(x => x.UsesObservedData(observedData))
            select parameterIdentification;
      }

      private void removeUsedObservedDataFromSimulation(IEnumerable<UsedObservedData> usedObservedData, ISimulation simulation)
      {
         _executionContext.Load(simulation);

         var observedDataList = observedDataListFrom(usedObservedData);
         observedDataList.Each(simulation.RemoveUsedObservedData);
         observedDataList.Each(simulation.RemoveOutputMappings);

         _executionContext.PublishEvent(new ObservedDataRemovedFromAnalysableEvent(simulation, observedDataList));
         _executionContext.PublishEvent(new SimulationStatusChangedEvent(simulation));
      }

      private DataRepository observedDataFrom(UsedObservedData usedObservedData)
      {
         return _executionContext.Project.ObservedDataBy(usedObservedData.Id);
      }

      private IReadOnlyCollection<ParameterIdentification> allParameterIdentifications()
      {
         return _executionContext.Project.AllParameterIdentifications;
      }

      private IEnumerable<IUsesObservedData> allUsersOfObservedData(DataRepository observedData)
      {
         return _executionContext.Project.AllUsersOfObservedData.Where(x => x.UsesObservedData(observedData));
      }

      private IReadOnlyList<DataRepository> observedDataListFrom(IEnumerable<UsedObservedData> usedObservedData)
      {
         return usedObservedData.Select(observedDataFrom).ToList();
      }

      //TODO See if code can be merged between APPS
      public abstract void Rename(DataRepository observedData);
   }
}