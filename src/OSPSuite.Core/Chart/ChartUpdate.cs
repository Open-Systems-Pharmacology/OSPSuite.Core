using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Events;

namespace OSPSuite.Core.Chart
{
   public enum CurveChartUpdateModes
   {
      All,
      Add,
      Remove
   }

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

   public class CurveChartAddUpdate : CurveChartUpdate
   {
      public CurveChartAddUpdate(IEventPublisher eventPublisher, CurveChart chart, bool refreshCurveData, bool propagateChartChangeEvent) : base(eventPublisher, chart, refreshCurveData, propagateChartChangeEvent)
      {
      }

      protected override IReadOnlyCollection<Curve> GetModifiedCurves()
      {
         return _chart.Curves.Except(_originalCurves).ToList();
      }
   }

   public class CurveChartRemoveUpdate : CurveChartUpdate
   {
      public CurveChartRemoveUpdate(IEventPublisher eventPublisher, CurveChart chart, bool refreshCurveData, bool propagateChartChangeEvent) : base(eventPublisher, chart, refreshCurveData, propagateChartChangeEvent)
      {
      }

      protected override IReadOnlyCollection<Curve> GetModifiedCurves()
      {
         return _originalCurves.Except(_chart.Curves).ToList();
      }
   }

   public class CurveChartAllUpdate : CurveChartUpdate
   {
      public CurveChartAllUpdate(IEventPublisher eventPublisher, CurveChart chart, bool refreshCurveData, bool propagateChartChangeEvent) : base(eventPublisher, chart, refreshCurveData, propagateChartChangeEvent)
      {
      }

      protected override IReadOnlyCollection<Curve> GetModifiedCurves()
      {
         return _chart.Curves;
      }
   }

   public class CurveChartSelectedUpdate : CurveChartUpdate
   {
      private readonly IReadOnlyList<Curve> _updatedCurves;

      public CurveChartSelectedUpdate(IEventPublisher eventPublisher, CurveChart chart, bool refreshCurveData, bool propagateChartChangeEvent, IReadOnlyList<Curve> updatedCurves) : base(eventPublisher, chart, refreshCurveData, propagateChartChangeEvent)
      {
         _updatedCurves = updatedCurves;
      }

      protected override IReadOnlyCollection<Curve> GetModifiedCurves()
      {
         return _updatedCurves;
      }
   }

   public abstract class CurveChartUpdate : ChartUpdate<CurveChart>
   {
      private readonly bool _refreshCurveData;
      protected readonly List<Curve> _originalCurves;

      public CurveChartUpdate(IEventPublisher eventPublisher, CurveChart chart, bool refreshCurveData, bool propagateChartChangeEvent) : base(eventPublisher, chart, propagateChartChangeEvent)
      {
         _originalCurves = chart.Curves.ToList();
         _refreshCurveData = refreshCurveData;
      }

      public override void Dispose()
      {
         _eventPublisher.PublishEvent(new CurveChartUpdatedEvent(_chart, GetModifiedCurves(), _refreshCurveData, _propagateChartChangeEvent));
      }

      protected abstract IReadOnlyCollection<Curve> GetModifiedCurves();
   }
}