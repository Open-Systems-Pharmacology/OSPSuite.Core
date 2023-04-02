using OSPSuite.Assets;

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