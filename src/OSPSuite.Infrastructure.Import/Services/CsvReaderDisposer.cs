using System;
using System.IO;
using System.Text;
using LumenWorks.Framework.IO.Csv;

namespace OSPSuite.Infrastructure.Import.Services
{
   public class CsvReaderDisposer : IDisposable
   {
      private readonly FileStream _fsReader;

      public CsvReader Csv { get; }

      public CsvReaderDisposer(string fileFullPath, char delimiter = ',')
      {
         _fsReader = new FileStream(fileFullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
         Csv = new CsvReader(new StreamReader(_fsReader, Encoding.UTF8), hasHeaders: true, delimiter: delimiter);
      }

      protected virtual void Cleanup()
      {
         Csv?.Dispose();

         _fsReader?.Dispose();
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

      ~CsvReaderDisposer()
      {
         Cleanup();
      }

      #endregion
   }
}