using OSPSuite.Utility.Events;

namespace OSPSuite.Presentation.Presenters
{
   public interface IReleasable
   {
      void ReleaseFrom(IEventPublisher eventPublisher);
   }
}