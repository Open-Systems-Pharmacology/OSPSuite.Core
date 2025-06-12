using OSPSuite.Utility.Events;
using System.Collections.Generic;

namespace OSPSuite.Core.Chart
{
   public interface ICurveChartUpdater
   {
      /// <summary>
      /// Use to prevent <paramref name="chart"/> from updating during the transaction.
      /// </summary>
      CurveChartUpdate UpdateTransaction(CurveChart chart, CurveChartUpdateModes mode, bool propagateChartChangeEvent = true);
      
      /// <summary>
      /// Use to prevent <paramref name="chart"/> from updating during the transaction.
      /// </summary>
      CurveChartUpdate UpdateTransaction(CurveChart chart, IReadOnlyList<Curve> updatedCurves, CurveChartUpdateModes mode, bool propagateChartChangeEvent = true);
      
      /// <summary>
      /// The <paramref name="chart"/> being updated will not recalculate and repaint during the update.
      /// </summary>
      void Update(CurveChart chart, CurveChartUpdateModes mode);

      /// <summary>
      /// The <paramref name="chart"/> being updated will not recalculate and repaint during the update.
      /// </summary>
      void Update(CurveChart chart, IReadOnlyList<Curve> updatedCurves, CurveChartUpdateModes mode);
   }

   public class CurveChartUpdater : ICurveChartUpdater
   {
      private readonly IEventPublisher _eventPublisher;

      public CurveChartUpdater(IEventPublisher eventPublisher)
      {
         _eventPublisher = eventPublisher;
      }

      public CurveChartUpdate UpdateTransaction(CurveChart chart, CurveChartUpdateModes mode, bool propagateChartChangeEvent = true)
      {
         switch(mode)
         {
            case CurveChartUpdateModes.Add:
               return new CurveChartAddUpdate(_eventPublisher, chart, propagateChartChangeEvent);
            case CurveChartUpdateModes.Remove:
               return new CurveChartRemoveUpdate(_eventPublisher, chart, propagateChartChangeEvent);
            default:
               return new CurveChartAllUpdate(_eventPublisher, chart, propagateChartChangeEvent);
         }
      }

      public CurveChartUpdate UpdateTransaction(CurveChart chart, IReadOnlyList<Curve> updatedCurves, CurveChartUpdateModes mode, bool propagateChartChangeEvent = true)
      {
         return new CurveChartSelectedUpdate(_eventPublisher, chart, propagateChartChangeEvent, updatedCurves, mode);
      }

      public void Update(CurveChart chart, CurveChartUpdateModes mode)
      {
         using (UpdateTransaction(chart, mode))
         {
         }
      }

      public void Update(CurveChart chart, IReadOnlyList<Curve> updatedCurves, CurveChartUpdateModes mode)
      {
         using (UpdateTransaction(chart, updatedCurves, mode))
         {
         }
      }
   }
}