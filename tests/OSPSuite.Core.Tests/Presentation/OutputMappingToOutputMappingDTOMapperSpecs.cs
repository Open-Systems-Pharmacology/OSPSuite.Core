using System.Collections.Generic;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using FakeItEasy;
using OSPSuite.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Helpers;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.DTO.ParameterIdentifications;
using OSPSuite.Presentation.Mappers.ParameterIdentifications;

namespace OSPSuite.Presentation
{
   public abstract class concern_for_OutputMappingToOutputMappingDTOMapper : ContextSpecification<IOutputMappingToOutputMappingDTOMapper>
   {
      protected List<SimulationQuantitySelectionDTO> _availableOutputs;
      protected OutputMapping _outputMapping;
      protected OutputMappingDTO _dto;
      protected ISimulation _simulation;

      protected override void Context()
      {
         sut = new OutputMappingToOutputMappingDTOMapper();
         _simulation= A.Fake<ISimulation>();
         _availableOutputs =new List<SimulationQuantitySelectionDTO>();
         _outputMapping = new OutputMapping();
      }


      protected override void Because()
      {
         _dto = sut.MapFrom(_outputMapping, _availableOutputs);
      }
   }

   public class When_mapping_an_output_mapping_not_well_defined_to_an_output_mapping_dto : concern_for_OutputMappingToOutputMappingDTOMapper
   {
      [Observation]
      public void should_return_an_output_mapping_dto_with_undefined_properties()
      {
         _dto.ObservedData.ShouldBeNull();
         _dto.Output.ShouldBeNull();
         _dto.Mapping.ShouldBeEqualTo(_outputMapping);
      }
   }

   public class When_mapping_a_well_defined_output_mapping_to_an_output_mapping_dto : concern_for_OutputMappingToOutputMappingDTOMapper
   {
      private QuantitySelectionDTO _quantitySelectionDTO;

      protected override void Context()
      {
         base.Context();
         _outputMapping.WeightedObservedData = new WeightedObservedData(DomainHelperForSpecs.ObservedData());
         _quantitySelectionDTO = new QuantitySelectionDTO {QuantityPath = "AA", };
         _availableOutputs.Add(new SimulationQuantitySelectionDTO(_simulation,  _quantitySelectionDTO, "Display"));
         _outputMapping.OutputSelection = new SimulationQuantitySelection(_simulation, _quantitySelectionDTO.ToQuantitySelection());
      }

      [Observation]
      public void should_return_an_output_mapping_dto_with_defined_properties()
      {
         _dto.ObservedData.ShouldBeEqualTo(_outputMapping.WeightedObservedData);
         _dto.Output.QuantitySelectionDTO.ShouldBeEqualTo(_quantitySelectionDTO);
         _dto.Mapping.ShouldBeEqualTo(_outputMapping);
      }
   }
}	