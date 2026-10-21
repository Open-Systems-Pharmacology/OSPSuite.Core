using OSPSuite.Core.Commands.Core;
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

      //Names and values as they were changed in the repository. Null if the meta data to change was not found
      private MetaDataChanged _appliedMetaDataChanged;

      public ChangeObservedDataMetaDataCommand(DataRepository observedData, MetaDataChanged metaDataChanged)
         : base(observedData)
      {
         _metaDataChanged = metaDataChanged;
         CommandType = Command.CommandTypeEdit;
      }

      protected override void ExecuteWith(IOSPSuiteExecutionContext context)
      {
         _appliedMetaDataChanged = changeMetaDataInRepository();

         updateMolecule(context);

         var metaDataChangedForDescription = _appliedMetaDataChanged ?? _metaDataChanged;
         Description = Command.SetMetaDataChangedCommandDescription(metaDataChangedForDescription.OldName, metaDataChangedForDescription.OldValue, metaDataChangedForDescription.NewName, metaDataChangedForDescription.NewValue);
         SetBuildingBlockParameters(context);
         context.PublishEvent(new ObservedDataMetaDataChangedEvent(_observedData));
      }

      private void updateMolecule(IOSPSuiteExecutionContext context)
      {
         if(_metaDataChanged.NewName != Constants.ObservedData.MOLECULE) return;

         var observedDataTask = context.Resolve<IObservedDataTask>();
         observedDataTask.UpdateMolWeight(_observedData);
      }

      private MetaDataChanged changeMetaDataInRepository()
      {
         if (!_observedData.ExtendedProperties.Contains(_metaDataChanged.OldName))
            return null;

         var oldValue = _observedData.ExtendedPropertyValueFor(_metaDataChanged.OldName);

         //A change that does not change the value (e.g. a rename) keeps the value of this repository,
         //which may differ from the value shared by all repositories edited together
         var newValue = string.Equals(_metaDataChanged.OldValue, _metaDataChanged.NewValue) ? oldValue : _metaDataChanged.NewValue;

         _observedData.ExtendedProperties.Remove(_metaDataChanged.OldName);
         _observedData.ExtendedProperties.Add(new ExtendedProperty<string> { Name = _metaDataChanged.NewName, Value = newValue });

         return new MetaDataChanged { OldName = _metaDataChanged.OldName, OldValue = oldValue, NewName = _metaDataChanged.NewName, NewValue = newValue };
      }

      protected override ICommand<IOSPSuiteExecutionContext> GetInverseCommand(IOSPSuiteExecutionContext context)
      {
         //Nothing was changed, so there is nothing to restore. Applying the same change again is a no-op that remains reversible
         if (_appliedMetaDataChanged == null)
            return new ChangeObservedDataMetaDataCommand(_observedData, _metaDataChanged).AsInverseFor(this);

         return new ChangeObservedDataMetaDataCommand(_observedData,
            new MetaDataChanged { NewName = _appliedMetaDataChanged.OldName, NewValue = _appliedMetaDataChanged.OldValue, OldValue = _appliedMetaDataChanged.NewValue, OldName = _appliedMetaDataChanged.NewName }).AsInverseFor(this);
      }
   }
}