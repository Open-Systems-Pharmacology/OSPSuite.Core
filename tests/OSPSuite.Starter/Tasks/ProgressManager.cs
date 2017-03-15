using System.Threading;
using OSPSuite.Utility.Events;

namespace OSPSuite.Starter.Tasks
{
   internal class ProgressManager : IProgressManager
   {
      private readonly IEventPublisher _eventPublisher;

      public ProgressManager()
      {
         _eventPublisher = new EventPublisher(new SynchronizationContext(), new SimpleExceptionManager());
      }

      public IProgressUpdater Create()
      {
         return new ProgressUpdater(_eventPublisher);
      }
   }
}