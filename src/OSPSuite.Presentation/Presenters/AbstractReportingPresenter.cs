using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Reporting;
using OSPSuite.Core.Services;
using OSPSuite.Presentation.Views;
using OSPSuite.Utility;

namespace OSPSuite.Presentation.Presenters
{
   public interface IReportingPresenter : IPresenter<IReportingView>, IDisposablePresenter
   {
      /// <summary>
      ///    Returns all available templates
      /// </summary>
      IEnumerable<ReportTemplate> AllTemplates();

      /// <summary>
      ///    Start the report use case to report the <paramref name="objectToReport" />
      /// </summary>
      void CreateReport(object objectToReport, bool isDraft = false);

      /// <summary>
      ///    Returns all available fonts
      /// </summary>
      IEnumerable<ReportFont> AvailableFonts();
   }

   public abstract class AbstractReportingPresenter : AbstractDisposablePresenter<IReportingView, IReportingPresenter>, IReportingPresenter
   {
      private readonly IReportTemplateRepository _reportTemplateRepository;
      private readonly IDialogCreator _dialogCreator;
      private readonly IReportingTask _reportingTask;
      private readonly IStartOptions _startOptions;
      private ReportConfiguration _reportConfiguration;
      private object _objectToReport;

      protected AbstractReportingPresenter(IReportingView view, IReportTemplateRepository reportTemplateRepository, IDialogCreator dialogCreator,
         IReportingTask reportingTask, IStartOptions startOptions)
         : base(view)
      {
         _reportTemplateRepository = reportTemplateRepository;
         _dialogCreator = dialogCreator;
         _reportingTask = reportingTask;
         _startOptions = startOptions;
      }

      public void CreateReport(object objectToReport, bool isDraft = false)
      {
         _objectToReport = objectToReport;
         _reportConfiguration = new ReportConfiguration
         {
            Template = _reportTemplateRepository.All().FirstOrDefault(),
            Title = titleFor(objectToReport),
            SubTitle = subTitleFor(objectToReport),
            Draft = isDraft
         };

         _view.BindTo(_reportConfiguration);
         _view.IsDeveloperMode = _startOptions.IsDeveloperMode;
         _view.Display();

         if (_view.Canceled) return;
         startCreationProcess();
      }

      public IEnumerable<ReportFont> AvailableFonts() => EnumHelper.AllValuesFor<ReportFont>();

      private void startCreationProcess()
      {
         string reportFile = _dialogCreator.AskForFileToSave(Captions.Reporting.SelectFile, Constants.Filter.PDF_FILE_FILTER, Constants.DirectoryKey.REPORT, defaultFileName: _reportConfiguration.SubTitle);
         if (string.IsNullOrEmpty(reportFile))
            return;

         _reportConfiguration.ReportFile = reportFile;
         _reportingTask.CreateReport(_objectToReport, _reportConfiguration);
      }

      private string subTitleFor(object objectToReport)
      {
         var withName = objectToReport as IWithName;
         if (withName != null)
            return string.IsNullOrEmpty(withName.Name) ? Captions.Reporting.DefaultTitle : withName.Name;

         return Captions.Reporting.DefaultTitle;
      }

      private string titleFor(object objectToReport)
      {
         var title = RetrieveObjectTypeFrom(objectToReport);
         return (String.IsNullOrEmpty(title) ? Captions.Reporting.DefaultTitle : title);
      }

      protected virtual string RetrieveObjectTypeFrom(object objectToReport)
      {
         return Captions.Reporting.DefaultTitle;
      }

      public IEnumerable<ReportTemplate> AllTemplates()
      {
         return _reportTemplateRepository.All();
      }
   }
}