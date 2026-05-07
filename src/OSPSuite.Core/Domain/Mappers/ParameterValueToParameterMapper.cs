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

         //if a distribution is defined we create a distributed parameter; the overwritten value (if any) is
         //applied as a fixed value via the IDistributedParameter Value setter, which keeps the distribution
         //formula intact and updates the percentile sub-parameter accordingly.
         //Otherwise, we create a plain parameter whose constant formula carries the value.
         IParameter parameter;
         if (parameterValue.IsDistributed())
         {
            parameter = _parameterFactory.CreateDistributedParameter(name, distributionType.Value, dimension: dimension, displayUnit: displayUnit);
            if (parameterValue.Value.HasValue)
               parameter.Value = parameterValue.Value.Value;
         }
         else
         {
            parameter = _parameterFactory.CreateParameter(name, value: parameterValue.Value, dimension: dimension, displayUnit: displayUnit);
         }

         parameter.ValueOrigin.UpdateAllFrom(parameterValue.ValueOrigin);

         return parameter.WithUpdatedMetaFrom(parameterValue);
      }
   }
}
