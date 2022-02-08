using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Services;

namespace OSPSuite.Presentation.UICommands
{
   public class ParameterIdentificationFeedbackViewVisibilityUICommand : ActiveObjectUICommand<IParameterIdentificationFeedbackPresenter>
   {
      private readonly ISingleStartPresenterTask _singleStartPresenterTask;
      private readonly IParameterIdentificationFeedbackManager _parameterIdentificationFeedbackManager;

      public ParameterIdentificationFeedbackViewVisibilityUICommand(ISingleStartPresenterTask singleStartPresenterTask, IParameterIdentificationFeedbackManager parameterIdentificationFeedbackManager, IActiveSubjectRetriever activeSubjectRetriever) : base(activeSubjectRetriever)
      {
         _singleStartPresenterTask = singleStartPresenterTask;
         _parameterIdentificationFeedbackManager = parameterIdentificationFeedbackManager;
      }

      protected override void PerformExecute()
      {
         _singleStartPresenterTask.StartForSubject(_parameterIdentificationFeedbackManager.GetFeedbackFor(Subject.ParameterIdentification));
      }
   }
}