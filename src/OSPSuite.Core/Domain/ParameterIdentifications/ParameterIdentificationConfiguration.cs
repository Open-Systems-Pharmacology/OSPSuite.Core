using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Core.Domain.ParameterIdentifications
{
   public class ParameterIdentificationConfiguration : IUpdatable
   {
      public virtual LLOQMode LLOQMode { get; set; }
      public virtual RemoveLLOQMode RemoveLLOQMode { get; set; }
      public virtual bool CalculateJacobian { get; set; }

      public virtual ParameterIdentificationRunMode RunMode { get; set; } = new StandardParameterIdentificationRunMode();
      public virtual OptimizationAlgorithmProperties AlgorithmProperties { get; set; }

      public ParameterIdentificationConfiguration()
      {
         LLOQMode = LLOQModes.SimulationOutputAsObservedDataLLOQ;
         RemoveLLOQMode = RemoveLLOQModes.Never;
         CalculateJacobian = false;
      }

      public virtual bool AlgorithmIsDefined => !string.IsNullOrEmpty(AlgorithmProperties?.Name);

      public void UpdatePropertiesFrom(IUpdatable source, ICloneManager cloneManager)
      {
         var sourceConfiguration = source as ParameterIdentificationConfiguration;
         if (sourceConfiguration == null) return;

         LLOQMode = sourceConfiguration.LLOQMode;
         RemoveLLOQMode = sourceConfiguration.RemoveLLOQMode;
         CalculateJacobian = sourceConfiguration.CalculateJacobian;
         RunMode = sourceConfiguration.RunMode?.Clone();
         AlgorithmProperties = sourceConfiguration.AlgorithmProperties?.Clone();
      }
   }
}