using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain.Repositories;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Presentation.Presenters.ParameterIdentifications;
using OSPSuite.Starter.Tasks;
using OSPSuite.Starter.Tasks.Starters;
using OSPSuite.Utility.Container;

namespace OSPSuite.Starter.Presenters
{
   public interface IOptimizationStarter : ITestStarter
   {
   }

   public class OptimizationStarter : IOptimizationStarter
   {
      private readonly IShellPresenter _shellPresenter;
      private readonly IParameterIdentificationTask _parameterIdentificationTask;
      private readonly ISimulationRepository _simulationRepository;

      public OptimizationStarter(IShellPresenter shellPresenter, IParameterIdentificationTask parameterIdentificationTask, ISimulationRepository simulationRepository)
      {
         _shellPresenter = shellPresenter;
         _parameterIdentificationTask = parameterIdentificationTask;
         _simulationRepository = simulationRepository;
      }

      public void StartParameterIdentificationTest()
      {
      }

      public void Start(int width = 0, int height = 0)
      {
         _shellPresenter.Start();
         var paramterIdentification = _parameterIdentificationTask.CreateParameterIdentificationBasedOn(_simulationRepository.All());
         var presenter = IoC.Resolve<IEditParameterIdentificationPresenter>();
         presenter.InitializeWith(new OSPSuiteMacroCommand<OSPSuiteExecutionContext>());
         presenter.Edit(paramterIdentification);
      }
   }
}