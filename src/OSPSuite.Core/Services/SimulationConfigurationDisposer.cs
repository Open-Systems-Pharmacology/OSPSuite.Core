using System;
using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Services
{
   public class SimulationConfigurationDisposer : IDisposable
   {
      private readonly SimulationConfiguration _simulationConfiguration;

      public SimulationConfigurationDisposer(SimulationConfiguration simulationConfiguration)
      {
         _simulationConfiguration = simulationConfiguration;
         _simulationConfiguration.Freeze();
      }

      protected virtual void Cleanup()
      {
         _simulationConfiguration.ClearCache();
      }

      #region Disposable properties

      private bool _disposed;

      public void Dispose()
      {
         if (_disposed) return;

         Cleanup();
         GC.SuppressFinalize(this);
         _disposed = true;
      }

      ~SimulationConfigurationDisposer()
      {
         Cleanup();
      }

      #endregion
   }
}