using System.Collections.Generic;
using OSPSuite.Core.Domain;
using OSPSuite.TeXReporting.Items;

namespace OSPSuite.Infrastructure.Reporting
{
   public static class TeXBuilderExtensions
   {
      public static IReadOnlyCollection<object> ReportDescription<T>(this OSPSuiteTeXBuilder<T> builder, IObjectBase objectBase, OSPSuiteTracker tracker)
      {
         return reportedDescriptionFor<T>(objectBase, tracker);
      }

      public static IReadOnlyCollection<object> ReportDescription<T>(this IOSPSuiteTeXReporter<T> reporter, IObjectBase objectBase, OSPSuiteTracker tracker)
      {
         return reportedDescriptionFor<T>(objectBase, tracker);
      }

      private static IReadOnlyCollection<object> reportedDescriptionFor<T>(IObjectBase objectBase, OSPSuiteTracker tracker)
      {
         if (!tracker.Settings.Verbose || string.IsNullOrEmpty(objectBase.Description))
            return new List<object>();
         
         return new List<object> {objectBase.Description, new LineBreak()};
      }
   }
}