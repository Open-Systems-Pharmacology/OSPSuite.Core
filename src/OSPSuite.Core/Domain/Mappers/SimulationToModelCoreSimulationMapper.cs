using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain.Mappers
{
   public interface ISimulationToModelCoreSimulationMapper 
   {
      /// <summary>
      /// Returns a new <see cref="ModelCoreSimulation"/> based on <paramref name="simulation"/>. If <paramref name="shouldCloneModel"/> is set to <c>true</c>, 
      /// the model of the <paramref name="simulation"/> will be cloned. Otherwise it will be simply referenced
      /// </summary>
      IModelCoreSimulation MapFrom(ISimulation simulation, bool shouldCloneModel);

      /// <summary>
      /// Returns a new <see cref="ModelCoreSimulation"/> based on <paramref name="simulation"/> and <paramref name="buildConfiguration"/>. If <paramref name="shouldCloneModel"/> is set to <c>true</c>, 
      /// the model of the <paramref name="simulation"/> will be cloned. Otherwise it will be simply referenced
      /// </summary>
      IModelCoreSimulation MapFrom(ISimulation simulation, IBuildConfiguration buildConfiguration, bool shouldCloneModel);
   }

   public class SimulationToModelCoreSimulationMapper : ISimulationToModelCoreSimulationMapper
   {
      private readonly IIdGenerator _idGenerator;
      private readonly ICloneManagerForBuildingBlock _cloneManagerForBuildingBlock;
      private readonly ICloneManagerForModel _cloneManagerForModel;

      public SimulationToModelCoreSimulationMapper(IIdGenerator idGenerator, ICloneManagerForBuildingBlock cloneManagerForBuildingBlock, ICloneManagerForModel cloneManagerForModel)
      {
         _idGenerator = idGenerator;
         _cloneManagerForBuildingBlock = cloneManagerForBuildingBlock;
         _cloneManagerForModel = cloneManagerForModel;
      }

      public IModelCoreSimulation MapFrom(ISimulation simulation, bool shouldCloneModel)
      {
         return MapFrom(simulation, new BuildConfiguration {SimulationSettings = _cloneManagerForBuildingBlock.Clone(simulation.SimulationSettings)},shouldCloneModel);
      }

      public IModelCoreSimulation MapFrom(ISimulation simulation, IBuildConfiguration buildConfiguration, bool shouldCloneModel)
      {
         var modelToUse = modelFrom(simulation.Model, shouldCloneModel);
         return new ModelCoreSimulation
         {
            BuildConfiguration = buildConfiguration,
            Model = modelToUse,
            Creation = simulation.Creation,
            Id = _idGenerator.NewId(),
            Name = simulation.Name
         };
      }

      private IModel modelFrom(IModel sourceModel, bool shouldCloneModel)
      {
         if (!shouldCloneModel)
            return sourceModel;

         return _cloneManagerForModel.CloneModel(sourceModel);
      }
   }
}