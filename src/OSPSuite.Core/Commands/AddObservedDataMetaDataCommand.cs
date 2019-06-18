using System;
using OSPSuite.Assets;
using OSPSuite.Core.Commands.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Events;
using Command = OSPSuite.Assets.Command;

namespace OSPSuite.Core.Commands
{
   public class AddObservedDataMetaDataCommand : AddOrRemoveObservedDataMetaDataAbstractCommand
   {
      public AddObservedDataMetaDataCommand(DataRepository observedData, MetaDataKeyValue metaDataKeyValue)
         : base(observedData, metaDataKeyValue)
      {
         CommandType = Command.CommandTypeAdd;
      }

      protected override void ExecuteWith(IOSPSuiteExecutionContext context)
      {
         setMetaDataInRepository();

         Description = Command.SetMetaDataAddedCommandDescription(_metaDataKeyValue.Key, _metaDataKeyValue.Value);
         SetBuildingBlockParameters(context);
         context.PublishEvent(new ObservedDataMetaDataAddedEvent(_observedData));
      }

      private void setMetaDataInRepository()
      {
         if (_observedData.ExtendedProperties.Contains(_metaDataKeyValue.Key))
            throw new ArgumentException(Error.CannotAddMetaDataDuplicateKey(_metaDataKeyValue.Key, _observedData.ExtendedPropertyValueFor(_metaDataKeyValue.Key)));

         _observedData.ExtendedProperties.Add(new ExtendedProperty<string> { Name = _metaDataKeyValue.Key, Value = _metaDataKeyValue.Value });
      }

      protected override IReversibleCommand<IOSPSuiteExecutionContext> GetInverseCommand(IOSPSuiteExecutionContext context)
      {
         return new RemoveObservedDataMetaDataCommand(_observedData,
            new MetaDataKeyValue { Key = _metaDataKeyValue.Key, Value = _metaDataKeyValue.Value }).AsInverseFor(this);
      }
   }
}