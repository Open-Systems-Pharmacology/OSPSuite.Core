using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Presentation.DTO.ParameterIdentifications;
using OSPSuite.Utility.Validation;

namespace OSPSuite.Presentation.DTO
{
   public class SimulationOutputMappingDTO : OutputMappingDTO
   {
      public SimulationOutputMappingDTO(OutputMapping outputMapping) : base(outputMapping)
      {
         Rules.Clear();
         Rules.AddRange(SimulationOutputMappingRules.All());
      }

      private static class SimulationOutputMappingRules
      {
         private static readonly IBusinessRule _outputMatchesObservationDimension = CreateRule.For<SimulationOutputMappingDTO>()
            .Property(x => x.ObservedData)
            .WithRule((x, y) => x.Mapping.SimulationDimensionsAreConsistent(y))
            .WithError(Error.OutputMappingHasInconsistentDimension);

         private static readonly IBusinessRule _observedDataDefined = GenericRules.NotNull<SimulationOutputMappingDTO, DataRepository>(x => x.ObservedData);

         private static readonly IBusinessRule _outputWeightGreaterThanZero = CreateRule.For<SimulationOutputMappingDTO>()
            .Property(x => x.Weight)
            .WithRule((x, weight) => weight >= 0)
            .WithError(Error.WeightValueCannotBeNegative);

         internal static IEnumerable<IBusinessRule> All()
         {
            yield return _observedDataDefined;
            yield return _outputMatchesObservationDimension;
            yield return _outputWeightGreaterThanZero;
         }
      }
   }
}