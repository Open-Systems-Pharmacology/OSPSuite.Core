using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Events;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Utility.Events;

namespace OSPSuite.Presentation.UICommands
{
   public class ParameterIdentificationFeedbackViewVisibilityUICommand : ObjectUICommand<IParameterIdentificationFeedbackPresenter>, IListener<ParameterIdentificationSelectedEvent>
   {
      private readonly IMultipleParameterIdentificationFeedbackPresentersManager _multipleParameterIdentificationFeedbackPresentersManager;
      private ParameterIdentification _parameterIdentification;

      public ParameterIdentificationFeedbackViewVisibilityUICommand(IMultipleParameterIdentificationFeedbackPresentersManager multipleParameterIdentificationFeedbackPresentersManager)
      {
         _multipleParameterIdentificationFeedbackPresentersManager = multipleParameterIdentificationFeedbackPresentersManager;
      }

      public void Handle(ParameterIdentificationSelectedEvent eventToHandle)
      {
         _parameterIdentification = eventToHandle.ParameterIdentification;
      }

      protected override void PerformExecute()
      {
         _multipleParameterIdentificationFeedbackPresentersManager.FeedbackPresenterFor(_parameterIdentification).Display();
      }
   }
}