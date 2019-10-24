using OSPSuite.Utility.Events;

namespace OSPSuite.Core.Services
{
   public class NoneProgressUpdater : IProgressUpdater
   {
      public void Dispose()
      {
         // nothing to do
      }

      public void Initialize(int numberOfIterations)
      {
         // nothing to do
      }

      public void Initialize(int numberOfIterations, string message)
      {
         // nothing to do
      }

      public void IncrementProgress()
      {
         // nothing to do
      }

      public void IncrementProgress(string message)
      {
         // nothing to do
      }

      public void ReportProgress(int iteration)
      {
         // nothing to do
      }

      public void ReportProgress(int iteration, string message)
      {
         // nothing to do
      }

      public void ReportStatusMessage(string message)
      {
         // nothing to do
      }

      public void Terminate()
      {
         // nothing to do
      }
   }
}