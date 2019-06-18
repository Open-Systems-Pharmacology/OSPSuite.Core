using System;
using OSPSuite.Utility.Exceptions;

namespace OSPSuite.Core.Qualification
{
   public class QualificationRunException : OSPSuiteException
   {
      public QualificationRunException()
      {
      }

      public QualificationRunException(string message) : base(message)
      {
      }

      public QualificationRunException(string message, Exception innerException) : base(message, innerException)
      {
      }
   }
}