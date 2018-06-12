using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Presentation.DTO;
using OSPSuite.Presentation.DTO.ParameterIdentifications;
using OSPSuite.Presentation.Services.ParameterIdentifications;
using OSPSuite.Utility.Extensions;

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
            addOptimizedParameterDTOTo(runResultDTO, optimizedParameterDTO);
         });

         parameterIdentification.AllFixedIdentificationParameters.Each(x =>
         {
            var fixedParameterDTO = mapFrom(x, x.StartValue, x.StartValue);
            addOptimizedParameterDTOTo(runResultDTO, fixedParameterDTO);
         });

         return runResultDTO;
      }

      private void addOptimizedParameterDTOTo(ParameterIdentificationRunResultDTO runResultDTO, OptimizedParameterDTO optimizedParameterDTO)
      {
         if (optimizedParameterDTO != null)
         {
            runResultDTO.AddOptimizedParameter(optimizedParameterDTO);
            if (runResultDTO.LegendImage == null)
               runResultDTO.LegendImage = _optimizedParameterRangeImageCreator.CreateLegendFor(optimizedParameterDTO);
         }
      }

      private OptimizedParameterDTO mapFrom(OptimizedParameterValue optimizedParameterValue, ParameterIdentification parameterIdentification)
      {
         var identificationParameter = parameterIdentification.IdentificationParameterByName(optimizedParameterValue.Name);
         if (identificationParameter == null)
            return null;

         return mapFrom(identificationParameter, optimizedParameterValue.Value, optimizedParameterValue.StartValue);
      }

      private OptimizedParameterDTO mapFrom(IdentificationParameter identificationParameter, double optimalValue, double startValue)
      {
         var dto = new OptimizedParameterDTO
         {
            Name = identificationParameter.Name,
            OptimalValue = mapFrom(optimalValue, identificationParameter.StartValueParameter),
            StartValue = mapFrom(startValue, identificationParameter.StartValueParameter),
            MinValue = mapFrom(identificationParameter.MinValue, identificationParameter.MinValueParameter),
            MaxValue = mapFrom(identificationParameter.MaxValue, identificationParameter.MaxValueParameter),
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