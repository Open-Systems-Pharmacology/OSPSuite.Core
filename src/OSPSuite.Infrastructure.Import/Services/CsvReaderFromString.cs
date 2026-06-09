using System.IO;
using LumenWorks.Framework.IO.Csv;

namespace OSPSuite.Infrastructure.Import.Services
{
   public class CsvReaderFromString : CsvReaderBase
   {
      private readonly StringReader _stringReader;

      public CsvReaderFromString(string csvContent, char delimiter = ',')
      {
         _stringReader = new StringReader(csvContent);
         Csv = new CsvReader(_stringReader, hasHeaders: true, delimiter: delimiter);
      }

      protected override void Cleanup()
      {
         Csv?.Dispose();
         _stringReader?.Dispose();
      }
   }
}
