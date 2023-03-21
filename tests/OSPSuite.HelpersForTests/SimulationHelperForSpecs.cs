using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Helpers
{
   public class SimulationHelperForSpecs : ModelHelperForSpecs
   {
      private readonly IModelConstructor _modelConstructor;

      public SimulationHelperForSpecs(IObjectBaseFactory objectBaseFactory, IParameterStartValuesCreator parameterStartValuesCreator, IMoleculeStartValuesCreator moleculeStartValuesCreator, IObjectPathFactory objectPathFactory, IDimensionFactory dimensionFactory, IModelConstructor modelConstructor,
         ISpatialStructureFactory spatialStructureFactory, INeighborhoodBuilderFactory neighborhoodFactory, IOutputSchemaFactory outputSchemaFactory, IMoleculeBuilderFactory moleculeBuilderFactory, ISolverSettingsFactory solverSettingsFactory)
         : base(objectBaseFactory, parameterStartValuesCreator, moleculeStartValuesCreator, objectPathFactory,
            dimensionFactory, spatialStructureFactory, neighborhoodFactory, outputSchemaFactory, moleculeBuilderFactory, solverSettingsFactory)
      {
         _modelConstructor = modelConstructor;
      }

      public IModelCoreSimulation CreateSimulation()
      {
         var simulationConfiguration = CreateSimulationConfiguration();
         var (model, _) = _modelConstructor.CreateModelFrom(simulationConfiguration, "SpecModel");
         return new ModelCoreSimulation {Configuration = simulationConfiguration, Model = model};
      }
   }
}