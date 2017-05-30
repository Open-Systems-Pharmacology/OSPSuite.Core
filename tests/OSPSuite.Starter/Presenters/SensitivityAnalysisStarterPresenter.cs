using System.Linq;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain.Repositories;
using OSPSuite.Core.Domain.Services.SensitivityAnalyses;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.SensitivityAnalyses;
using OSPSuite.Starter.Tasks;
using OSPSuite.Starter.Views;
using OSPSuite.Utility.Container;

namespace OSPSuite.Starter.Presenters
{
   public interface ISensitivityAnalysisStarterPresenter : IPresenter<ISensitivityAnalysisStarterView>
   {
      void StartSensitivityAnalysisTest();
   }

   public class SensitivityAnalysisStarterPresenter : AbstractPresenter<ISensitivityAnalysisStarterView, ISensitivityAnalysisStarterPresenter>, ISensitivityAnalysisStarterPresenter

   {
      private readonly IShellPresenter _shellPresenter;
      private readonly ISensitivityAnalysisTask _sensitivityAnalysisTask;
      private readonly ISimulationRepository _simulationRepository;

      public SensitivityAnalysisStarterPresenter(ISensitivityAnalysisStarterView view, IShellPresenter shellPresenter, ISensitivityAnalysisTask sensitivityAnalysisTask, ISimulationRepository simulationRepository) : base(view)
      {
         _shellPresenter = shellPresenter;
         _sensitivityAnalysisTask = sensitivityAnalysisTask;
         _simulationRepository = simulationRepository;
      }

      public void StartSensitivityAnalysisTest()
      {
         _shellPresenter.Start();
         var sensitivityAnalysis = _sensitivityAnalysisTask.CreateSensitivityAnalysisFor(_simulationRepository.All().First());
         var presenter = IoC.Resolve<IEditSensitivityAnalysisPresenter>();
         presenter.InitializeWith(new OSPSuiteMacroCommand<OSPSuiteExecutionContext>());
         presenter.Edit(sensitivityAnalysis);
      }
   }
}