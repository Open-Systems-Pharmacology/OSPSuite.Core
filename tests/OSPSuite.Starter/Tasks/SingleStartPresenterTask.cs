using System;
using OSPSuite.Core.Commands;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Services;

namespace OSPSuite.Starter.Tasks
{
   public class SingleStartPresenterTask : ISingleStartPresenterTask
   {
      private readonly IApplicationController _applicationController;

      public SingleStartPresenterTask(IApplicationController applicationController)
      {
         _applicationController = applicationController;
      }

      public void StartForSubject<T>(T subject)
      {
         var presenter = _applicationController.Open(subject, new OSPSuiteMacroCommand<OSPSuiteExecutionContext>());
         try
         {
            presenter.Edit(subject);
         }
         catch (Exception)
         {
            //exception while loading the subject. We need to close the presenter to avoid memory leaks
            _applicationController.Close(subject);
            throw;
         }
      }
   }
}