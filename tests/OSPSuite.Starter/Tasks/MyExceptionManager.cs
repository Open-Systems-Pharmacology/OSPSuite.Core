using System;
using System.Diagnostics;
using OSPSuite.Utility.Exceptions;

namespace OSPSuite.Starter.Tasks
{
   internal class MyExceptionManager : ExceptionManagerBase
   {
      public override void LogException(Exception ex)
      {
         Debug.Print(ex.Message);
      }
   }
}