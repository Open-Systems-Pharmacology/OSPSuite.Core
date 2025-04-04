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
      private readonly IObjectTracker _objectTracker;

      public ParameterBuilderToParameterMapper(ICloneManagerForModel cloneManagerForModel, IObjectTracker objectTracker)
      {
         _cloneManagerForModel = cloneManagerForModel;
         _objectTracker = objectTracker;
      }

      public IParameter MapFrom(IParameter parameterBuilder, SimulationBuilder simulationBuilder)
      {
         var parameter = _cloneManagerForModel.Clone(parameterBuilder);
         //We reset the container criteria explicitly in the model instance 
         parameter.ContainerCriteria = null;
         simulationBuilder.AddBuilderReference(parameter, parameterBuilder);
         _objectTracker.TrackObject(parameter, parameterBuilder, simulationBuilder);

         return parameter;
      }
   }
}