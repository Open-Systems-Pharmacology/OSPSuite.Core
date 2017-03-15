using System;
using OSPSuite.Presentation.Core;

namespace OSPSuite.Presentation.Presenters
{
   public class BatchUpdate : IDisposable
   {
      private IBatchUpdatable _batchUpdatable;
      private readonly bool _batchStartedHere;

      public BatchUpdate(IBatchUpdatable batchUpdatable)
      {
         _batchUpdatable = batchUpdatable;
         _batchStartedHere = !batchUpdatable.Updating;
         _batchUpdatable.BeginUpdate();
      }

      protected virtual void Cleanup()
      {
         if (_batchStartedHere)
            _batchUpdatable.EndUpdate();

         _batchUpdatable = null;
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

      ~BatchUpdate()
      {
         Cleanup();
      }

      #endregion
   }
}