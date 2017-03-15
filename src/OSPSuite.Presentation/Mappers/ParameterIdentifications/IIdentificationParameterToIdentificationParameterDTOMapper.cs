using OSPSuite.Utility;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Presentation.DTO.ParameterIdentifications;

namespace OSPSuite.Presentation.Mappers.ParameterIdentifications
{
   public interface IIdentificationParameterToIdentificationParameterDTOMapper : IMapper<IdentificationParameter, IdentificationParameterDTO>
   {
   }

   public class IdentificationParameterToIdentificationParameterDTOMapper : IIdentificationParameterToIdentificationParameterDTOMapper
   {
      private readonly IParameterToParameterDTOInContainerMapper<IdentificationParameterDTO> _parameterMapper;

      public IdentificationParameterToIdentificationParameterDTOMapper(IParameterToParameterDTOInContainerMapper<IdentificationParameterDTO> parameterMapper)
      {
         _parameterMapper = parameterMapper;
      }

      public IdentificationParameterDTO MapFrom(IdentificationParameter identificationParameter)
      {
         var identificationParameterDTO = new IdentificationParameterDTO(identificationParameter);
         identificationParameterDTO.StartValueParameter = _parameterMapper.MapFrom(identificationParameter.StartValueParameter, identificationParameterDTO, x => x.StartValue, x => x.StartValueParameter);
         identificationParameterDTO.MinValueParameter = _parameterMapper.MapFrom(identificationParameter.MinValueParameter, identificationParameterDTO, x => x.MinValue, x => x.MinValueParameter);
         identificationParameterDTO.MaxValueParameter = _parameterMapper.MapFrom(identificationParameter.MaxValueParameter, identificationParameterDTO, x => x.MaxValue, x => x.MaxValueParameter);
         return identificationParameterDTO;
      }
   }
}