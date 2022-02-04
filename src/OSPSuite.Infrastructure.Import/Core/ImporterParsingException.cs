using OSPSuite.Assets;
using OSPSuite.Infrastructure.Import.Core.Exceptions;
using OSPSuite.Utility.Collections;
using System.Collections.Generic;
using System.Linq;

namespace OSPSuite.Infrastructure.Import.Core
{
   public class ImporterParsingException : AbstractImporterException
   {
      public ParseErrors FaultyDataSet { get; private set; }

      public ImporterParsingException(ParseErrors faultyDataSets)
         : base(Error.SimpleParseErrorMessage)
      {
         FaultyDataSet = faultyDataSets;
      }
   }
}