using OSPSuite.Utility;
using OSPSuite.Utility.Container;

namespace OSPSuite.Infrastructure.Reporting
{
   public interface IOSPSuiteTeXReporterRepository 
   {
      IOSPSuiteTeXReporter ReportFor(object objectToReport);
   }

   public class OSPSuiteTeXReporterRepository : BuilderRepository<IOSPSuiteTeXReporter>, IOSPSuiteTeXReporterRepository
   {
      public OSPSuiteTeXReporterRepository(IContainer container)
         : base(container, typeof (IOSPSuiteTeXReporter<>))
      {
      }

      public IOSPSuiteTeXReporter ReportFor(object objectToReport)
      {
         return BuilderFor(objectToReport);
      }
   }
}