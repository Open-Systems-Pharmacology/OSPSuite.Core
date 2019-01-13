using System.Collections.Generic;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Presentation.DTO.ParameterIdentifications;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Views.ParameterIdentifications;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_MultipleParameterIdentificationFeedbackPresenter : ContextSpecification<IMultipleParameterIdentificationFeedbackPresenter>
   {
      protected IMultipleParameterIdentificationFeedbackView _view;
      protected IEnumerable<MultiOptimizationRunResultDTO> _allMultiOptimizationRunResultDTO;

      protected override void Context()
      {
         _view = A.Fake<IMultipleParameterIdentificationFeedbackView>();
         sut = new MultipleParameterIdentificationFeedbackPresenter(_view);

         A.CallTo(() => _view.BindTo(A<IEnumerable<MultiOptimizationRunResultDTO>>._))
            .Invokes(x => _allMultiOptimizationRunResultDTO = x.GetArgument<IEnumerable<MultiOptimizationRunResultDTO>>(0));
      }
   }

   public class When_the_multi_parameter_identification_feedback_presenter_is_updating_the_feedback : concern_for_MultipleParameterIdentificationFeedbackPresenter
   {
      private ParameterIdentificationRunState _runStatus1;
      private ParameterIdentificationRunState _runStatus2;

      protected override void Context()
      {
         base.Context();
         _runStatus1 = A.Fake<ParameterIdentificationRunState>();
         A.CallTo(() => _runStatus1.RunResult).Returns(new ParameterIdentificationRunResult {Index = 1});
         A.CallTo(() => _runStatus1.BestResult.TotalError).Returns(1);
         A.CallTo(() => _runStatus1.CurrentResult.TotalError).Returns(2);
         A.CallTo(() => _runStatus1.Status).Returns(RunStatus.Running);

         _runStatus2 = A.Fake<ParameterIdentificationRunState>();
         A.CallTo(() => _runStatus2.RunResult).Returns(new ParameterIdentificationRunResult {Index = 2});
         A.CallTo(() => _runStatus2.BestResult.TotalError).Returns(3);
         A.CallTo(() => _runStatus2.CurrentResult.TotalError).Returns(4);
         A.CallTo(() => _runStatus2.Status).Returns(RunStatus.Canceled);
      }

      protected override void Because()
      {
         sut.UpdateFeedback(_runStatus1);
         sut.UpdateFeedback(_runStatus2);
         sut.UpdateFeedback(_runStatus1);
      }

      [Observation]
      public void should_add_any_new_parameter_identification_run_that_is_not_monitored_yet_to_the_view()
      {
         A.CallTo(() => _view.BindTo(A<IEnumerable<MultiOptimizationRunResultDTO>>._)).MustHaveHappenedTwiceExactly();
      }

      [Observation]
      public void should_update_the_view_when_the_newly_updatedr_run_was_already_added()
      {
         A.CallTo(() => _view.RefreshData()).MustHaveHappenedOnceExactly();
      }

      [Observation]
      public void should_update_the_visual_feedback_with_the_latest_properties_of_the_optimization_run()
      {
         var dtoStatus1 = _allMultiOptimizationRunResultDTO.ElementAt(0);
         validatePropertiesUpdate(dtoStatus1, _runStatus1);

         var dtoStatus2 = _allMultiOptimizationRunResultDTO.ElementAt(1);
         validatePropertiesUpdate(dtoStatus2, _runStatus2);
      }

      private void validatePropertiesUpdate(MultiOptimizationRunResultDTO dto, ParameterIdentificationRunState runState)
      {
         dto.BestError.ShouldBeEqualTo(runState.BestResult.TotalError);
         dto.CurrentError.ShouldBeEqualTo(runState.CurrentResult.TotalError);
         dto.Status.ShouldBeEqualTo(runState.Status);
      }
   }

   public class When_the_multi_parameter_identification_feedback_presenter_is_resetting_the_feedback : concern_for_MultipleParameterIdentificationFeedbackPresenter
   {
      protected override void Context()
      {
         base.Context();
         var runStatus = A.Fake<ParameterIdentificationRunState>();
         sut.UpdateFeedback(runStatus);
         _allMultiOptimizationRunResultDTO.Any().ShouldBeTrue();
      }

      protected override void Because()
      {
         sut.ResetFeedback();
      }

      [Observation]
      public void should_clear_the_binding()
      {
         _allMultiOptimizationRunResultDTO.Any().ShouldBeTrue();
         A.CallTo(() => _view.DeleteBinding()).MustHaveHappened();
      }
   }
}