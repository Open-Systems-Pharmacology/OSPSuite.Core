using OSPSuite.Core.Domain;
using OSPSuite.Utility.Events;
using System;
using System.Collections.Generic;

namespace OSPSuite.Core.Chart
{
   public interface ICurveChartUpdater
   {
      /// <summary>
      /// Use to prevent <paramref name="chart"/> from updating during the transaction.
      /// </summary>
      CurveChartUpdate UpdateTransaction(CurveChart chart, bool refreshCurveData, CurveChartUpdateModes mode = CurveChartUpdateModes.All, bool propagateChartChangeEvent = true);
      
      /// <summary>
      /// Use to prevent <paramref name="chart"/> from updating during the transaction.
      /// </summary>
      CurveChartUpdate UpdateTransaction(CurveChart chart, bool refreshCurveData, IReadOnlyList<Curve> updatedCurves, bool propagateChartChangeEvent = true);
      
      /// <summary>
      /// The <paramref name="chart"/> being updated will not recalculate and repaint during the update.
      /// </summary>
      void Update(CurveChart chart, bool refreshCurveData, CurveChartUpdateModes mode = CurveChartUpdateModes.All);

      /// <summary>
      /// The <paramref name="chart"/> being updated will not recalculate and repaint during the update.
      /// </summary>
      void Update(CurveChart chart, bool refreshCurveData, IReadOnlyList<Curve> updatedCurves);
   }

   public class CurveChartUpdater : ICurveChartUpdater
   {
      private readonly IEventPublisher _eventPublisher;

      public CurveChartUpdater(IEventPublisher eventPublisher)
      {
         _eventPublisher = eventPublisher;
      }

      public CurveChartUpdate UpdateTransaction(CurveChart chart, bool refreshCurveData, CurveChartUpdateModes mode = CurveChartUpdateModes.All, bool propagateChartChangeEvent = true)
      {
         switch(mode)
         {
            case CurveChartUpdateModes.Add:
               return new CurveChartAddUpdate(_eventPublisher, chart, refreshCurveData, propagateChartChangeEvent);
            case CurveChartUpdateModes.Remove:
               return new CurveChartRemoveUpdate(_eventPublisher, chart, refreshCurveData, propagateChartChangeEvent);
            default:
               return new CurveChartAllUpdate(_eventPublisher, chart, refreshCurveData, propagateChartChangeEvent);
         }
      }

      public CurveChartUpdate UpdateTransaction(CurveChart chart, bool refreshCurveData, IReadOnlyList<Curve> updatedCurves, bool propagateChartChangeEvent = true)
      {
         return new CurveChartSelectedUpdate(_eventPublisher, chart, refreshCurveData, propagateChartChangeEvent, updatedCurves);
      }

      public void Update(CurveChart chart, bool refreshCurveData, CurveChartUpdateModes mode = CurveChartUpdateModes.All)
      {
         using (UpdateTransaction(chart, refreshCurveData, mode))
         {
         }
      }

      public void Update(CurveChart chart, bool refreshCurveData, IReadOnlyList<Curve> updatedCurves)
      {
         using (UpdateTransaction(chart, refreshCurveData, updatedCurves))
         {
         }
      }


   }
}