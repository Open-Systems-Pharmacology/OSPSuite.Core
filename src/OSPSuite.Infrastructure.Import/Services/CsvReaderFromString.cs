using System;
using System.IO;
using System.Text;
using LumenWorks.Framework.IO.Csv;

namespace OSPSuite.Infrastructure.Import.Services
{
   public class CsvReaderFromString : IDisposable
   {
      private readonly StringReader _stringReader;

      public CsvReader Csv { get; }

      public CsvReaderFromString(string csvContent, char delimiter = ',')
      {
         _stringReader = new StringReader(csvContent);
         Csv = new CsvReader(_stringReader, hasHeaders: true, delimiter: delimiter);
      }

      protected virtual void Cleanup()
      {
         Csv?.Dispose();
         _stringReader?.Dispose();
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

      ~CsvReaderFromString()
      {
         Cleanup();
      }

      #endregion
   }
}
