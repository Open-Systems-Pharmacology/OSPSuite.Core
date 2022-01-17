using OSPSuite.Assets;
using OSPSuite.Infrastructure.Import.Core.Exceptions;
using OSPSuite.Utility.Collections;
using System.Collections.Generic;
using System.Linq;

namespace OSPSuite.Infrastructure.Import.Core
{
   public class ImporterParsingException : AbstractImporterException
   {
      public Cache<IDataSet, List<ParseErrorDescription>>FaultyDataSet { get; private set; }

      public ImporterParsingException(Cache<IDataSet, List<ParseErrorDescription>> faultyDataSets)
         : base(Error.SimpleParseErrorMessage)
      {
         FaultyDataSet = faultyDataSets;
      }
   }
}