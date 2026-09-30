using OSPSuite.Assets;

namespace OSPSuite.Infrastructure.Import.Core
{
   public class ImporterParsingException : AbstractImporterException
   {
      public ParseErrors FaultyDataSet { get; private set; }

      public ImporterParsingException(ParseErrors faultyDataSets)
         : this(faultyDataSets, Error.SimpleParseErrorMessage)
      {
      }

      public ImporterParsingException(ParseErrors faultyDataSets, string message)
         : base(message)
      {
         FaultyDataSet = faultyDataSets;
      }
   }
}