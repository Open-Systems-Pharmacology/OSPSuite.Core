using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters.ObservedData;
using Command = OSPSuite.Assets.Command;

namespace OSPSuite.Presentation.Services
{
   public class EditObservedDataTask : IEditObservedDataTask
   {
      private readonly IOSPSuiteExecutionContext _executionContext;
      private readonly IApplicationController _applicationController;
      private readonly IParameterIdentificationTask _parameterIdentificationTask;
      private readonly IDimension _molWeightDimension;

      public EditObservedDataTask(IOSPSuiteExecutionContext executionContext, IApplicationController applicationController, IDimensionFactory dimensionFactory, IParameterIdentificationTask parameterIdentificationTask)
      {
         _executionContext = executionContext;
         _applicationController = applicationController;
         _parameterIdentificationTask = parameterIdentificationTask;
         _molWeightDimension = dimensionFactory.Dimension(Constants.Dimension.MOLECULAR_WEIGHT);
      }

      public ICommand SetValue(DataRepository observedData, CellValueChanged cellValueChanged)
      {
         return new SetObservedDataValueCommand(observedData, cellValueChanged).Run(_executionContext);
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

      public ICommand SetUnit(DataRepository dataRepository, string columnId, Unit newUnit)
      {
         return new SetObservedDataColumnUnitCommand(dataRepository, columnId, newUnit).Run(_executionContext);
      }

      public ICommand RemoveValue(DataRepository observedData, int dataRowIndex)
      {
         var identificationsUsingDataRepository = ParameterIdentificationsUsingDataRepository(observedData);

         if (!identificationsUsingDataRepository.Any())
            return new RemoveObservedDataRowCommand(observedData, dataRowIndex).Run(_executionContext);

         throw new OSPSuiteException(Captions.ParameterIdentification.CannotDeleteObservedDataPointBeingUsedByParameterIdentification(observedData.Name, identificationsUsingDataRepository));
      }

      public IReadOnlyList<string> ParameterIdentificationsUsingDataRepository(DataRepository observedData)
      {
         return _parameterIdentificationTask.ParameterIdentificationsUsingObservedData(observedData).Select(x => x.Name).ToList();
      }

      public ICommand AddValue(DataRepository observedData, DataRowData dataRowAdded)
      {
         var identificationsUsingDataRepository = ParameterIdentificationsUsingDataRepository(observedData);

         if (!identificationsUsingDataRepository.Any())
            return new AddObservedDataRowCommand(observedData, dataRowAdded).Run(_executionContext);

         throw new OSPSuiteException(Captions.ParameterIdentification.CannotAddObservedDataPointBeingUsedByParameterIdentification(observedData.Name, identificationsUsingDataRepository));
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
   }

}