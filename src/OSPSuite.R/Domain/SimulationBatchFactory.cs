using OSPSuite.Core.Domain;
using OSPSuite.Core.Services;
using IContainer = OSPSuite.Utility.Container.IContainer;

namespace OSPSuite.R.Domain
{
   public interface ISimulationBatchFactory
   {
      SimulationBatch Create(IModelCoreSimulation modelCoreSimulation, SimulationBatchOptions simulationBatchOptions);
   }

   public class SimulationBatchFactory : DynamicFactory<SimulationBatch>, ISimulationBatchFactory
   {
      public SimulationBatchFactory(IContainer container) : base(container)
      {
      }

      public SimulationBatch Create(IModelCoreSimulation modelCoreSimulation, SimulationBatchOptions simulationBatchOptions)
      {
         var simulationBatch = Create();
         simulationBatch.Initialize(modelCoreSimulation, simulationBatchOptions);
         return simulationBatch;
      }
   }
}