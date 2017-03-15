using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Validation;
using OSPSuite.Core.Domain.SensitivityAnalyses;

namespace OSPSuite.Core.Domain.Services
{
   public interface ISensitivityAnalysisValidator
   {
      ValidationResult Validate(SensitivityAnalysis sensitivityAnalysis);
   }

   public class SensitivityAnalysisValidator : ISensitivityAnalysisValidator
   {
      public ValidationResult Validate(SensitivityAnalysis sensitivityAnalysis)
      {
         var validationResult = new ValidationResult();
         var simulation = sensitivityAnalysis.Simulation;
         if (simulation == null)
            validationResult.AddMessage(NotificationType.Error, sensitivityAnalysis, Error.SensitivityAnalysis.NoSimulationDefined);

         var nonValidSensitivityAnalyses = sensitivityAnalysis.AllSensitivityParameters.Where(x => !x.IsValid()).ToList();

         if (nonValidSensitivityAnalyses.Any())
         {
            nonValidSensitivityAnalyses.Each(x =>
            {
               validationResult.AddMessage(NotificationType.Error, sensitivityAnalysis, Error.SensitivityParameterIsInvalid(x.Name));
            });

            // Exit early when sensitivity analyses are invalid;
            return validationResult;
         }

         if (simulation != null && !simulation.OutputSelections.HasSelection)
            validationResult.AddMessage(NotificationType.Error, sensitivityAnalysis, Error.SensitivityAnalysis.NoOutputAvailableInSelectedSimulation(simulation.Name));

         if (!sensitivityAnalysis.AllSensitivityParameters.Any())
            validationResult.AddMessage(NotificationType.Error, sensitivityAnalysis, Error.SensitivityAnalysis.NoSensitivityParameterDefined);

         if (sensitivityAnalysis.AllSensitivityParameters.All(x => x.DefaultValue == 0))
            validationResult.AddMessage(NotificationType.Error, sensitivityAnalysis, Error.SensitivityAnalysis.OnlyInactiveSensitivityParameterDefined);

         return validationResult;
      }
   }
}