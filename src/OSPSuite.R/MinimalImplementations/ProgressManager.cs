using OSPSuite.Utility.Events;

namespace OSPSuite.R.MinimalImplementations
{
   public class ProgressManager : IProgressManager
   {
      private readonly IEventPublisher _eventPublisher;

      public ProgressManager(IEventPublisher eventPublisher)
      {
         _eventPublisher = eventPublisher;
      }

      public IProgressUpdater Create()
      {
         return new ProgressUpdater(_eventPublisher);
      }
   }
}