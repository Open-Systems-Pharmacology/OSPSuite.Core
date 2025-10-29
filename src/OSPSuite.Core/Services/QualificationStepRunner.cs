using System;
using System.Threading.Tasks;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Services
{
   public interface IQualificationStepRunner : IDisposable
   {
      Task RunAsync(IQualificationStep qualificationStep);
   }

   public abstract class QualificationStepRunner<T> : IQualificationStepRunner where T : IQualificationStep
   {
      protected readonly IOSPSuiteLogger _logger;

      protected QualificationStepRunner(IOSPSuiteLogger logger)
      {
         _logger = logger;
      }

      protected virtual void Cleanup()
      {
      }

      public async Task RunAsync(IQualificationStep qualificationStep)
      {
         _logger.AddDebug(Captions.StartingQualificationStep(qualificationStep.Display));
         await RunAsync(qualificationStep.DowncastTo<T>());
      }

      public abstract Task RunAsync(T qualificationStep);

      #region Disposable properties

      private bool _disposed;

      public void Dispose()
      {
         if (_disposed) return;

         Cleanup();
         GC.SuppressFinalize(this);
         _disposed = true;
      }

      ~QualificationStepRunner() => Cleanup();

      #endregion
   }
}