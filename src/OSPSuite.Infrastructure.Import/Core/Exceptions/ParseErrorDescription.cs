using System.Collections.Generic;
using OSPSuite.Assets;
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

   public class NoUnitColumnValues : ParseErrorDescription
   {
      public NoUnitColumnValues(string mappingName)
      {
         Message = Error.NoUnitColumnValues(mappingName);
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

   public class InvalidMappingColumnParseErrorDescription : ParseErrorDescription
   {
      public InvalidMappingColumnParseErrorDescription()
      {
         Message = Error.InvalidMappingColumn;
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

   public class EmptyDataSetsParseErrorDescription : ParseErrorDescription
   {
      public EmptyDataSetsParseErrorDescription(IEnumerable<string> dataSetNames)
      {
         Message = Error.EmptyDataSet($"{dataSetNames.ToString(", ")}");
      }
   }

   public class NonMonotonicalTimeParseErrorDescription : ParseErrorDescription
   {
      public NonMonotonicalTimeParseErrorDescription(string message)
      {
         Message = message;
      }
   }
}