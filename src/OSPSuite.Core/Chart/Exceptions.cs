using System;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Chart
{
   public class MissingDataException : OSPSuiteException
   {
      public MissingDataException(string msg) : base(msg) {}

      public MissingDataException(string axis, string curveName) : base(string.Format("{0}Data missing for curve {1}", axis, curveName))
      {
      }
   }
}