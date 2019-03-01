using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Utility.Validation;

namespace OSPSuite.Presentation.DTO.ParameterIdentifications
{
   public class OutputMappingDTO : DxValidatableDTO
   {
      public OutputMapping Mapping { get; }
      private SimulationQuantitySelectionDTO _output;
      private DataRepository _observedData;

      public OutputMappingDTO(OutputMapping outputMapping)
      {
         Mapping = outputMapping;
         _observedData = outputMapping.WeightedObservedData;
         Rules.AddRange(OutputMappingRules.All());
      }

      public WeightedObservedData WeightedObservedData => Mapping.WeightedObservedData;

      public SimulationQuantitySelectionDTO Output
      {
         get => _output;
         set
         {
            _output = value;
            Mapping.OutputSelection = new SimulationQuantitySelection(_output?.Simulation, _output?.QuantitySelectionDTO?.ToQuantitySelection());
            OnPropertyChanged(() => Output);
         }
      }

      public DataRepository ObservedData
      {
         get => _observedData;
         set => SetProperty(ref _observedData, value);
      }

      public float Weight
      {
         get => Mapping.Weight;
         set
         {
            Mapping.Weight = value;
            OnPropertyChanged(() => Weight);
         }
      }

      public Scalings Scaling
      {
         get => Mapping.Scaling;
         set
         {
            if (Mapping.Scaling == value)
               return;

            Mapping.Scaling = value;
            OnPropertyChanged(() => Scaling);
         }
      }

      private static class OutputMappingRules
      {
         private static readonly IBusinessRule _outputMatchesObservationDimension = CreateRule.For<OutputMappingDTO>()
            .Property(x => x.ObservedData)
            .WithRule((x, y) => x.Mapping.DimensionsAreConsistent(y))
            .WithError(Error.OutputMappingHasInconsistentDimension);

         private static readonly IBusinessRule _outputDefined = GenericRules.NotNull<OutputMappingDTO, SimulationQuantitySelectionDTO>(x => x.Output);

         private static readonly IBusinessRule _observedDataDefined = GenericRules.NotNull<OutputMappingDTO, DataRepository>(x => x.ObservedData);

         private static readonly IBusinessRule _outputWeightGreaterThanZero = CreateRule.For<OutputMappingDTO>()
            .Property(x => x.Weight)
            .WithRule((x, weight) => weight >= 0)
            .WithError(Error.WeightValueCannotBeNegative);

         internal static IEnumerable<IBusinessRule> All()
         {
            yield return _outputDefined;
            yield return _observedDataDefined;
            yield return _outputMatchesObservationDimension;
            yield return _outputWeightGreaterThanZero;
         }
      }
   }
}