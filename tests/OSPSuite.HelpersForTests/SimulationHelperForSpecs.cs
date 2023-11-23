using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Helpers
{
   public class SimulationHelperForSpecs
   {
      private readonly ModelHelperForSpecs _modelHelper;
      private readonly IModelConstructor _modelConstructor;

      public SimulationHelperForSpecs(
         ModelHelperForSpecs modelHelper,
         IModelConstructor modelConstructor)
      {
         _modelHelper = modelHelper;
         _modelConstructor = modelConstructor;
      }

      public IModelCoreSimulation CreateSimulation()
      {
         var simulationConfiguration = _modelHelper.CreateSimulationConfiguration();
         var (model, _) = _modelConstructor.CreateModelFrom(simulationConfiguration, "SpecModel");
         return new ModelCoreSimulation {Configuration = simulationConfiguration, Model = model};
      }
   }
}