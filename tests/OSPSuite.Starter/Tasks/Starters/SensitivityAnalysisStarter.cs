using OSPSuite.Starter.Presenters;

namespace OSPSuite.Starter.Tasks.Starters
{
   public interface ISensitivityAnalysisStarter : ITestStarter
   {

   }

   public class SensitivityAnalysisStarter : TestStarter<ISensitivityAnalysisStarterPresenter>, ISensitivityAnalysisStarter
   {
      public SensitivityAnalysisStarter(ISensitivityAnalysisStarterPresenter presenter) : base(presenter)
      {
      }
   }
}