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
         //TODO
         var module = simulationConfiguration.Module;
         validateBuildingBlockWithFormulaCache(module?.Molecule, validationResult);
         validateBuildingBlockWithFormulaCache(module?.Reaction, validationResult);
         validateBuildingBlockWithFormulaCache(module?.SpatialStructure, validationResult);
         validateBuildingBlockWithFormulaCache(module?.PassiveTransport, validationResult);
         validateBuildingBlockWithFormulaCache(module?.Observer, validationResult);
         validateEventGroupBuildingBlock(module?.EventGroup, module?.Molecule, validationResult);
         module?.MoleculeStartValuesCollection.Each(cm => validateBuildingBlockWithFormulaCache(cm, validationResult));
         module?.ParameterStartValuesCollection.Each(cm => validateBuildingBlockWithFormulaCache(cm, validationResult));
         simulationConfiguration.AllCalculationMethods.Each(cm => validateBuildingBlockWithFormulaCache(cm, validationResult));
         return validationResult;
      }

      private void validateEventGroupBuildingBlock(IEventGroupBuildingBlock eventGroups, MoleculeBuildingBlock moleculeBuildingBlock, ValidationResult validationResult)
      {
         var allMolecules = moleculeBuildingBlock.Select(mb => mb.Name);
         foreach (var eventGroup in eventGroups)
         {
            var applicationBuilders = eventGroup.GetAllContainersAndSelf<IApplicationBuilder>();
            foreach (var applicationBuilder in applicationBuilders.Where(applicationBuilder => !allMolecules.Contains(applicationBuilder.MoleculeName)))
            {
               validationResult.AddMessage(NotificationType.Error, applicationBuilder, Validation.ApplicatedMoleculeNotPresent(applicationBuilder.MoleculeName, applicationBuilder.Name, moleculeBuildingBlock.Name), eventGroups);
            }
         }

         validateBuildingBlockWithFormulaCache(eventGroups, validationResult);
      }

      private void validateBuildingBlockWithFormulaCache(IBuildingBlock buildingBlockWithFormulaCache, ValidationResult validationResult)
      {
         foreach (var formula in buildingBlockWithFormulaCache.FormulaCache.Where(f => f.IsExplicit()).Cast<ExplicitFormula>())
         {
            var (valid, message) = formula.IsValid();
            if (!valid)
               validationResult.AddMessage(NotificationType.Error, formula, Validation.FormulaIsNotValid(formula.Name, buildingBlockWithFormulaCache.Name, message), buildingBlockWithFormulaCache);
         }
      }
   }
}