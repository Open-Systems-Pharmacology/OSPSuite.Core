using OSPSuite.Utility.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace OSPSuite.Infrastructure.Import.Core
{
   public abstract class AbstractImporterExceptions : OSPSuiteException
   {
      public AbstractImporterExceptions(string message) : base(message) { }
   }
}
