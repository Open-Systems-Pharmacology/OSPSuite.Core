using System;
using OSPSuite.Utility.Events;

namespace OSPSuite.R.MinimalImplementations
{
   public class ProgressUpdater : Utility.Events.ProgressUpdater
   {
      public ProgressUpdater(IEventPublisher eventPublisher) : base(eventPublisher)
      {
      }

      public override void ReportProgress(int iteration, int numberOfIterations, string message)
      {
         Console.Write(value: $"..{PercentFrom(iteration, numberOfIterations)}");
      }

      public override void ReportStatusMessage(string message)
      {
         Console.Write(message);
      }
   }
}