using System;
using OSPSuite.Utility.Events;

namespace OSPSuite.Core.Chart
{
   public class ChartUpdate : IDisposable
   {
      private readonly IEventPublisher _eventPublisher;
      private readonly IChart _chart;
      private readonly bool _propagateChartChangeEvent;

      public ChartUpdate(IEventPublisher eventPublisher, IChart chart, bool propagateChartChangeEvent)
      {
         _eventPublisher = eventPublisher;
         _chart = chart;
         _propagateChartChangeEvent = propagateChartChangeEvent;
      }

      public void Dispose()
      {
         _eventPublisher.PublishEvent(new ChartUpdatedEvent(_chart,_propagateChartChangeEvent));
      }
   }
}