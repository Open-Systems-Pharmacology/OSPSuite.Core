using System.Collections.Generic;
using OSPSuite.Assets;

namespace OSPSuite.Infrastructure.Import.Core
{
   public class MissingColumnException : AbstractImporterException
   {
      public MissingColumnException(IReadOnlyList<string> missingColumns) : base(Error.MissingColumnException(missingColumns))
      {
      }

      public MissingColumnException(string missingColumn) : base(Error.MissingColumnException(new List<string>() { missingColumn}))
      {
      }
   }
}
