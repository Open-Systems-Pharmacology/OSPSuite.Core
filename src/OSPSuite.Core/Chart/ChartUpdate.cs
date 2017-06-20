using System;
using OSPSuite.Utility.Events;

namespace OSPSuite.Core.Chart
{
   public class ChartUpdate : IDisposable
   {
      private readonly IEventPublisher _eventPublisher;
      private readonly IChart _chart;
      private readonly bool _propogateChartChangeEvent;

      public ChartUpdate(IEventPublisher eventPublisher, IChart chart, bool propogateChartChangeEvent)
      {
         _eventPublisher = eventPublisher;
         _chart = chart;
         _propogateChartChangeEvent = propogateChartChangeEvent;
      }

      public void Dispose()
      {
         _eventPublisher.PublishEvent(new ChartUpdatedEvent(_chart,_propogateChartChangeEvent));
      }
   }
}