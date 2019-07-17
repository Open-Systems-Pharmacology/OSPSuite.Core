using OSPSuite.TeXReporting;
using OSPSuite.TeXReporting.Builder;

namespace OSPSuite.Infrastructure.Reporting
{
   public class OSPSuiteBuildSettings : BuildSettings
   {
   }

   public class OSPSuiteTracker : BuildTracker
   {
      public OSPSuiteBuildSettings Settings { get; set; }
   }
}