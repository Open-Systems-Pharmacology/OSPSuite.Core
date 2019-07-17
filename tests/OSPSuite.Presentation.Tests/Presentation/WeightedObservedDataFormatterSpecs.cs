using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Helpers;
using OSPSuite.Presentation.DTO.ParameterIdentifications;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;

namespace OSPSuite.Presentation.Presentation
{
   public abstract class concern_for_WeightedObservedDataFormatter : ContextSpecification<WeightedObservedDataFormatter>
   {
      private OutputMappingDTO _outputMappingDTO;
      protected OutputMapping _outputMapping;
      private DataRepository _observedData;

      protected override void Context()
      {
         _observedData = DomainHelperForSpecs.ObservedData("TOTO");
         _outputMapping = new OutputMapping {WeightedObservedData = new WeightedObservedData(_observedData) {Id = 5}};
         _outputMappingDTO = new OutputMappingDTO(_outputMapping);
         sut = new WeightedObservedDataFormatter(_outputMappingDTO);

      }
   }

   public class When_formatting_a_weighted_observed_data_for_a_given_output_mapping : concern_for_WeightedObservedDataFormatter
   {
      [Observation]
      public void should_return_the_display_name_of_the_weighted_observed_data()
      {
         sut.Format(null).ShouldBeEqualTo(_outputMapping.WeightedObservedData.DisplayName);
      }
   }
}	