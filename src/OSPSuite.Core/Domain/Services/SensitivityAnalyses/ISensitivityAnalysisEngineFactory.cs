using OSPSuite.Core.Services;

namespace OSPSuite.Core.Domain.Services.SensitivityAnalyses
{
   public interface ISensitivityAnalysisEngineFactory
   {
      ISensitivityAnalysisEngine Create();
   }

   class SensitivityAnalysisEngineFactory : DynamicFactory<ISensitivityAnalysisEngine>, ISensitivityAnalysisEngineFactory
   {
      public SensitivityAnalysisEngineFactory(Utility.Container.IContainer container) : base(container)
      {
      }
   }
}