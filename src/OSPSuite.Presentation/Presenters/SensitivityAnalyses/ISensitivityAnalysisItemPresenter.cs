using OSPSuite.Core.Domain.SensitivityAnalyses;

namespace OSPSuite.Presentation.Presenters.SensitivityAnalyses
{
   public interface ISensitivityAnalysisPresenter : IPresenter
   {
      void EditSensitivityAnalysis(SensitivityAnalysis sensitivityAnalysis);
   }

   public interface ISensitivityAnalysisItemPresenter : ISensitivityAnalysisPresenter, ISubPresenter
   {
   }
}