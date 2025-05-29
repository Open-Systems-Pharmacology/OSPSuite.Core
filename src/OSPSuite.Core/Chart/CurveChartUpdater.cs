using System;
using System.Collections.Generic;
using OSPSuite.Utility.Events;

namespace OSPSuite.Core.Chart
{
   public interface ICurveChartUpdater
   {
      /// <summary>
      /// Use to prevent <paramref name="chart"/> from updating during the transaction. If <paramref name="calculateModifiedCurves"/> is given, it will be used to calculate modified curves to be updated
      /// If not given, all curves will be updated.
      /// </summary>
      CurveChartUpdate UpdateTransaction(CurveChart chart, bool curveDataChanged, Func<CurveChart, IReadOnlyCollection<Curve>> calculateModifiedCurves = null, bool propagateChartChangeEvent = true);

      /// <summary>
      /// The <paramref name="chart"/> being updated will not recalculate and repaint during the update. Specific curves that need to be recalculated should be specified
      /// by <paramref name="calculateModifiedCurves"/>. If the parameter is not given, then all curves will be recalculated
      /// </summary>
      void Update(CurveChart chart, bool curveDataChanged, Func<CurveChart, IReadOnlyCollection<Curve>> calculateModifiedCurves = null);
   }

   public class CurveChartUpdater : ICurveChartUpdater
   {
      private readonly IEventPublisher _eventPublisher;

      public CurveChartUpdater(IEventPublisher eventPublisher)
      {
         _eventPublisher = eventPublisher;
      }

      public CurveChartUpdate UpdateTransaction(CurveChart chart, bool curveDataChanged, Func<CurveChart, IReadOnlyCollection<Curve>> calculateModifiedCurves = null, bool propagateChartChangeEvent = true)
      {
         // default update all curves
         if (calculateModifiedCurves == null)
            calculateModifiedCurves = x => x.Curves;

         return new CurveChartUpdate(_eventPublisher, chart, calculateModifiedCurves, curveDataChanged, propagateChartChangeEvent);
      }

      public void Update(CurveChart chart, bool curveDataChanged, Func<CurveChart, IReadOnlyCollection<Curve>> calculateModifiedCurves)
      {
         using (UpdateTransaction(chart, curveDataChanged, calculateModifiedCurves))
         {
         }
      }
   }
}