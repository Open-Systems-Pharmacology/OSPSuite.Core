using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Events;
using Command = OSPSuite.Assets.Command;

namespace OSPSuite.Core.Commands
{
   public class RemoveObservedDataMetaDataCommand : AddOrRemoveObservedDataMetaDataAbstractCommand
   {
      //Key and value as they were in the repository when removed. Null if the key was not found
      private MetaDataKeyValue _removedMetaData;

      public RemoveObservedDataMetaDataCommand(DataRepository observedData, MetaDataKeyValue metaDataKeyValue)
         : base(observedData, metaDataKeyValue)
      {
         CommandType = Command.CommandTypeDelete;
      }

      protected override void ExecuteWith(IOSPSuiteExecutionContext context)
      {
         _removedMetaData = removeMetaDataInRepository();

         var metaDataForDescription = _removedMetaData ?? _metaDataKeyValue;
         Description = Command.SetMetaDataRemovedCommandDescription(metaDataForDescription.Key, metaDataForDescription.Value);

         SetBuildingBlockParameters(context);
         context.PublishEvent(new ObservedDataMetaDataRemovedEvent(_observedData));
      }

      private MetaDataKeyValue removeMetaDataInRepository()
      {
         if (!_observedData.ExtendedProperties.Contains(_metaDataKeyValue.Key))
            return null;

         var removedMetaData = new MetaDataKeyValue { Key = _metaDataKeyValue.Key, Value = _observedData.ExtendedPropertyValueFor(_metaDataKeyValue.Key) };
         _observedData.ExtendedProperties.Remove(_metaDataKeyValue.Key);
         return removedMetaData;
      }

      protected override ICommand<IOSPSuiteExecutionContext> GetInverseCommand(IOSPSuiteExecutionContext context)
      {
         //Nothing was removed, so there is nothing to restore. Removing the absent key again is a no-op that remains reversible
         if (_removedMetaData == null)
            return new RemoveObservedDataMetaDataCommand(_observedData, _metaDataKeyValue).AsInverseFor(this);

         return new AddObservedDataMetaDataCommand(_observedData, _removedMetaData).AsInverseFor(this);
      }
   }
}