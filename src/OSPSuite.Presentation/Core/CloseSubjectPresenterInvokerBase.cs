using OSPSuite.Utility.Events;
using OSPSuite.Core.Events;

namespace OSPSuite.Presentation.Core
{
   public interface ICloseSubjectPresenterInvokerBase : IListener<ObservedDataRemovedEvent>
   {
      
   }

   public abstract class CloseSubjectPresenterInvokerBase : ICloseSubjectPresenterInvokerBase
   {
      protected readonly IApplicationController _applicationController;

      protected CloseSubjectPresenterInvokerBase(IApplicationController applicationController)
      {
         _applicationController = applicationController;
      }

      protected void CloseAll()
      {
         _applicationController.CloseAll();
      }

      protected void Close(object subject)
      {
         _applicationController.Close(subject);
      }

      public void Handle(ObservedDataRemovedEvent eventToHandle)
      {
         Close(eventToHandle.DataRepository);
      }
   }
}
