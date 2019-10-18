using System;
using OSPSuite.Utility.Events;

namespace OSPSuite.R.MinimalImplementations
{
   public class ProgressUpdater : IProgressUpdater
   {
      private int _numberOfIterations;
      private int _currentIteration;

      public void Dispose()
      {
      }

      public void Initialize(int numberOfIterations)
      {
         this.Initialize(numberOfIterations, string.Empty);
      }

      public void Initialize(int numberOfIterations, string message)
      {
         _numberOfIterations = numberOfIterations;
      }

      public virtual void IncrementProgress()
      {
         this.IncrementProgress(string.Empty);
      }

      public virtual void IncrementProgress(string message)
      {
         this.ReportProgress(this._currentIteration + 1, message);
      }

      public virtual void ReportProgress(int iteration)
      {
         this.ReportProgress(iteration, string.Empty);
      }

      public virtual void ReportProgress(int iteration, string message)
      {
         _currentIteration = iteration;
         Console.Write(this.percentFrom(iteration));
      }

      public virtual void ReportStatusMessage(string message)
      {
         Console.Write(message);
      }

      private int percentFrom(int iteration)
      {
         return (int)Math.Floor((double)iteration * 100.0 / (double)this._numberOfIterations);
      }

      public void Terminate()
      {
      }
   }
}