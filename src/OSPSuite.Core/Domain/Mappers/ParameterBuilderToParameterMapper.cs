using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain.Mappers
{
   public interface IParameterBuilderToParameterMapper : IBuilderMapper<IParameter, IParameter>
   {
   }

   internal class ParameterBuilderToParameterMapper : IParameterBuilderToParameterMapper
   {
      private readonly ICloneManagerForModel _cloneManagerForModel;
      private readonly IEntityTracker _entityTracker;

      public ParameterBuilderToParameterMapper(ICloneManagerForModel cloneManagerForModel, IEntityTracker entityTracker)
      {
         _cloneManagerForModel = cloneManagerForModel;
         _entityTracker = entityTracker;
      }

      public IParameter MapFrom(IParameter parameterBuilder, SimulationBuilder simulationBuilder)
      {
         var parameter = _cloneManagerForModel.Clone(parameterBuilder);
         //We reset the container criteria explicitly in the model instance 
         parameter.ContainerCriteria = null;
         _entityTracker.Track(parameter, parameterBuilder, simulationBuilder);

         return parameter;
      }
   }
}