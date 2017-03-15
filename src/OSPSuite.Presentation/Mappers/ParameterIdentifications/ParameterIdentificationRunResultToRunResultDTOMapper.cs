using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.DTO.ParameterIdentifications;
using OSPSuite.Presentation.Services.ParameterIdentifications;

namespace OSPSuite.Presentation.Mappers.ParameterIdentifications
{
   public interface IParameterIdentificationRunResultToRunResultDTOMapper
   {
      ParameterIdentificationRunResultDTO MapFrom(ParameterIdentification parameterIdentification, ParameterIdentificationRunResult runResult);
   }

   public class ParameterIdentificationRunResultToRunResultDTOMapper : IParameterIdentificationRunResultToRunResultDTOMapper
   {
      private readonly IOptimizedParameterRangeImageCreator _optimizedParameterRangeImageCreator;

      public ParameterIdentificationRunResultToRunResultDTOMapper(IOptimizedParameterRangeImageCreator optimizedParameterRangeImageCreator)
      {
         _optimizedParameterRangeImageCreator = optimizedParameterRangeImageCreator;
      }

      public ParameterIdentificationRunResultDTO MapFrom(ParameterIdentification parameterIdentification, ParameterIdentificationRunResult runResult)
      {
         var runResultDTO = new ParameterIdentificationRunResultDTO(runResult);
        
         runResult.BestResult.Values.Each(x =>
         {
            var optimizedParameterDTO = mapFrom(x, parameterIdentification);
            if (optimizedParameterDTO != null)
            {
               runResultDTO.AddOptimizedParameter(optimizedParameterDTO);
               if (runResultDTO.LegendImage == null)
                  runResultDTO.LegendImage = _optimizedParameterRangeImageCreator.CreateLegendFor(optimizedParameterDTO);
            }

         });

         return runResultDTO;
      }

      private OptimizedParameterDTO mapFrom(OptimizedParameterValue optimizedParameterValue, ParameterIdentification parameterIdentification)
      {
         var identificationParameter = parameterIdentification.IdentificationParameterByName(optimizedParameterValue.Name);
         if (identificationParameter == null)
            return null;

         var dto= new OptimizedParameterDTO
         {
            Name = optimizedParameterValue.Name,
            OptimalValue = mapFrom(optimizedParameterValue.Value, identificationParameter.StartValueParameter),
            StartValue = mapFrom(optimizedParameterValue.StartValue, identificationParameter.StartValueParameter),
            MinValue = mapFrom(identificationParameter.MinValueParameter.Value, identificationParameter.MinValueParameter),
            MaxValue = mapFrom(identificationParameter.MaxValueParameter.Value, identificationParameter.MaxValueParameter),
            Scaling = identificationParameter.Scaling
         };
         dto.RangeImage = _optimizedParameterRangeImageCreator.CreateFor(dto);
         return dto;
      }

      private ValueDTO mapFrom(double value, IParameter parameter)
      {
         return new ValueDTO
         {
            Value = value,
            Dimension = parameter.Dimension,
            DisplayUnit = parameter.DisplayUnit
         };
      }
   }
}