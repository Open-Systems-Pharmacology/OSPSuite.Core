﻿using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Events;
using Command = OSPSuite.Assets.Command;

namespace OSPSuite.Core.Commands
{
   public class ChangeObservedDataMetaDataCommand : ObservedDataCommandBase
   {
      private readonly MetaDataChanged _metaDataChanged;

      public ChangeObservedDataMetaDataCommand(DataRepository observedData, MetaDataChanged metaDataChanged)
         : base(observedData)
      {
         _metaDataChanged = metaDataChanged;
         CommandType = Command.CommandTypeEdit;
      }

      protected override void ExecuteWith(IOSPSuiteExecutionContext context)
      {
         changeMetaDataInRepository();

         updateMolecule(context);

         Description = Command.SetMetaDataChangedCommandDescription(_metaDataChanged.OldName, _metaDataChanged.OldValue, _metaDataChanged.NewName, _metaDataChanged.NewValue);
         SetBuildingBlockParameters(context);
         context.PublishEvent(new ObservedDataMetaDataChangedEvent(_observedData));
      }

      private void updateMolecule(IOSPSuiteExecutionContext context)
      {
         if(_metaDataChanged.NewName != Constants.ObservedData.MOLECULE) return;

         var observedDataTask = context.Resolve<IObservedDataTask>();
         observedDataTask.UpdateMolWeight(_observedData);
      }

      private void changeMetaDataInRepository()
      {
         if (!_observedData.ExtendedProperties.Contains(_metaDataChanged.OldName)) return;

         _observedData.ExtendedProperties.Remove(_metaDataChanged.OldName);
         _observedData.ExtendedProperties.Add(new ExtendedProperty<string> { Name = _metaDataChanged.NewName, Value = _metaDataChanged.NewValue });
      }

      protected override ICommand<IOSPSuiteExecutionContext> GetInverseCommand(IOSPSuiteExecutionContext context)
      {
         return new ChangeObservedDataMetaDataCommand(_observedData,
            new MetaDataChanged { NewName = _metaDataChanged.OldName, NewValue = _metaDataChanged.OldValue, OldValue = _metaDataChanged.NewValue, OldName = _metaDataChanged.NewName });
      }
   }
}