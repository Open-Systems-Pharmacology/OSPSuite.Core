using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Services
{
   public interface ISimulationConfigurationValidator
   {
      ValidationResult Validate(SimulationConfiguration simulationConfiguration);
   }

   class SimulationConfigurationValidator : ISimulationConfigurationValidator
   {
      public ValidationResult Validate(SimulationConfiguration simulationConfiguration)
      {
         var validationResult = new ValidationResult();
         var allMolecules = simulationConfiguration.ModuleConfigurations.Where(x => x.Module.Molecules != null).SelectMany(x => x.Module.Molecules).ToList();
         simulationConfiguration.ModuleConfigurations.Each(x =>
         {
            validateModule(x.Module, validationResult);
            validateEventGroupBuildingBlock(x.Module.EventGroups, allMolecules, validationResult);
         });

         simulationConfiguration.AllCalculationMethods.Each(cm => validateBuildingBlockWithFormulaCache(cm, validationResult));
         return validationResult;
      }

      private void validateModule(Module module, ValidationResult validationResult)
      {
         validateBuildingBlockWithFormulaCache(module.Molecules, validationResult);
         validateBuildingBlockWithFormulaCache(module.Reactions, validationResult);
         validateBuildingBlockWithFormulaCache(module.SpatialStructure, validationResult);
         validateBuildingBlockWithFormulaCache(module.PassiveTransports, validationResult);
         validateBuildingBlockWithFormulaCache(module.Observers, validationResult);
         module.InitialConditionsCollection.Each(cm => validateBuildingBlockWithFormulaCache(cm, validationResult));
         module.ParameterValuesCollection.Each(cm => validateBuildingBlockWithFormulaCache(cm, validationResult));
      }

      private void validateEventGroupBuildingBlock(EventGroupBuildingBlock eventGroups, IReadOnlyList<MoleculeBuilder> allMolecules, ValidationResult validationResult)
      {
         if (eventGroups == null || allMolecules == null)
            return;

         var allMoleculeNames = allMolecules.Select(mb => mb.Name);
         foreach (var eventGroup in eventGroups)
         {
            var applicationBuilders = eventGroup.GetAllContainersAndSelf<ApplicationBuilder>();
            foreach (var applicationBuilder in applicationBuilders.Where(applicationBuilder => !allMoleculeNames.Contains(applicationBuilder.MoleculeName)))
            {
               validationResult.AddMessage(NotificationType.Error, applicationBuilder, Validation.ApplicatedMoleculeNotPresent(applicationBuilder.MoleculeName, applicationBuilder.Name), eventGroups);
            }
         }

         validateBuildingBlockWithFormulaCache(eventGroups, validationResult);
      }

      private void validateBuildingBlockWithFormulaCache(IBuildingBlock buildingBlockWithFormulaCache, ValidationResult validationResult)
      {
         if (buildingBlockWithFormulaCache == null)
            return;

         foreach (var formula in buildingBlockWithFormulaCache.FormulaCache.Where(f => f.IsExplicit()).Cast<ExplicitFormula>())
         {
            var (valid, message) = formula.IsValid();
            if (!valid)
               validationResult.AddMessage(NotificationType.Error, formula, Validation.FormulaIsNotValid(formula.Name, buildingBlockWithFormulaCache.Name, message), buildingBlockWithFormulaCache);
         }
      }
   }
}