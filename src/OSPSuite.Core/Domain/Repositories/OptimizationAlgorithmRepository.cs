using OSPSuite.Utility.Collections;
using OSPSuite.Core.Domain.ParameterIdentifications.Algorithms;

namespace OSPSuite.Core.Domain.Repositories
{
   public interface IOptimizationAlgorithmRepository : IRepository<IOptimizationAlgorithm>
   {
   }

   public class OptimizationAlgorithmRepository : ImplementationRepository<IOptimizationAlgorithm>, IOptimizationAlgorithmRepository
   {
      public OptimizationAlgorithmRepository(OSPSuite.Utility.Container.IContainer container) : base(container)
      {
      }
   }
}