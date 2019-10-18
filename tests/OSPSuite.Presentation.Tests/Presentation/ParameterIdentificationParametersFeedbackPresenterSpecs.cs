using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using OSPSuite.Assets;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Services.ParameterIdentifications;
using OSPSuite.Helpers;
using OSPSuite.Presentation.DTO.ParameterIdentifications;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Presentation.Views.ParameterIdentifications;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_ParameterIdentificationParametersFeedbackPresenter : ContextSpecification<IParameterIdentificationParametersFeedbackPresenter>
   {
      protected IParameterIdentificationParametersFeedbackView _view;
      private ParameterIdentification _paramterIdentification;
      private IdentificationParameter _identificationParameter1;
      private IdentificationParameter _identificationParameter2;
      protected List<ParameterFeedbackDTO> _allParameterFeedbackDTO;
      protected List<IRunPropertyDTO> _allPropertiesDTO;
      protected IParameterIdentificationExportTask _exportTask;
      private IdentificationParameter _identificationParameter3;

      protected override void Context()
      {
         _view = A.Fake<IParameterIdentificationParametersFeedbackView>();
         _exportTask = A.Fake<IParameterIdentificationExportTask>();
         sut = new ParameterIdentificationParametersFeedbackPresenter(_view, _exportTask);

         _paramterIdentification = new ParameterIdentification();
         _identificationParameter1 = createIdentificationParameter("P1", 10, 0, 20);
         _identificationParameter2 = createIdentificationParameter("P2", 20, 5, 40);
         _identificationParameter3 = createIdentificationParameter("P3", 20, 5, 40);
         _identificationParameter3.IsFixed = true;

         _paramterIdentification.AddIdentificationParameter(_identificationParameter1);
         _paramterIdentification.AddIdentificationParameter(_identificationParameter2);
         _paramterIdentification.AddIdentificationParameter(_identificationParameter3);

         A.CallTo(() => _view.BindTo(A<IEnumerable<ParameterFeedbackDTO>>._, A<IEnumerable<IRunPropertyDTO>>._))
            .Invokes(x =>
            {
               _allParameterFeedbackDTO = x.GetArgument<IEnumerable<ParameterFeedbackDTO>>(0).ToList();
               _allPropertiesDTO = x.GetArgument<IEnumerable<IRunPropertyDTO>>(1).ToList();
            });

         sut.EditParameterIdentification(_paramterIdentification);
      }

      private IdentificationParameter createIdentificationParameter(string name, double value, double min, double max)
      {
         return DomainHelperForSpecs.IdentificationParameter(name, min, max, value);
      }
   }

   public class When_the_parameter_identification_parameters_feedback_presenter_is_editing_a_parameter_identification : concern_for_ParameterIdentificationParametersFeedbackPresenter
   {
      [Observation]
      public void should_create_one_parameter_entry_for_the_total_error()
      {
         _allParameterFeedbackDTO.ExistsByName(Captions.ParameterIdentification.TotalError).ShouldBeTrue();
      }

      [Observation]
      public void should_create_one_parameter_entry_for_each_variable_identified_parameter()
      {
         _allParameterFeedbackDTO.ExistsByName("P1").ShouldBeTrue();
         _allParameterFeedbackDTO.ExistsByName("P2").ShouldBeTrue();
      }

      [Observation]
      public void should_create_a_parameter_entry_for_fixed_identification_parameter()
      {
         _allParameterFeedbackDTO.ExistsByName("P3").ShouldBeTrue();
      }

      [Observation]
      public void should_have_set_the_expected_values_based_on_the_start_value_of_the_identified_parameter()
      {
         _allParameterFeedbackDTO.FindByName(Captions.ParameterIdentification.TotalError).Best.Value.ShouldBeEqualTo(double.NaN);
         _allParameterFeedbackDTO.FindByName(Captions.ParameterIdentification.TotalError).Current.Value.ShouldBeEqualTo(double.NaN);

         _allParameterFeedbackDTO.FindByName("P1").Best.Value.ShouldBeEqualTo(10);
         _allParameterFeedbackDTO.FindByName("P1").Current.Value.ShouldBeEqualTo(10);

         _allParameterFeedbackDTO.FindByName("P2").Best.Value.ShouldBeEqualTo(20);
         _allParameterFeedbackDTO.FindByName("P2").Current.Value.ShouldBeEqualTo(20);
      }
   }

   public class When_the_parameter_identification_parameters_feedback_presenter_is_updating_the_feedback : concern_for_ParameterIdentificationParametersFeedbackPresenter
   {
      private ParameterIdentificationRunState _runState;
      private OptimizationRunResult _currentResult;
      private OptimizationRunResult _bestResult;

      protected override void Context()
      {
         base.Context();
         _bestResult = new OptimizationRunResult();
         _bestResult.AddValue(new OptimizedParameterValue("P1", 1, 1));
         _bestResult.AddValue(new OptimizedParameterValue("P2", 2, 2));
         _currentResult = new OptimizationRunResult();
         _currentResult.AddValue(new OptimizedParameterValue("P1", 3, 4));
         _currentResult.AddValue(new OptimizedParameterValue("P2", 4, 4));

         _runState = A.Fake<ParameterIdentificationRunState>();
         A.CallTo(() => _runState.Status).Returns(RunStatus.Running);
         A.CallTo(() => _runState.BestResult).Returns(_bestResult);
         A.CallTo(() => _runState.CurrentResult).Returns(_currentResult);
      }

      protected override void Because()
      {
         sut.UpdateFeedback(_runState);
      }

      [Observation]
      public void should_update_the_value_in_the_underlying_parameter_feedback_dto()
      {
         _allParameterFeedbackDTO.FindByName("P1").Best.Value.ShouldBeEqualTo(1);
         _allParameterFeedbackDTO.FindByName("P1").Current.Value.ShouldBeEqualTo(3);

         _allParameterFeedbackDTO.FindByName("P2").Best.Value.ShouldBeEqualTo(2);
         _allParameterFeedbackDTO.FindByName("P2").Current.Value.ShouldBeEqualTo(4);
      }

      [Observation]
      public void should_refresh_the_view()
      {
         A.CallTo(() => _view.RefreshData()).MustHaveHappened();
      }

      [Observation]
      public void should_have_enabled_the_export_to_excel()
      {
         _view.CanExportParametersHistory.ShouldBeTrue();
      }
   }

   public class When_the_parameter_feedback_presenter_is_exporting_the_parameters_history_to_excel_but_no_history_is_available : concern_for_ParameterIdentificationParametersFeedbackPresenter
   {
      protected override void Because()
      {
         sut.ExportParametersHistory();
      }

      [Observation]
      public void should_not_export_anything()
      {
         A.CallTo(() => _exportTask.ExportParametersHistoryToExcel(A<IReadOnlyList<IdentificationParameterHistory>>._, A<IReadOnlyList<float>>._)).MustNotHaveHappened();
      }
   }

   public class When_the_parameter_feedback_presenter_is_exporting_the_parameters_history_to_excel_with_history_available : concern_for_ParameterIdentificationParametersFeedbackPresenter
   {
      private ParameterIdentificationRunState _runState;

      protected override void Context()
      {
         base.Context();

         _runState = A.Fake<ParameterIdentificationRunState>();
         A.CallTo(() => _runState.Status).Returns(RunStatus.Created);
         sut.UpdateFeedback(_runState);
      }

      protected override void Because()
      {
         sut.ExportParametersHistory();
      }
      
      [Observation]
      public void should_export_the_history_to_excel()
      {
         A.CallTo(() => _exportTask.ExportParametersHistoryToExcel(_runState.ParametersHistory, _runState.ErrorHistory)).MustHaveHappened();
      }
   }

   public class When_the_feedback_is_being_reseted_in_the_parameter_feedback_presenter : concern_for_ParameterIdentificationParametersFeedbackPresenter
   {
      protected override void Context()
      {
         base.Context();
         _view.CanExportParametersHistory = true;
      }

      protected override void Because()
      {
         sut.ResetFeedback();
      }

      [Observation]
      public void should_disable_the_export_to_excel()
      {
         _view.CanExportParametersHistory.ShouldBeFalse();
      }
   }
}