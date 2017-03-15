using OSPSuite.Core.Domain.Data;

namespace OSPSuite.Core.Commands
{
   public class MetaDataKeyValue
   {
      public string Key { set; get; }
      public string Value { set; get; }
   }

   public class MetaDataChanged
   {
      public string OldName { set; get; }
      public string OldValue { set; get; }

      public string NewName { set; get; }
      public string NewValue { set; get; }
   }

   public abstract class AddOrRemoveObservedDataMetaDataAbstractCommand : ObservedDataCommandBase
   {
      protected readonly MetaDataKeyValue _metaDataKeyValue;

      protected AddOrRemoveObservedDataMetaDataAbstractCommand(DataRepository observedData, MetaDataKeyValue metaDataKeyValue)
         : base(observedData)
      {
         _metaDataKeyValue = metaDataKeyValue;
         _observedDataId = _observedData.Id;
      }
   }
}