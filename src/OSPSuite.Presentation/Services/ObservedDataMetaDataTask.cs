using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters.ObservedData;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Utility.Extensions;
using Command = OSPSuite.Assets.Command;

namespace OSPSuite.Presentation.Services
{
   public class ObservedDataMetaDataTask : IObservedDataMetaDataTask
   {
      private readonly IOSPSuiteExecutionContext _executionContext;
      private readonly IApplicationController _applicationController;
      private readonly IParameterIdentificationTask _parameterIdentificationTask;
      private readonly IDimension _molWeightDimension;

      public ObservedDataMetaDataTask(IOSPSuiteExecutionContext executionContext, IApplicationController applicationController, IDimensionFactory dimensionFactory, IParameterIdentificationTask parameterIdentificationTask)
      {
         _executionContext = executionContext;
         _applicationController = applicationController;
         _parameterIdentificationTask = parameterIdentificationTask;
         _molWeightDimension = dimensionFactory.Dimension(Constants.Dimension.MOLECULAR_WEIGHT);
      }

      public ICommand AddMetaData(IEnumerable<DataRepository> observedData, MetaDataKeyValue metaDataKeyValue)
      {
         var macroCommand = new OSPSuiteMacroCommand<IOSPSuiteExecutionContext>
         {
            CommandType = Command.CommandTypeAdd,
            ObjectType = ObjectTypes.ObservedData,
            Description = Command.MetaDataAddedToDataRepositories
         };

         observedData.Each(x => macroCommand.Add(new AddObservedDataMetaDataCommand(x, metaDataKeyValue).Run(_executionContext)));
         return macroCommand;
      }

      public ICommand RemoveMetaData(IEnumerable<DataRepository> observedData, MetaDataKeyValue metaDataKeyValue)
      {
         var macroCommand = new OSPSuiteMacroCommand<IOSPSuiteExecutionContext>
         {
            CommandType = Command.CommandTypeDelete,
            ObjectType = ObjectTypes.ObservedData,
            Description = Command.MetaDataRemovedFromDataRepositories
         };

         observedData.Each(x => macroCommand.Add(new RemoveObservedDataMetaDataCommand(x, metaDataKeyValue).Run(_executionContext)));
         return macroCommand;
      }

      public ICommand ChangeMetaData(IEnumerable<DataRepository> observedData, MetaDataChanged metaDataChanged)
      {
         var macroCommand = new OSPSuiteMacroCommand<IOSPSuiteExecutionContext>
         {
            CommandType = Command.CommandTypeEdit,
            ObjectType = ObjectTypes.ObservedData,
            Description = Command.MetaDataModifiedInDataRepositories
         };

         observedData.Each(x => macroCommand.Add(new ChangeObservedDataMetaDataCommand(x, metaDataChanged).Run(_executionContext)));
         return macroCommand;
      }

      public IReadOnlyList<string> ParameterIdentificationsUsingDataRepository(DataRepository observedData)
      {
         return _parameterIdentificationTask.ParameterIdentificationsUsingObservedData(observedData).Select(x => x.Name).ToList();
      }

      public void EditMultipleMetaDataFor(IEnumerable<DataRepository> dataRepositories)
      {
         using (var editPresenter = _applicationController.Start<IEditMultipleDataRepositoriesMetaDataPresenter>())
         {
            _executionContext.AddToHistory(editPresenter.Edit(dataRepositories));
         }
      }

      public ICommand UpdateMolWeight(IEnumerable<DataRepository> allDataRepositories, double oldMolWeightValue,  double newMolWeightValue)
      {
         if (ValueComparer.AreValuesEqual(oldMolWeightValue, newMolWeightValue))
            return new OSPSuiteEmptyCommand<IOSPSuiteExecutionContext>();

         var macroCommand = new OSPSuiteMacroCommand<IOSPSuiteExecutionContext>
         {
            CommandType = Command.CommandTypeEdit,
            ObjectType = ObjectTypes.ObservedData,
            Description = Command.MolecularWeightModifiedInDataRepositories
         };

         allDataRepositories.Each(x => macroCommand.Add(new UpdateObservedDataMolWeightCommand(x, _molWeightDimension, oldMolWeightValue, newMolWeightValue).Run(_executionContext)));
         return macroCommand;
      }

      public ICommand UpdateSubjectCount(IEnumerable<DataRepository> allDataRepositories, string subjectCount)
      {
         var newSubjectCount = normalizedSubjectCount(subjectCount);

         var macroCommand = new OSPSuiteMacroCommand<IOSPSuiteExecutionContext>
         {
            CommandType = Command.CommandTypeEdit,
            ObjectType = ObjectTypes.ObservedData,
            Description = Command.SubjectCountModifiedInDataRepositories
         };

         foreach (var dataRepository in allDataRepositories)
         {
            var command = updateSubjectCountCommand(dataRepository, newSubjectCount);
            if (command != null)
               macroCommand.Add(command.Run(_executionContext));
         }

         return macroCommand;
      }

      /// <summary>
      ///    Returns the command updating the subject count of <paramref name="dataRepository" /> to
      ///    <paramref name="newSubjectCount" /> or <c>null</c> if the subject count is already as expected
      /// </summary>
      private ObservedDataCommandBase updateSubjectCountCommand(DataRepository dataRepository, string newSubjectCount)
      {
         var key = Constants.ObservedData.SUBJECT_COUNT;
         if (!dataRepository.ExtendedProperties.Contains(key))
            return newSubjectCount == null ? null : new AddObservedDataMetaDataCommand(dataRepository, new MetaDataKeyValue {Key = key, Value = newSubjectCount});

         var oldSubjectCount = dataRepository.ExtendedPropertyValueFor(key);
         if (newSubjectCount == null)
            return new RemoveObservedDataMetaDataCommand(dataRepository, new MetaDataKeyValue {Key = key, Value = oldSubjectCount});

         if (string.Equals(oldSubjectCount, newSubjectCount))
            return null;

         return new ChangeObservedDataMetaDataCommand(dataRepository, new MetaDataChanged {OldName = key, NewName = key, OldValue = oldSubjectCount, NewValue = newSubjectCount});
      }

      /// <summary>
      ///    Returns <c>null</c> for an empty <paramref name="subjectCount" />, otherwise the subject count without leading zeros, sign
      ///    or white spaces
      /// </summary>
      private static string normalizedSubjectCount(string subjectCount)
      {
         if (string.IsNullOrWhiteSpace(subjectCount))
            return null;

         if (!DataRepositoryExtensions.IsValidSubjectCount(subjectCount))
            throw new OSPSuiteException(Error.InvalidSubjectCount(subjectCount, Constants.ObservedData.MIN_SUBJECT_COUNT));

         return int.Parse(subjectCount, NumberStyles.Integer, CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture);
      }
   }

}