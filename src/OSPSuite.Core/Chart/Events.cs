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
      public bool PropagateChartChangeEvent { get; }

      public ChartUpdatedEvent(IChart chart, bool propagateChartChangeEvent) : base(chart)
      {
         PropagateChartChangeEvent = propagateChartChangeEvent;
      }
   }

   public class ChartPropertiesChangedEvent : ChartEvent
   {
      public ChartPropertiesChangedEvent(IChart chart) : base(chart)
      {
      }
   }
}