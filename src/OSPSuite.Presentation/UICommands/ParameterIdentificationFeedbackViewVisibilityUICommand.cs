using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Events;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Utility.Events;

namespace OSPSuite.Presentation.UICommands
{
   public class ParameterIdentificationFeedbackViewVisibilityUICommand : ObjectUICommand<IParameterIdentificationFeedbackPresenter>, IListener<ParameterIdentificationSelectedEvent>
   {
      private readonly IParameterIdentificationFeedbackPresentersManager _parameterIdentificationFeedbackPresentersManager;
      private ParameterIdentification _parameterIdentification;

      public ParameterIdentificationFeedbackViewVisibilityUICommand(IParameterIdentificationFeedbackPresentersManager multipleParameterIdentificationFeedbackPresentersManager)
      {
         _parameterIdentificationFeedbackPresentersManager = multipleParameterIdentificationFeedbackPresentersManager;
      }

      public void Handle(ParameterIdentificationSelectedEvent eventToHandle)
      {
         _parameterIdentification = eventToHandle.ParameterIdentification;
      }

      protected override void PerformExecute()
      {
         _parameterIdentificationFeedbackPresentersManager.FeedbackPresenterFor(_parameterIdentification).Display();
      }
   }
}