using System.Threading.Tasks;
using OSPSuite.Core.Reporting;

namespace OSPSuite.Core.Services
{
   public interface IReportingTask
   {
      void CreateReport(object objectToReport, ReportConfiguration reportConfiguration);
      Task CreateReportAsync(object objectToReport, ReportConfiguration reportConfiguration);
   }
}