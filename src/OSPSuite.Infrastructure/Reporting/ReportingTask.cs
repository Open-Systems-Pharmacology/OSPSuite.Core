using System.Threading.Tasks;
using OSPSuite.Assets;
using OSPSuite.Core;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Reporting;
using OSPSuite.Core.Services;
using OSPSuite.TeXReporting;
using OSPSuite.Utility;
using OSPSuite.Utility.Exceptions;

namespace OSPSuite.Infrastructure.Reporting
{
   public class ReportingTask : IReportingTask
   {
      private readonly IReportCreator _reportCreator;
      private readonly IOSPSuiteTeXReporterRepository _reporterRepository;
      private readonly IBuildTrackerFactory _buildTrackerFactory;
      private readonly IApplicationConfiguration _applicationConfiguration;

      public ReportingTask(IReportCreator reportCreator, IOSPSuiteTeXReporterRepository reporterRepository, IBuildTrackerFactory buildTrackerFactory, IApplicationConfiguration applicationConfiguration)
      {
         _reportCreator = reportCreator;
         _reporterRepository = reporterRepository;
         _buildTrackerFactory = buildTrackerFactory;
         _applicationConfiguration = applicationConfiguration;
      }

      public void CreateReport(object objectToReport, ReportConfiguration reportConfiguration)
      {
         CreateReportAsync(objectToReport, reportConfiguration)
            .SecureContinueWith(t =>
            {
               if (reportConfiguration.OpenReportAfterCreation)
                  FileHelper.TryOpenFile(reportConfiguration.ReportFile);
            });
      }

      public async Task CreateReportAsync(object objectToReport, ReportConfiguration reportConfiguration)
      {
         if (objectToReport == null)
            return;

         var reporter = _reporterRepository.ReportFor(objectToReport);
         if (reporter == null)
            throw new OSPSuiteException(Error.CouldNotFindAReporterFor(objectToReport.GetType()));

         var tracker = _buildTrackerFactory.CreateFor<OSPSuiteTracker>(reportConfiguration.ReportFile);
         tracker.Settings = buildSettingsFrom(reportConfiguration);

         await _reportCreator.ReportToPDF(tracker, reportSettingsFrom(reportConfiguration), reporter.Report(objectToReport, tracker));
      }

      private ReportSettings reportSettingsFrom(ReportConfiguration reportConfiguration) =>
         new ReportSettings
         {
            Author = reportConfiguration.Author,
            ContentFileName = "Content",
            Software = _applicationConfiguration.ProductNameWithTrademark,
            SoftwareVersion = _applicationConfiguration.FullVersion,
            SubTitle = reportConfiguration.SubTitle,
            TemplateFolder = reportConfiguration.Template.Path,
            Title = reportConfiguration.Title,
            DeleteWorkingDir = reportConfiguration.DeleteWorkingDir,
            SaveArtifacts = reportConfiguration.SaveArtifacts,
            ColorStyle = EnumHelper.ParseValue<ReportSettings.ReportColorStyles>(reportConfiguration.ColorStyle.ToString()),
            Draft = reportConfiguration.Draft,
            Font =  EnumHelper.ParseValue<ReportSettings.ReportFont>(reportConfiguration.Font.ToString()),
            NumberOfCompilations = reportConfiguration.NumberOfCompilations
         };

      private OSPSuiteBuildSettings buildSettingsFrom(ReportConfiguration reportConfiguration) =>
         new OSPSuiteBuildSettings
         {
            Verbose = reportConfiguration.Verbose,
         };
   }
}