using System;
using System.Collections.Generic;
using OSPSuite.Utility.Events;

namespace OSPSuite.Core.Chart
{
   public abstract class ChartUpdate<TChart> : IDisposable
   {
      protected readonly TChart _chart;
      protected readonly IEventPublisher _eventPublisher;
      protected readonly bool _propagateChartChangeEvent;

      protected ChartUpdate(IEventPublisher eventPublisher, TChart chart, bool propagateChartChangeEvent)
      {
         _eventPublisher = eventPublisher;
         _chart = chart;
         _propagateChartChangeEvent = propagateChartChangeEvent;
      }

      public abstract void Dispose();
   }

   public class ChartUpdate : ChartUpdate<IChart>
   {
      public ChartUpdate(IEventPublisher eventPublisher, IChart chart, bool propagateChartChangeEvent) : base(eventPublisher, chart, propagateChartChangeEvent)
      {
      }

      public override void Dispose() => 
         _eventPublisher.PublishEvent(new ChartUpdatedEvent(_chart, _propagateChartChangeEvent));
   }

   public class CurveChartUpdate : ChartUpdate<CurveChart>
   {
      private readonly Func<CurveChart, IReadOnlyCollection<Curve>> _calculateModifiedCurves;
      private readonly bool _curveDataChanged;

      public CurveChartUpdate(IEventPublisher eventPublisher, CurveChart chart, Func<CurveChart, IReadOnlyCollection<Curve>> calculateModifiedCurves, bool curveDataChanged, bool propagateChartChangeEvent) : base(eventPublisher, chart, propagateChartChangeEvent)
      {
         _calculateModifiedCurves = calculateModifiedCurves;
         _curveDataChanged = curveDataChanged;
      }

      public override void Dispose() => 
         _eventPublisher.PublishEvent(new CurveChartUpdatedEvent(_chart, _calculateModifiedCurves(_chart), _curveDataChanged, _propagateChartChangeEvent));
   }
}