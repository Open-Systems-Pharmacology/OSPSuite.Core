using IoC = OSPSuite.Utility.Container.IContainer;

namespace OSPSuite.Core.Domain.Builder
{
   public interface ISimulationBuilderFactory
   {
      SimulationBuilder CreateFor(SimulationConfiguration configuration);
   }

   public class SimulationBuilderFactory : ISimulationBuilderFactory
   {
      private readonly IoC _container;

      public SimulationBuilderFactory(IoC container)
      {
         _container = container;
      }

      public SimulationBuilder CreateFor(SimulationConfiguration configuration)
      {
         var simulationBuilder = _container.Resolve<SimulationBuilder>();
         simulationBuilder.PerformMerge(configuration);
         return simulationBuilder;
      }
   }
}