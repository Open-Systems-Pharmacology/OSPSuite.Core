using OSPSuite.BDDHelper;
using FakeItEasy;
using OSPSuite.Core.Domain.SensitivityAnalyses;
using OSPSuite.Core.Events;
using OSPSuite.Presentation.Presenters.SensitivityAnalyses;
using OSPSuite.Presentation.Views.SensitivityAnalyses;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_SensitivityAnalysisFeedbackPresenter : ContextSpecification<SensitivityAnalysisFeedbackPresenter>
   {
      protected IPresentationUserSettings _presentationUserSettings;
      protected ISensitivityAnalysisFeedbackView _view;
      protected SensitivityAnalysis _sensitivityAnalysis;

      protected override void Context()
      {
         _presentationUserSettings = A.Fake<IPresentationUserSettings>();
         _view = A.Fake<ISensitivityAnalysisFeedbackView>();
         _sensitivityAnalysis = new SensitivityAnalysis();

         sut = new SensitivityAnalysisFeedbackPresenter(_view, _presentationUserSettings);
      }
   }

   public class When_updating_the_progress_for_a_new_count_of_calculated_simulations : concern_for_SensitivityAnalysisFeedbackPresenter
   {
      protected override void Because()
      {
         sut.Handle(new SensitivityAnalysisProgressEvent(_sensitivityAnalysis, 100, 200));
      }

      [Observation]
      public void should_update_the_view_with_the_new_progress()
      {
         A.CallTo(() => _view.UpdateProgress(100, 200)).MustHaveHappened();
      }
   }

   public class When_updating_the_progress_for_start_of_sensitivity_analysis : concern_for_SensitivityAnalysisFeedbackPresenter
   {
      protected override void Because()
      {
         sut.Handle(new SensitivityAnalysisStartedEvent(_sensitivityAnalysis));
      }

      [Observation]
      public void should_reset_the_progress_and_show_the_progress_in_the_view()
      {
         A.CallTo(() => _view.ResetFeedback()).MustHaveHappened();
         A.CallTo(() => _view.ShowFeedback()).MustHaveHappened();
      }
   }

}
