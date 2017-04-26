using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Utility;
using OSPSuite.Core.Domain.SensitivityAnalyses;
using OSPSuite.Presentation.DTO.SensitivityAnalyses;

namespace OSPSuite.Presentation.Mappers.SensitivityAnalyses
{
   public interface ISensitivityParameterToSensitivityParameterDTOMapper : IMapper<SensitivityParameter,SensitivityParameterDTO>
   {
   }

   public class SensitivityParameterToSensitivityParameterDTOMapper : ISensitivityParameterToSensitivityParameterDTOMapper
   {
      private readonly IParameterToParameterDTOInContainerMapper<SensitivityParameterDTO> _parameterMapper;
      private readonly IQuantityPathToQuantityDisplayPathMapper _quantityDisplayPathMapper;

      public SensitivityParameterToSensitivityParameterDTOMapper(IParameterToParameterDTOInContainerMapper<SensitivityParameterDTO> parameterMapper, IQuantityPathToQuantityDisplayPathMapper quantityDisplayPathMapper)
      {
         _parameterMapper = parameterMapper;
         _quantityDisplayPathMapper = quantityDisplayPathMapper;
      }

      public SensitivityParameterDTO MapFrom(SensitivityParameter sensitivityParameter)
      {
         var sensitivityParameterDTO = new SensitivityParameterDTO(sensitivityParameter);
         sensitivityParameterDTO.NumberOfStepsParameter = _parameterMapper.MapFrom(sensitivityParameter.NumberOfStepsParameter, sensitivityParameterDTO, x => x.NumberOfSteps, x => x.NumberOfStepsParameter);
         sensitivityParameterDTO.VariationRangeParameter = _parameterMapper.MapFrom(sensitivityParameter.VariationRangeParameter, sensitivityParameterDTO, x => x.VariationRange, x => x.VariationRangeParameter);

         sensitivityParameterDTO.DisplayPath = sensitivityParameter.Parameter != null ? _quantityDisplayPathMapper.DisplayPathAsStringFor(sensitivityParameter.Parameter, addSimulationName: true) : sensitivityParameter.ParameterSelection.Path;
         return sensitivityParameterDTO;
      }
   }
}