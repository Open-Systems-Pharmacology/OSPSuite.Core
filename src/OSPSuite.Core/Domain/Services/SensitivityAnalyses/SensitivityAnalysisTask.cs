using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain.SensitivityAnalyses;
using OSPSuite.Core.Events;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Services.SensitivityAnalyses
{
   public class SensitivityAnalysisTask : ISensitivityAnalysisTask
   {
      private readonly ISensitivityAnalysisFactory _sensitivityAnalysisFactory;
      private readonly IOSPSuiteExecutionContext _executionContext;
      private readonly ISensitivityAnalysisSimulationSwapCorrector _sensitivityAnalysisSimulationSwapCorrector;
      private readonly ISensitivityAnalysisSimulationSwapValidator _sensitivityAnalysisSimulationSwapValidator;
      private readonly ISensitivityParameterFactory _sensitivityParameterFactory;
      private readonly IDialogCreator _dialogCreator;

      public SensitivityAnalysisTask(
         ISensitivityAnalysisFactory sensitivityAnalysisFactory,
         IOSPSuiteExecutionContext executionContext,
         ISensitivityAnalysisSimulationSwapCorrector sensitivityAnalysisSimulationSwapCorrector,
         ISensitivityAnalysisSimulationSwapValidator sensitivityAnalysisSimulationSwapValidator,
         ISensitivityParameterFactory sensitivityParameterFactory,
         IDialogCreator dialogCreator)
      {
         _sensitivityAnalysisFactory = sensitivityAnalysisFactory;
         _executionContext = executionContext;
         _sensitivityAnalysisSimulationSwapCorrector = sensitivityAnalysisSimulationSwapCorrector;
         _sensitivityAnalysisSimulationSwapValidator = sensitivityAnalysisSimulationSwapValidator;
         _sensitivityParameterFactory = sensitivityParameterFactory;
         _dialogCreator = dialogCreator;
      }

      public void SwapSimulations(SensitivityAnalysis sensitivityAnalysis, ISimulation oldSimulation, ISimulation newSimulation)
      {
         loadSimulation(newSimulation);
         if (Equals(newSimulation, oldSimulation)) return;

         _sensitivityAnalysisSimulationSwapCorrector.CorrectSensitivityAnalysis(sensitivityAnalysis, oldSimulation, newSimulation);
         sensitivityAnalysis.SwapSimulations(oldSimulation, newSimulation);
         _executionContext.PublishEvent(new SimulationReplacedInParameterAnalyzableEvent(sensitivityAnalysis, oldSimulation, newSimulation));
      }

      public ValidationResult ValidateSwap(SensitivityAnalysis sensitivityAnalysis, ISimulation oldSimulation, ISimulation newSimulation)
      {
         loadSimulation(newSimulation);

         return Equals(oldSimulation, newSimulation) ? new ValidationResult() : _sensitivityAnalysisSimulationSwapValidator.ValidateSwap(sensitivityAnalysis, oldSimulation, newSimulation);
      }

      private void loadSimulation(ISimulation newSimulation) => _executionContext.Load(newSimulation);

      public SensitivityAnalysis CreateSensitivityAnalysis()
      {
         var sensitivityAnalysis = _sensitivityAnalysisFactory.Create();
         AddToProject(sensitivityAnalysis);
         return sensitivityAnalysis;
      }

      public SensitivityAnalysis CreateSensitivityAnalysisFor(ISimulation simulation)
      {
         var sensitivity = CreateSensitivityAnalysis();
         if (sensitivity != null)
            sensitivity.Simulation = simulation;
         return sensitivity;
      }

      public void UpdateSensitivityParameterName(SensitivityAnalysis sensitivityAnalysis, SensitivityParameter sensitivityParameter, string newName)
      {
         var oldName = sensitivityParameter.Name;
         sensitivityParameter.Name = newName;
         if (!sensitivityAnalysis.HasResults)
            return;

         sensitivityAnalysis.Results.UpdateSensitivityParameterName(oldName, newName);
         _executionContext.PublishEvent(new SensitivityAnalysisResultsUpdatedEvent(sensitivityAnalysis));
      }

      public void AddParametersTo(SensitivityAnalysis sensitivityAnalysis, IReadOnlyList<ParameterSelection> parameters)
      {
         parameters.Select(parameterSelection => _sensitivityParameterFactory.CreateFor(parameterSelection, sensitivityAnalysis))
            .Where(sensitivityParameter => sensitivityParameter != null)
            .Each(sensitivityAnalysis.AddSensitivityParameter);
      }

      public void AddToProject(SensitivityAnalysis sensitivityAnalysis) => addSensitivityAnalysisToProject(sensitivityAnalysis, _executionContext);

      public bool Delete(IReadOnlyList<SensitivityAnalysis> sensitivityAnalyses)
      {
         var res = _dialogCreator.MessageBoxYesNo(Captions.SensitivityAnalysis.ReallyDeleteSensitivityAnalyses(sensitivityAnalyses.AllNames()));
         if (res == ViewResult.No)
            return false;

         sensitivityAnalyses.Each(delete);
         return true;
      }

      private void delete(SensitivityAnalysis sensitivityAnalysis)
      {
         _executionContext.Project.RemoveSensitivityAnalysis(sensitivityAnalysis);
         _executionContext.Unregister(sensitivityAnalysis);
         _executionContext.PublishEvent(new SensitivityAnalysisDeletedEvent(sensitivityAnalysis));
      }

      private void addSensitivityAnalysisToProject(SensitivityAnalysis sensitivityAnalysis, IOSPSuiteExecutionContext executionContext)
      {
         var project = executionContext.Project;
         project.AddSensitivityAnalysis(sensitivityAnalysis);
         executionContext.Register(sensitivityAnalysis);
         executionContext.PublishEvent(new SensitivityAnalysisCreatedEvent(sensitivityAnalysis));
      }
   }
}