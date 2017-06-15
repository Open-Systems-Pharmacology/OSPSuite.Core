using System.Linq;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Utility;

namespace OSPSuite.Core.Chart.Mappers
{
   public enum DataMode
   {
      Invalid,
      SingleValue,
      ArithmeticStdDev,
      GeometricStdDev,
      ArithmeticMeanArea,
      GeometricMeanArea
   }

   public interface ICurveToDataModeMapper : IMapper<Curve, DataMode>
   {
   }

   public class CurveToDataModeMapper : ICurveToDataModeMapper
   {
      public DataMode MapFrom(Curve curve)
      {
         if (!curve.yData.RelatedColumns.Any())
            return DataMode.SingleValue;

         if (curve.yData.RelatedColumns.Count > 1)
            return DataMode.Invalid;

         if (curve.yData.ContainsRelatedColumn(AuxiliaryType.ArithmeticStdDev))
            return DataMode.ArithmeticStdDev;

         if (curve.yData.ContainsRelatedColumn(AuxiliaryType.GeometricStdDev))
            return DataMode.GeometricStdDev;

         if (curve.yData.ContainsRelatedColumn(AuxiliaryType.ArithmeticMeanPop))
            return DataMode.ArithmeticMeanArea;

         if (curve.yData.ContainsRelatedColumn(AuxiliaryType.GeometricMeanPop))
            return DataMode.GeometricMeanArea;

         return DataMode.Invalid;
      }
   }
}