using OSPSuite.Utility;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.ParameterIdentifications.Algorithms;

namespace OSPSuite.Core.Domain.Mappers
{
   public interface IOptimizationAlgorithmToOptimizationAlgorithmPropertiesMapper : IMapper<IOptimizationAlgorithm, OptimizationAlgorithmProperties>
   {
   }

   public class OptimizationAlgorithmToOptimizationAlgorithmPropertiesMapper : IOptimizationAlgorithmToOptimizationAlgorithmPropertiesMapper
   {
      public OptimizationAlgorithmProperties MapFrom(IOptimizationAlgorithm optimizationAlgorithm)
      {
         return optimizationAlgorithm.Properties.Clone();
      }
   }
}