using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.ParameterIdentifications.Algorithms;
using OSPSuite.Presentation.Extensions;

namespace OSPSuite.Presentation.DTO.ParameterIdentifications
{
   public class ParameterIdentificationConfigurationDTO : ValidatableDTO
   {
      private readonly ParameterIdentificationConfiguration _parameterIdentificationConfiguration;

      public ParameterIdentificationConfigurationDTO(ParameterIdentificationConfiguration parameterIdentificationConfiguration)
      {
         _parameterIdentificationConfiguration = parameterIdentificationConfiguration;
      }

      public LLOQMode LLOQMode
      {
         get => _parameterIdentificationConfiguration.LLOQMode;
         set
         {
            _parameterIdentificationConfiguration.LLOQMode = value;
            OnPropertyChanged(()=>LLOQMode);
            OnPropertyChanged(() => LLOQModeDescription);
         }
      }

      public string LLOQModeDescription => LLOQMode.Description.FormatForDescription();

      public RemoveLLOQMode RemoveLLOQMode
      {
         get => _parameterIdentificationConfiguration.RemoveLLOQMode;
         set
         {
            _parameterIdentificationConfiguration.RemoveLLOQMode = value;
            OnPropertyChanged(() => RemoveLLOQMode);
            OnPropertyChanged(() => LLOQUsageDescription);
         }
      }

      public bool CalculateJacobian
      {
         get => _parameterIdentificationConfiguration.CalculateJacobian;
         set
         {
            _parameterIdentificationConfiguration.CalculateJacobian = value;
            OnPropertyChanged();
         }
      }

      public string LLOQUsageDescription => RemoveLLOQMode.Description.FormatForDescription();


      public IOptimizationAlgorithm OptimizationAlgorithm { get; set; }

      public OptimizationAlgorithmProperties Properties => _parameterIdentificationConfiguration.AlgorithmProperties;


      public ParameterIdentificationRunMode ParameterIdentificationRunMode
      {
         get => _parameterIdentificationConfiguration.RunMode;
         set => _parameterIdentificationConfiguration.RunMode = value;
      }

   }
}
