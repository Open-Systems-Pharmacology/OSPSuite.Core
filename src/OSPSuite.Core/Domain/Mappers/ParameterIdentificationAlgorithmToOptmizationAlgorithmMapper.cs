using System.Linq;
using OSPSuite.Utility;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.ParameterIdentifications.Algorithms;
using IoC = OSPSuite.Utility.Container.IContainer;

namespace OSPSuite.Core.Domain.Mappers
{
   public interface IParameterIdentificationAlgorithmToOptmizationAlgorithmMapper : IMapper<OptimizationAlgorithmProperties, IOptimizationAlgorithm>
   {
   }

   public class ParameterIdentificationAlgorithmToOptmizationAlgorithmMapper : IParameterIdentificationAlgorithmToOptmizationAlgorithmMapper
   {
      private readonly IoC _container;

      public ParameterIdentificationAlgorithmToOptmizationAlgorithmMapper(IoC container)
      {
         _container = container;
      }

      public IOptimizationAlgorithm MapFrom(OptimizationAlgorithmProperties optimizationAlgorithmProperties)
      {
         var algorithm = _container.Resolve<IOptimizationAlgorithm>(optimizationAlgorithmProperties.Name);
         foreach (var property in optimizationAlgorithmProperties.Where(p => algorithm.Properties.Contains(p.Name)))
         {
            algorithm.Properties[property.Name].ValueAsObject = property.ValueAsObject;
         }
         return algorithm;
      }
   }
}