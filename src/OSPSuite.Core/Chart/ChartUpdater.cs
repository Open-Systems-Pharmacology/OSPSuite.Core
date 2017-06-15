using OSPSuite.Utility.Events;

namespace OSPSuite.Core.Chart
{
   public interface IChartUpdater
   {
      ChartUpdate UpdateTransaction(IChart chart);
      void Update(IChart chart);
   }

   public class ChartUpdater : IChartUpdater
   {
      private readonly IEventPublisher _eventPublisher;

      public ChartUpdater(IEventPublisher eventPublisher)
      {
         _eventPublisher = eventPublisher;
      }

      public ChartUpdate UpdateTransaction(IChart chart)
      {
         return new ChartUpdate(_eventPublisher, chart);
      }

      public void Update(IChart chart)
      {
         using (UpdateTransaction(chart))
         {
         }
      }
   }
}