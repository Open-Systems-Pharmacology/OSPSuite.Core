using System.Linq;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Utility;

namespace OSPSuite.Core.Chart.Mappers
{
   public enum DataMode
   {
      Invalid,
      SingleValue,
      StdDevA,
      StdDevG,
      StdDevAPop,
      StdDevGPop
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

         if (curve.yData.RelatedColumns.Count() > 1)
            return DataMode.Invalid;

         if (curve.yData.ContainsRelatedColumn(AuxiliaryType.ArithmeticStdDev))
            return DataMode.StdDevA;

         if (curve.yData.ContainsRelatedColumn(AuxiliaryType.GeometricStdDev))
            return DataMode.StdDevG;

         if (curve.yData.ContainsRelatedColumn(AuxiliaryType.ArithmeticMeanPop))
            return DataMode.StdDevAPop;

         if (curve.yData.ContainsRelatedColumn(AuxiliaryType.GeometricMeanPop))
            return DataMode.StdDevGPop;

         return DataMode.Invalid;
      }
   }
}