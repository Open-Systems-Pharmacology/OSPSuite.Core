using System.Collections.Generic;

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

   public class CurveChartUpdatedEvent : ChartUpdatedEvent
   {
      public IReadOnlyCollection<Curve> CurvesToUpdate { get; }
      public bool CurveDataChanged { get; }

      public CurveChartUpdatedEvent(CurveChart chart, IReadOnlyCollection<Curve> curvesToUpdate, bool curveDataChanged, bool propagateChartChangeEvent) : base(chart, propagateChartChangeEvent)
      {
         CurvesToUpdate = curvesToUpdate;
         CurveDataChanged = curveDataChanged;
      }
   }

   public class ChartPropertiesChangedEvent : ChartEvent
   {
      public ChartPropertiesChangedEvent(IChart chart) : base(chart)
      {
      }
   }
}