using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.Utility.Validation;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;

namespace OSPSuite.Presentation.DTO.ParameterIdentifications
{
   public class OutputMappingDTO : DxValidatableDTO
   {
      public OutputMapping Mapping { get; private set; }
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
         get { return _output; }
         set
         {
            _output = value;
            Mapping.OutputSelection = new SimulationQuantitySelection(_output?.Simulation, _output?.QuantitySelectionDTO?.ToQuantitySelection());
            OnPropertyChanged(() => Output);
         }
      }

      public DataRepository ObservedData
      {
         get { return _observedData; }
         set
         {
            _observedData = value;
            OnPropertyChanged(() => ObservedData);
         }
      }

      public float Weight
      {
         get { return Mapping.Weight; }
         set
         {
            Mapping.Weight = value;
            OnPropertyChanged(() => Weight);
         }
      }

      public Scalings Scaling
      {
         get { return Mapping.Scaling; }
         set
         {
            if(Mapping.Scaling==value)
               return;

            Mapping.Scaling = value;
            OnPropertyChanged(() => Scaling);
         }
      }

      private static class OutputMappingRules
      {
         private static IBusinessRule outputMatchesObservationDimension
         {
            get
            {
               return CreateRule.For<OutputMappingDTO>()
                  .Property(x => x.ObservedData)
                  .WithRule((x, y) => x.Mapping.DimensionsAreConsistent(y))
                  .WithError(Error.OutputMappingHasInconsistentDimension);
            }
         }

         private static IBusinessRule outputDefined
         {
            get { return GenericRules.NotNull<OutputMappingDTO, SimulationQuantitySelectionDTO>(x => x.Output); }
         }

         private static IBusinessRule observedDataDefined
         {
            get { return GenericRules.NotNull<OutputMappingDTO, DataRepository>(x => x.ObservedData); }
         }

         private static IBusinessRule outputWeightGreaterThanZero
         {
            get
            {
               return CreateRule.For<OutputMappingDTO>()
                  .Property(x => x.Weight)
                  .WithRule((x, weight) => weight >= 0)
                  .WithError(Error.WeightValueCannotBeNegative);
            }
         }

         internal static IEnumerable<IBusinessRule> All()
         {
            yield return outputDefined;
            yield return observedDataDefined;
            yield return outputMatchesObservationDimension;
            yield return outputWeightGreaterThanZero;
         }
      }
   }
}