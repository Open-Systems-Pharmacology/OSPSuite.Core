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

      public ParameterBuilderToParameterMapper(ICloneManagerForModel cloneManagerForModel)
      {
         _cloneManagerForModel = cloneManagerForModel;
      }

      public IParameter MapFrom(IParameter parameterBuilder, SimulationBuilder simulationBuilder)
      {
         var parameter = _cloneManagerForModel.Clone(parameterBuilder);
         //We reset the container criteria explicitly in the model instance 
         parameter.ContainerCriteria = null;
         simulationBuilder.AddBuilderReference(parameter, parameterBuilder);
         return parameter;
      }
   }
}