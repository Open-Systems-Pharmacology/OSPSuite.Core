using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Events;
using Command = OSPSuite.Assets.Command;

namespace OSPSuite.Core.Commands
{
   public class RemoveObservedDataMetaDataCommand : AddOrRemoveObservedDataMetaDataAbstractCommand
   {
      public RemoveObservedDataMetaDataCommand(DataRepository observedData, MetaDataKeyValue metaDataKeyValue)
         : base(observedData, metaDataKeyValue)
      {
         CommandType = Command.CommandTypeDelete;
      }

      protected override void ExecuteWith(IOSPSuiteExecutionContext context)
      {
         removeMetaDataInRepository();

         Description = Command.SetMetaDataRemovedCommandDescription(_metaDataKeyValue.Key, _metaDataKeyValue.Value);

         SetBuildingBlockParameters(context);
         context.PublishEvent(new ObservedDataMetaDataRemovedEvent(_observedData));
      }

      private void removeMetaDataInRepository()
      {
         if (_observedData.ExtendedProperties.Contains(_metaDataKeyValue.Key))
            _observedData.ExtendedProperties.Remove(_metaDataKeyValue.Key);
      }

      protected override IReversibleCommand<IOSPSuiteExecutionContext> GetInverseCommand(IOSPSuiteExecutionContext context)
      {
         return new AddObservedDataMetaDataCommand(_observedData, new MetaDataKeyValue { Key = _metaDataKeyValue.Key, Value = _metaDataKeyValue.Value });
      }
   }
}