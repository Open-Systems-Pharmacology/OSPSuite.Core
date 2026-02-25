using System;
using System.IO;
using LumenWorks.Framework.IO.Csv;

namespace OSPSuite.Infrastructure.Import.Services
{
   public abstract class CsvReaderBase : IDisposable
   {
      public CsvReader Csv { get; protected set; }

      protected abstract void Cleanup();

      #region Disposable properties

      private bool _disposed;

      public void Dispose()
      {
         if (_disposed) return;

         Cleanup();
         GC.SuppressFinalize(this);
         _disposed = true;
      }

      ~CsvReaderBase()
      {
         Cleanup();
      }

      #endregion
   }
}
