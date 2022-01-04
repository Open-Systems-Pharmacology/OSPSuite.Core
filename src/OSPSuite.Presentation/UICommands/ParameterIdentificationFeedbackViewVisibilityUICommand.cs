using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Events;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Services;
using OSPSuite.Utility.Events;

namespace OSPSuite.Presentation.UICommands
{
   public class ParameterIdentificationFeedbackViewVisibilityUICommand : ObjectUICommand<IParameterIdentificationFeedbackPresenter>, IListener<ParameterIdentificationSelectedEvent>
   {
      private readonly ISingleStartPresenterTask _singleStartPresenterTask;
      private ParameterIdentification _parameterIdentification;
      private readonly IParameterIdentificationFeedbackManager _parameterIdentificationFeedbackManager;

      public ParameterIdentificationFeedbackViewVisibilityUICommand(ISingleStartPresenterTask singleStartPresenterTask, IParameterIdentificationFeedbackManager parameterIdentificationFeedbackManager)
      {
         _singleStartPresenterTask = singleStartPresenterTask;
         _parameterIdentificationFeedbackManager = parameterIdentificationFeedbackManager;
      }

      public void Handle(ParameterIdentificationSelectedEvent eventToHandle)
      {
         _parameterIdentification = eventToHandle.ParameterIdentification;
      }

      protected override void PerformExecute()
      {
         _singleStartPresenterTask.StartForSubject(_parameterIdentificationFeedbackManager.GetFeedbackFor(_parameterIdentification));
      }
   }
}