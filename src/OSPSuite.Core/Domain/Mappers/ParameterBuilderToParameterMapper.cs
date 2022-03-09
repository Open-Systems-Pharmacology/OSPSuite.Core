using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain.Mappers
{
   public interface IParameterBuilderToParameterMapper : IBuilderMapper<IParameter, IParameter>
   {
   }

   public class ParameterBuilderToParameterMapper : IParameterBuilderToParameterMapper
   {
      private readonly ICloneManagerForModel _cloneManagerForModel;

      public ParameterBuilderToParameterMapper(ICloneManagerForModel cloneManagerForModel)
      {
         _cloneManagerForModel = cloneManagerForModel;
      }

      public IParameter MapFrom(IParameter parameterBuilder, IBuildConfiguration buildConfiguration)
      {
         var parameter = _cloneManagerForModel.Clone(parameterBuilder);
         //We reset the container criteria explicitly in the model instance 
         parameter.ContainerCriteria = null;
         buildConfiguration.AddBuilderReference(parameter, parameterBuilder);
         return parameter;
      }
   }
}