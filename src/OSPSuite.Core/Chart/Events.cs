namespace OSPSuite.Core.Chart
{
   public abstract class ChartEvent
   {
      public IChart Chart { get; }

      protected ChartEvent(IChart chart)
      {
         Chart = chart;
      }
   }

   public class ChartUpdatedEvent : ChartEvent
   {
      public ChartUpdatedEvent(IChart chart) : base(chart)
      {
      }
   }

   public class ChartPropertiesChangedEvent : ChartEvent
   {
      public ChartPropertiesChangedEvent(IChart chart) : base(chart)
      {
      }
   }

}