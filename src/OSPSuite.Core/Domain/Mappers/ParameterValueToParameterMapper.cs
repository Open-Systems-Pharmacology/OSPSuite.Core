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

         //if a distribution is defined we create a distributed parameter (even if a value is also provided:
         //the value is applied later as a fixed value while keeping the distribution intact).
         //Otherwise, we create a plain parameter.
         var parameter = parameterValue.IsDistributed()
            ? _parameterFactory.CreateDistributedParameter(name, distributionType.Value, dimension: dimension, displayUnit: displayUnit)
            : _parameterFactory.CreateParameter(name, dimension: dimension, displayUnit: displayUnit);

         parameter.ValueOrigin.UpdateAllFrom(parameterValue.ValueOrigin);

         return parameter.WithUpdatedMetaFrom(parameterValue);
      }
   }
}
