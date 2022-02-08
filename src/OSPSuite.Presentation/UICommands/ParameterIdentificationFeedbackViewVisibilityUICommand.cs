using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Services;

namespace OSPSuite.Presentation.UICommands
{
   public class ParameterIdentificationFeedbackViewVisibilityUICommand : ObjectUICommand<IParameterIdentificationFeedbackPresenter>
   {
      private readonly ISingleStartPresenterTask _singleStartPresenterTask;
      private readonly IParameterIdentificationFeedbackManager _parameterIdentificationFeedbackManager;
      private readonly IActiveSubjectRetriever _activeSubjectRetriever;

      public ParameterIdentificationFeedbackViewVisibilityUICommand(ISingleStartPresenterTask singleStartPresenterTask, IParameterIdentificationFeedbackManager parameterIdentificationFeedbackManager, IActiveSubjectRetriever activeSubjectRetriever)
      {
         _singleStartPresenterTask = singleStartPresenterTask;
         _parameterIdentificationFeedbackManager = parameterIdentificationFeedbackManager;
         _activeSubjectRetriever = activeSubjectRetriever;
      }

      protected override void PerformExecute()
      {
         _singleStartPresenterTask.StartForSubject(_parameterIdentificationFeedbackManager.GetFeedbackFor(_activeSubjectRetriever.Active<ParameterIdentification>()));
      }
   }
}