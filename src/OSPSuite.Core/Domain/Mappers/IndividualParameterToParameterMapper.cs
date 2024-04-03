using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility;

namespace OSPSuite.Core.Domain.Mappers
{
   public interface IIndividualParameterToParameterMapper : IMapper<IndividualParameter, IParameter>
   {
   }

   public class IndividualParameterToParameterMapper : IIndividualParameterToParameterMapper
   {
      private readonly IParameterFactory _parameterFactory;

      public IndividualParameterToParameterMapper(IParameterFactory parameterFactory)
      {
         _parameterFactory = parameterFactory;
      }

      public IParameter MapFrom(IndividualParameter individualParameter)
      {
         var name = individualParameter.Name;
         var dimension = individualParameter.Dimension;
         var displayUnit = individualParameter.DisplayUnit;
         var distributionType = individualParameter.DistributionType;

         //if the distribution is undefined or the value is set, we create a default parameter to ensure that the value will take precedence.
         //Otherwise, we create a distributed parameter and assume that required sub-parameters will be created as well
         var parameter = !individualParameter.IsDistributed() || individualParameter.Value != null ? 
            _parameterFactory.CreateParameter(name, dimension: dimension, displayUnit: displayUnit) : 
            _parameterFactory.CreateDistributedParameter(name, distributionType.Value, dimension: dimension, displayUnit: displayUnit);

         return parameter.WithUpdatedMetaFrom(individualParameter);
      }
   }
}