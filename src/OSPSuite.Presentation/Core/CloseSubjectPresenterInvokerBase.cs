using OSPSuite.Utility.Events;
using OSPSuite.Core.Events;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Core
{
   public interface ICloseSubjectPresenterInvokerBase : 
      IListener<ObservedDataRemovedEvent>,
      IListener<SensitivityAnalysisDeletedEvent>
   {
      
   }

   public abstract class CloseSubjectPresenterInvokerBase : ICloseSubjectPresenterInvokerBase
   {
      protected readonly IApplicationController _applicationController;

      protected CloseSubjectPresenterInvokerBase(IApplicationController applicationController)
      {
         _applicationController = applicationController;
      }

      protected void CloseAll() => _applicationController.CloseAll();

      protected void Close(object subject) => _applicationController.Close(subject);

      public void Handle(ObservedDataRemovedEvent eventToHandle) => eventToHandle.DataRepositories.Each(Close);

      public void Handle(SensitivityAnalysisDeletedEvent eventToHandle) => Close(eventToHandle.SensitivityAnalysis);
   }
}
