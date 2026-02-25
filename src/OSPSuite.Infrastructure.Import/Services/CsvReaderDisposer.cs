using System;
using System.IO;
using System.Text;
using LumenWorks.Framework.IO.Csv;

namespace OSPSuite.Infrastructure.Import.Services
{
   public class CsvReaderDisposer : CsvReaderBase
   {
      private readonly FileStream _fsReader;

      public CsvReaderDisposer(string fileFullPath, char delimiter = ',')
      {
         _fsReader = new FileStream(fileFullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
         Csv = new CsvReader(new StreamReader(_fsReader, Encoding.UTF8), hasHeaders: true, delimiter: delimiter);
      }

      protected override void Cleanup()
      {
         Csv?.Dispose();
         _fsReader?.Dispose();
      }
   }
}