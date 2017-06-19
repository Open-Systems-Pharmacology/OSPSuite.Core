using System.Linq;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain.Repositories;
using OSPSuite.Core.Domain.Services.SensitivityAnalyses;
using OSPSuite.Presentation.Presenters.SensitivityAnalyses;
using OSPSuite.Starter.Tasks;
using OSPSuite.Starter.Tasks.Starters;
using OSPSuite.Utility.Container;

namespace OSPSuite.Starter.Presenters
{
   public interface ISensitivityAnalysisStarter : ITestStarter
   {
   }

   public class SensitivityAnalysisStarter : ISensitivityAnalysisStarter

   {
      private readonly IShellPresenter _shellPresenter;
      private readonly ISensitivityAnalysisTask _sensitivityAnalysisTask;
      private readonly ISimulationRepository _simulationRepository;

      public SensitivityAnalysisStarter(IShellPresenter shellPresenter, ISensitivityAnalysisTask sensitivityAnalysisTask, ISimulationRepository simulationRepository)
      {
         _shellPresenter = shellPresenter;
         _sensitivityAnalysisTask = sensitivityAnalysisTask;
         _simulationRepository = simulationRepository;
      }

      public void Start(int width = 0, int height = 0)
      {
         _shellPresenter.Start();
         var sensitivityAnalysis = _sensitivityAnalysisTask.CreateSensitivityAnalysisFor(_simulationRepository.All().First());
         var presenter = IoC.Resolve<IEditSensitivityAnalysisPresenter>();
         presenter.InitializeWith(new OSPSuiteMacroCommand<OSPSuiteExecutionContext>());
         presenter.Edit(sensitivityAnalysis);
      }
   }
}