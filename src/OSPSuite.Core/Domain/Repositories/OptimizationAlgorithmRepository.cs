using OSPSuite.Core.Domain.ParameterIdentifications.Algorithms;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Core.Domain.Repositories
{
   public interface IOptimizationAlgorithmRepository : IRepository<IOptimizationAlgorithm>
   {
   }

   public class OptimizationAlgorithmRepository : ImplementationRepository<IOptimizationAlgorithm>, IOptimizationAlgorithmRepository
   {
      public OptimizationAlgorithmRepository(Utility.Container.IContainer container) : base(container)
      {
      }
   }
}