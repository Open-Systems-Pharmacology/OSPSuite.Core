using System.Globalization;
using LumenWorks.Framework.IO.Csv;

namespace OSPSuite.Infrastructure.Import.Extensions
{
   public static class CsvReaderExtensions
   {
      /// <summary>
      ///    Returns the double value define in the current record at the index <paramref name="fieldIndex" />
      /// </summary>
      public static double DoubleAt(this CsvReader csv, int fieldIndex) => validateDoubleValue(csv[fieldIndex]);

      /// <summary>
      ///    Returns the double value define in the current record with field named <paramref name="fieldName" />
      /// </summary>
      public static double DoubleAt(this CsvReader csv, string fieldName) => validateDoubleValue(csv[fieldName]);

      private static double validateDoubleValue(string value) =>
         string.IsNullOrEmpty(value) ? double.NaN : double.Parse(value, NumberFormatInfo.InvariantInfo);

      /// <summary>
      ///    Returns the float value define in the current record at the index <paramref name="fieldIndex" />
      /// </summary>
      public static float FloatAt(this CsvReader csv, int fieldIndex) => validateFloatValue(csv[fieldIndex]);

      /// <summary>
      ///    Returns the float value define in the current record with field named <paramref name="fieldName" />
      /// </summary>
      public static float FloatAt(this CsvReader csv, string fieldName) => validateFloatValue(csv[fieldName]);

      private static float validateFloatValue(string value) =>
         string.IsNullOrEmpty(value) ? float.NaN : float.Parse(value, NumberFormatInfo.InvariantInfo);

      /// <summary>
      ///    Returns the int value define in the current record at the index <paramref name="fieldIndex" />
      /// </summary>
      public static int IntAt(this CsvReader csv, int fieldIndex)
      {
         return int.Parse(csv[fieldIndex], NumberFormatInfo.InvariantInfo);
      }

      /// <summary>
      ///    Returns the int value define in the current record at the index <paramref name="fieldName" />
      /// </summary>
      public static int IntAt(this CsvReader csv, string fieldName)
      {
         return int.Parse(csv[fieldName], NumberFormatInfo.InvariantInfo);
      }
   }
}