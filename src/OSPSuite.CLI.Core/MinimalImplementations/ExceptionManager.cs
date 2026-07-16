using System;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.CLI.Core.MinimalImplementations
{
   public class ExceptionManager : ExceptionManagerBase
   {
      public override void LogException(Exception ex)
      {
         Console.WriteLine(ex.FullMessage());
      }
   }
}