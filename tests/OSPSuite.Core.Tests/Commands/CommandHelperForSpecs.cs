﻿using OSPSuite.Core.Commands.Core;

namespace OSPSuite.Core.Commands
{
   public static class CommandHelperForSpecs
   {
      public static ICommand<IOSPSuiteExecutionContext> ExecuteAndInvokeInverse(this IReversibleCommand<IOSPSuiteExecutionContext> command, IOSPSuiteExecutionContext context)
      {
         command.Execute(context);
         return command.InvokeInverse(context);
      }

      public static ICommand<IOSPSuiteExecutionContext> InvokeInverse(this IReversibleCommand<IOSPSuiteExecutionContext> command, IOSPSuiteExecutionContext context)
      {
         command.RestoreExecutionData(context);
         var inverse = command.InverseCommand(context);
         inverse.Execute(context);
         return inverse;
      }
   }
}