using System;
using OSPSuite.Utility.Exceptions;

namespace OSPSuite.Core.Domain.Services.ParameterIdentifications
{
   public class MatrixCalculationException : OSPSuiteException
   {
      public MatrixCalculationException(string message) : base(message)
      {
      }

      public MatrixCalculationException(string message, Exception innerException) : base(message, innerException)
      {
      }
   }
}