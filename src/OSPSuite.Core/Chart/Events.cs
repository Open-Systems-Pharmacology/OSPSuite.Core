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
      public bool PropogateChartChangeEvent { get; }

      public ChartUpdatedEvent(IChart chart, bool propogateChartChangeEvent) : base(chart)
      {
         PropogateChartChangeEvent = propogateChartChangeEvent;
      }
   }

   public class ChartPropertiesChangedEvent : ChartEvent
   {
      public ChartPropertiesChangedEvent(IChart chart) : base(chart)
      {
      }
   }
}