using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.Core.Import;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Infrastructure.Import.Core.Exceptions
{
   public abstract class ParseErrorDescription
   {
      public string Message { get; protected set; }
   }

   public class NoMappedDimensionForColumn : ParseErrorDescription
   {
      public NoMappedDimensionForColumn(string sheetName, string columnName)
      {
         Message = Error.DimensionCannotBeDeterminedFor(sheetName, columnName);
      }
   }

   public class InvalidDimensionParseErrorDescription : ParseErrorDescription
   {
      public InvalidDimensionParseErrorDescription(string invalidUnit, string mappingName)
      {
         Message = Error.InvalidDimensionException(invalidUnit, mappingName);
      }
   }

   public class NaNParseErrorDescription : ParseErrorDescription
   {
      public NaNParseErrorDescription()
      {
         Message = Error.NaNOnData;
      }
   }

   public class MismatchingArrayLengthsParseErrorDescription : ParseErrorDescription
   {
      public MismatchingArrayLengthsParseErrorDescription()
      {
         Message = Error.MismatchingArrayLengths;
      }
   }

   public class InconsistentDimensionBetweenUnitsParseErrorDescription : ParseErrorDescription
   {
      public InconsistentDimensionBetweenUnitsParseErrorDescription(string mappingName)
      {
         Message = Error.InconsistentDimensionBetweenUnitsException(mappingName);
      }
   }

   public class ErrorUnitParseErrorDescription : ParseErrorDescription
   {
      public ErrorUnitParseErrorDescription()
      {
         Message = Error.InvalidErrorDimension;
      }
   }

   public class UnknownErrorTypeParseErrorDescription : ParseErrorDescription
   {
      public UnknownErrorTypeParseErrorDescription(string sheetName, string dataSetName, string errorType)
      {
         Message = Error.UnknownErrorType(sheetName, dataSetName, errorType, ErrorStdDevValues.AcceptedValues);
      }
   }

   public class ConflictingErrorTypesParseErrorDescription : ParseErrorDescription
   {
      public ConflictingErrorTypesParseErrorDescription(string sheetName, string dataSetName, IEnumerable<string> errorTypes)
      {
         Message = Error.ConflictingErrorTypes(sheetName, dataSetName, errorTypes);
      }
   }

   public class EmptyDataSetsParseErrorDescription : ParseErrorDescription
   {
      public EmptyDataSetsParseErrorDescription(IEnumerable<string> dataSetNames)
      {
         Message = Error.EmptyDataSet($"{dataSetNames.ToString(", ")}");
      }
   }

}