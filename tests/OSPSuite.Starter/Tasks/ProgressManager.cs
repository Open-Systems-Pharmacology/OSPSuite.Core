using System.Threading;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Exceptions;

namespace OSPSuite.Starter.Tasks
{
   internal class ProgressManager : IProgressManager
   {
      private readonly IEventPublisher _eventPublisher;

      public ProgressManager(IExceptionManager exceptionManager)
      {
         _eventPublisher = new EventPublisher(new SynchronizationContext(), exceptionManager);
      }

      public IProgressUpdater Create()
      {
         return new ProgressUpdater(_eventPublisher);
      }
   }
}