using OSPSuite.Core.Commands.Core;

namespace OSPSuite.Core.Services
{
   public interface IHistoryExportTask
   {
      void CreateReport(IHistoryManager historyManager, ReportOptions reportOptions);
   }
}