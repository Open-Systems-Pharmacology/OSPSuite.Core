using System.Collections.Generic;
using OSPSuite.Assets;

namespace OSPSuite.Infrastructure.Import.Core
{
   public class MissingColumnException : AbstractImporterException
   {
      public MissingColumnException(string sheetName, IReadOnlyList<string> missingColumns) : base(Error.MissingColumnException(sheetName, missingColumns))
      {
      }

      public MissingColumnException(string sheetName, string missingColumn) : base(Error.MissingColumnException(sheetName, new List<string>() { missingColumn}))
      {
      }
   }
}
