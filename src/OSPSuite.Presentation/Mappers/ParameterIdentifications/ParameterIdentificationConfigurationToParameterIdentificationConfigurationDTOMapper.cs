using System.Collections.Generic;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.ParameterIdentifications.Algorithms;
using OSPSuite.Presentation.DTO.ParameterIdentifications;

namespace OSPSuite.Presentation.Mappers.ParameterIdentifications
{
   public interface IParameterIdentificationConfigurationToParameterIdentificationConfigurationDTOMapper
   {
      ParameterIdentificationConfigurationDTO MapFrom(ParameterIdentificationConfiguration parameterIdentificationConfiguration, IReadOnlyList<IOptimizationAlgorithm> allAlgorithms);
   }

   public class ParameterIdentificationConfigurationToParameterIdentificationConfigurationDTOMapper : IParameterIdentificationConfigurationToParameterIdentificationConfigurationDTOMapper
   {
      public ParameterIdentificationConfigurationDTO MapFrom(ParameterIdentificationConfiguration parameterIdentificationConfiguration, IReadOnlyList<IOptimizationAlgorithm> allAlgorithms)
      {
         return new ParameterIdentificationConfigurationDTO(parameterIdentificationConfiguration)
         {
            OptimizationAlgorithm = algorithmByName(allAlgorithms, parameterIdentificationConfiguration.AlgorithmProperties?.Name)
         };
      }

      private IOptimizationAlgorithm algorithmByName(IEnumerable<IOptimizationAlgorithm> algorithms, string name)
      {
         return algorithms.Find(x => Equals(x.Name, name));
      }
   }
}