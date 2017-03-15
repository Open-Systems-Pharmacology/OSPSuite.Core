using OSPSuite.Core.Commands.Core;

namespace OSPSuite.Core.Services
{
   public interface IReportTask
   {
      void CreateReport(IHistoryManager historyManager, ReportOptions reportOptions);
   }
}