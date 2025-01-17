using System.Collections.Generic;
using System.Data;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.SensitivityAnalyses;
using OSPSuite.Core.Events;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Mappers.SensitivityAnalyses;
using OSPSuite.Presentation.Presenters.SensitivityAnalyses;
using OSPSuite.Presentation.Views.SensitivityAnalyses;

namespace OSPSuite.Presentation.Presentation
{
   internal class concern_for_SensitivityAnalysisResultsPresenter : ContextSpecification<SensitivityAnalysisResultsPresenter>
   {
      protected IDialogCreator _dialogCreator;
      protected IExportDataTableToExcelTask _exportToExcelTask;
      protected ISensitivityAnalysisRunResultToDataTableMapper _dataTableMapper;
      protected ISensitivityAnalysisResultsView _view;

      protected override void Context()
      {
         _view = A.Fake<ISensitivityAnalysisResultsView>();
         _dialogCreator = A.Fake<IDialogCreator>();
         _exportToExcelTask = A.Fake<IExportDataTableToExcelTask>();
         _dataTableMapper = A.Fake<ISensitivityAnalysisRunResultToDataTableMapper>();
         sut = new SensitivityAnalysisResultsPresenter(_view, _dataTableMapper, _exportToExcelTask, _dialogCreator);
      }
   }

   internal class When_editing_a_sensitivity_analysis_and_there_arent_any_results : concern_for_SensitivityAnalysisResultsPresenter
   {
      private SensitivityAnalysis _sensitivityAnalysis;

      protected override void Context()
      {
         base.Context();
         _sensitivityAnalysis = new SensitivityAnalysis();
      }

      protected override void Because()
      {
         sut.EditSensitivityAnalysis(_sensitivityAnalysis);
      }

      [Observation]
      public void should_hide_the_error_view()
      {
         A.CallTo(() => _view.HideResultsView()).MustHaveHappened();
      }
   }

   internal class When_editing_a_sensitivity_analysis_and_there_are_errors : concern_for_SensitivityAnalysisResultsPresenter
   {
      private SensitivityAnalysis _sensitivityAnalysis;

      protected override void Context()
      {
         base.Context();
         _sensitivityAnalysis = new SensitivityAnalysis
         {
            Results = new SensitivityAnalysisRunResult
            {
               Errors = new[] { new IndividualRunInfo { ErrorMessage = "error message" } }
            }
         };
      }

      protected override void Because()
      {
         sut.EditSensitivityAnalysis(_sensitivityAnalysis);
      }

      [Observation]
      public void should_show_the_error_view()
      {
         A.CallTo(() => _view.ShowErrors(A<IReadOnlyList<string>>._)).MustHaveHappened();
      }
   }

   internal class When_exporting_to_excel_and_the_file_name_is_confirmed : concern_for_SensitivityAnalysisResultsPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.EditSensitivityAnalysis(new SensitivityAnalysis());
         A.CallTo(_dialogCreator).WithReturnType<string>().Returns("filename");
      }

      protected override void Because()
      {
         sut.ExportToExcel();
      }

      [Observation]
      public void the_export_task_is_used()
      {
         A.CallTo(() => _exportToExcelTask.ExportDataTableToExcel(A<DataTable>._, A<string>._, A<bool>._)).MustHaveHappened();
      }

      [Observation]
      public void the_datatable_mapper_is_used()
      {
         A.CallTo(() => _dataTableMapper.MapFrom(A<SensitivityAnalysis>._, A<SensitivityAnalysisRunResult>._)).MustHaveHappened();
      }
   }

   internal class When_exporting_to_excel_and_the_file_name_is_not_confirmed : concern_for_SensitivityAnalysisResultsPresenter
   {
      protected override void Context()
      {
         base.Context();
         sut.EditSensitivityAnalysis(new SensitivityAnalysis());
         A.CallTo(_dialogCreator).WithReturnType<string>().Returns(null);
      }

      protected override void Because()
      {
         sut.ExportToExcel();
      }
         
      [Observation]
      public void the_export_task_is_not_used()
      {
         A.CallTo(() => _exportToExcelTask.ExportDataTableToExcel(A<DataTable>._, A<string>._, A<bool>._)).MustNotHaveHappened();
      }

      [Observation]
      public void the_datatable_mapper_is_not_used()
      {
         A.CallTo(() => _dataTableMapper.MapFrom(A<SensitivityAnalysis>._, A<SensitivityAnalysisRunResult>._)).MustNotHaveHappened();
      }
   }

   internal class When_handling_a_sensitivity_analysis_results_updated_event_for_a_different_analysis : concern_for_SensitivityAnalysisResultsPresenter
   {
      private SensitivityAnalysis _sensitivityAnalysis;

      protected override void Context()
      {
         base.Context();
         _sensitivityAnalysis = new SensitivityAnalysis
         {
            Results = new SensitivityAnalysisRunResult()
         };
         _sensitivityAnalysis.Results.AddPKParameterSensitivity(new PKParameterSensitivity());
         sut.EditSensitivityAnalysis(_sensitivityAnalysis);
      }
      protected override void Because()
      {
         sut.Handle(new SensitivityAnalysisResultsUpdatedEvent(new SensitivityAnalysis()));
      }

      [Observation]
      public void should_not_show_the_results()
      {
         A.CallTo(() => _view.BindTo(A<DataTable>._)).MustHaveHappenedOnceExactly();
      }
   }

   internal class When_handling_a_sensitivity_analysis_results_updated_event : concern_for_SensitivityAnalysisResultsPresenter
   {
      private SensitivityAnalysis _sensitivityAnalysis;

      protected override void Context()
      {
         base.Context();
         _sensitivityAnalysis = new SensitivityAnalysis
         {
            Results = new SensitivityAnalysisRunResult()
         };
         _sensitivityAnalysis.Results.AddPKParameterSensitivity(new PKParameterSensitivity());
         sut.EditSensitivityAnalysis(_sensitivityAnalysis);
      }
      protected override void Because()
      {
         sut.Handle(new SensitivityAnalysisResultsUpdatedEvent(_sensitivityAnalysis));
      }
      [Observation]
      public void should_show_the_results()
      {
         A.CallTo(() => _view.BindTo(A<DataTable>._)).MustHaveHappenedTwiceExactly();
      }
   }
}