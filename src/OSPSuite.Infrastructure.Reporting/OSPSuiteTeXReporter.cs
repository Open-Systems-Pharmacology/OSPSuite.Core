using System;
using System.Collections.Generic;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Infrastructure.Reporting
{
   public interface IOSPSuiteTeXReporter : ISpecification<Type>
   {
      IReadOnlyCollection<object> Report(object objectToReport, OSPSuiteTracker buildTracker);
   }

   public interface IOSPSuiteTeXReporter<T> : IOSPSuiteTeXReporter
   {
   }

   public abstract class OSPSuiteTeXReporter<T> : IOSPSuiteTeXReporter<T>
   {
      public virtual bool IsSatisfiedBy(Type type)
      {
         return type.IsAnImplementationOf<T>();
      }

      public IReadOnlyCollection<object> Report(object objectToReport, OSPSuiteTracker buildTracker)
      {
         return Report(objectToReport.DowncastTo<T>(), buildTracker);
      }

      public abstract IReadOnlyCollection<object> Report(T objectToReport, OSPSuiteTracker buildTracker);
   }
}