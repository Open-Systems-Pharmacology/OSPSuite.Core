using System;
using OSPSuite.Utility.Exceptions;

namespace OSPSuite.CLI.Core.MinimalImplementations
{
   public class ExceptionManager : ExceptionManagerBase
   {
      public override void LogException(Exception ex)
      {
         throw ex;
      }
   }
}