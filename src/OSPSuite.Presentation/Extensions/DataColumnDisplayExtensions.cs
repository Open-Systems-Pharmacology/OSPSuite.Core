using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;

namespace OSPSuite.Presentation.Extensions
{
   public static class DataColumnDisplayExtensions
   {
      /// <summary>
      ///    Returns the name of the <paramref name="column" />, followed by the error type when the column is an arithmetic or
      ///    geometric standard deviation
      /// </summary>
      public static string NameWithErrorType(this DataColumn column)
      {
         var errorType = errorTypeFor(column.DataInfo?.AuxiliaryType);
         return errorType == null ? column.Name : Captions.NameWithErrorType(column.Name, errorType);
      }

      private static string errorTypeFor(AuxiliaryType? auxiliaryType) => auxiliaryType switch
      {
         AuxiliaryType.ArithmeticStdDev => Constants.STD_DEV_ARITHMETIC,
         AuxiliaryType.GeometricStdDev => Constants.STD_DEV_GEOMETRIC,
         _ => null
      };
   }
}
