using System.Data;
using OSPSuite.Assets;
using OSPSuite.Utility.Events;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.SensitivityAnalyses;
using OSPSuite.Core.Events;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Mappers.SensitivityAnalyses;
using OSPSuite.Presentation.Views.SensitivityAnalyses;

namespace OSPSuite.Presentation.Presenters.SensitivityAnalyses
{
   public interface ISensitivityAnalysisResultsPresenter : ISensitivityAnalysisItemPresenter,
      IListener<SensitivityAnalysisResultsUpdatedEvent>
   {
      void ExportToExcel();
   }

   public class SensitivityAnalysisResultsPresenter : AbstractSubPresenter<ISensitivityAnalysisResultsView, ISensitivityAnalysisResultsPresenter>, ISensitivityAnalysisResultsPresenter
   {
      private readonly ISensitivityAnalysisRunResultToDataTableMapper _dataTableMapper;
      private readonly IExportDataTableToExcelTask _exportToExcelTask;
      private readonly IDialogCreator _dialogCreator;
      private SensitivityAnalysis _sensitivityAnalysis;

      public SensitivityAnalysisResultsPresenter(ISensitivityAnalysisResultsView view, ISensitivityAnalysisRunResultToDataTableMapper dataTableMapper,
         IExportDataTableToExcelTask exportToExcelTask, IDialogCreator dialogCreator) : base(view)
      {
         _dataTableMapper = dataTableMapper;
         _exportToExcelTask = exportToExcelTask;
         _dialogCreator = dialogCreator;
      }

      public void EditSensitivityAnalysis(SensitivityAnalysis sensitivityAnalysis)
      {
         _sensitivityAnalysis = sensitivityAnalysis;
         showSensitivityAnalysisResults();
      }

      private void showSensitivityAnalysisResults()
      {
         if (!_sensitivityAnalysis.HasResults)
         {
            showNoResultsAvailable();
            return;
         }

         _view.BindTo(mapResultsToTable());
      }

      private DataTable mapResultsToTable()
      {
         return _dataTableMapper.MapFrom(_sensitivityAnalysis, _sensitivityAnalysis.Results);
      }

      private void showNoResultsAvailable()
      {
         _view.HideResultsView();
      }

      public void Handle(SensitivityAnalysisResultsUpdatedEvent eventToHandle)
      {
         if (!canHandle(eventToHandle))
            return;

         showSensitivityAnalysisResults();
      }

      public void ExportToExcel()
      {
         var fileName = _dialogCreator.AskForFileToSave(Captions.SensitivityAnalysis.ExportPKAnalysesSentitivityTToExcelTitle, Constants.Filter.EXCEL_SAVE_FILE_FILTER, Constants.DirectoryKey.REPORT, _sensitivityAnalysis.Name);
         if (string.IsNullOrEmpty(fileName)) return;

         _exportToExcelTask.ExportDataTableToExcel(mapResultsToTable(), fileName, openExcel: true);
      }

      private bool canHandle(SensitivityAnalysisEvent eventToHandle)
      {
         return Equals(eventToHandle.SensitivityAnalysis, _sensitivityAnalysis);
      }
   }
}