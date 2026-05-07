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
         var value = parameterValue.Value;
         var dimension = parameterValue.Dimension;
         var displayUnit = parameterValue.DisplayUnit;
         var distributionType = parameterValue.DistributionType;

         //if a distribution is defined we create a distributed parameter; the value (if any) is applied as a
         //fixed value while the distribution formula and its sub-parameters are kept intact.
         //Otherwise, we create a plain parameter whose constant formula carries the value.
         var parameter = parameterValue.IsDistributed()
            ? _parameterFactory.CreateDistributedParameter(name, distributionType.Value, value: value, dimension: dimension, displayUnit: displayUnit)
            : _parameterFactory.CreateParameter(name, value: value, dimension: dimension, displayUnit: displayUnit);

         parameter.ValueOrigin.UpdateAllFrom(parameterValue.ValueOrigin);

         return parameter.WithUpdatedMetaFrom(parameterValue);
      }
   }
}
