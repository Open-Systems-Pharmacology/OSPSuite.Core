using System;
using OSPSuite.Utility.Events;

namespace OSPSuite.Core.Chart
{
   public class ChartUpdate : IDisposable
   {
      private readonly IEventPublisher _eventPublisher;
      private readonly IChart _chart;

      public ChartUpdate(IEventPublisher eventPublisher, IChart chart)
      {
         _eventPublisher = eventPublisher;
         _chart = chart;
      }

      public void Dispose()
      {
         _eventPublisher.PublishEvent(new ChartUpdatedEvent(_chart));
      }
   }
}