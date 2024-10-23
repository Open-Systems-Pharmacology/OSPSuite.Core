using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility;

namespace OSPSuite.Core.Domain.Mappers
{
   public interface IParameterValueToParameterMapper : IMapper<ParameterValue, IParameter>
   {
   }

   public class ParameterValueToParameterMapper : IParameterValueToParameterMapper
   {
      private readonly IParameterFactory _parameterFactory;

      public ParameterValueToParameterMapper(IParameterFactory parameterFactory)
      {
         _parameterFactory = parameterFactory;
      }

      public IParameter MapFrom(ParameterValue parameterValue)
      {
         var name = parameterValue.Name;
         var dimension = parameterValue.Dimension;
         var displayUnit = parameterValue.DisplayUnit;
         var distributionType = parameterValue.DistributionType;

         //if the distribution is undefined or the value is set, we create a default parameter to ensure that the value will take precedence.
         //Otherwise, we create a distributed parameter and assume that required sub-parameters will be created as well
         var parameter = !parameterValue.IsDistributed() || parameterValue.Value != null
            ? _parameterFactory.CreateParameter(name, dimension: dimension, displayUnit: displayUnit)
            : _parameterFactory.CreateDistributedParameter(name, distributionType.Value, dimension: dimension, displayUnit: displayUnit);

         parameter.ValueOrigin.UpdateAllFrom(parameterValue.ValueOrigin);

         return parameter.WithUpdatedMetaFrom(parameterValue);
      }
   }
}