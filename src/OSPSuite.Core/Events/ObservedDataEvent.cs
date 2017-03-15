using OSPSuite.Core.Domain.Data;

namespace OSPSuite.Core.Events
{
   public abstract class ObservedDataEvent
   {
      public DataRepository ObservedData { get; private set; }

      protected ObservedDataEvent(DataRepository observedData)
      {
         ObservedData = observedData;
      }
   }

   public class ObservedDataValueChangedEvent : ObservedDataEvent
   {
      public ObservedDataValueChangedEvent(DataRepository observedData)
         : base(observedData)
      {
      }
   }

   public class ObservedDataTableChangedEvent : ObservedDataEvent
   {
      public ObservedDataTableChangedEvent(DataRepository observedData)
         : base(observedData)
      {
      }
   }
   public class ObservedDataMetaDataAddedEvent : ObservedDataEvent
   {
      public ObservedDataMetaDataAddedEvent(DataRepository observedData)
         : base(observedData)
      {

      }
   }

   public class ObservedDataMetaDataRemovedEvent : ObservedDataEvent
   {
      public ObservedDataMetaDataRemovedEvent(DataRepository observedData)
         : base(observedData)
      {

      }
   }

   public class ObservedDataMetaDataChangedEvent : ObservedDataEvent
   {
      public ObservedDataMetaDataChangedEvent(DataRepository observedData)
         : base(observedData)
      {
      }
   }
}