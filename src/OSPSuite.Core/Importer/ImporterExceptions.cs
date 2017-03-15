using OSPSuite.Utility.Exceptions;

namespace OSPSuite.Core.Importer
{
   public class ColumnNotFoundException : OSPSuiteException
   {
      public ColumnNotFoundException(string columnName) : base($"The column {columnName} could not be found.")
      {
      }
   }

   public class NoMappingForDataColumnException : OSPSuiteException
   {
      public NoMappingForDataColumnException(string columnName) : base($"There is no mapping for required table column {columnName}.")
      {
      }
   }

   public class ExcelColumnNotFoundException : OSPSuiteException
   {
      public ExcelColumnNotFoundException(string columnName) : base($"Mapped column {columnName} could not be found in the excel file.")
      {
      }
   }

   public class InvalidUnitForExcelColumnException : OSPSuiteException
   {
      public InvalidUnitForExcelColumnException(string unit, string columnName) : base($"The unit {unit} is invalid for column {columnName}.")
      {
      }
   }

   public class DataColumnHasNoUnitInformationException : OSPSuiteException
   {
      public DataColumnHasNoUnitInformationException(string unit, string columnName, string dataColumnName) : base($"The unit {unit} is not supported for column {columnName}, because data column {dataColumnName} does not expect units.")
      {
      }
   }

   public class DifferentDataTypeException : OSPSuiteException
   {
      public DifferentDataTypeException(string columnName, string dataColumnName) : base($"The data types of the mapped columns ({dataColumnName} - {columnName}) are different.")
      {
      }
   }

   public class SheetNotFoundException : OSPSuiteException
   {
      public SheetNotFoundException(string sheetName, string fileName) : base($"The sheet {sheetName} could not be found in {fileName}.")
      {
      }
   }
}