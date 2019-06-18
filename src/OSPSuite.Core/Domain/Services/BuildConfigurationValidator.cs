using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Services
{
   public interface IBuildConfigurationValidator
   {
      ValidationResult Validate(IBuildConfiguration buildConfiguration);
   }

   class BuildConfigurationValidator : IBuildConfigurationValidator
   {
      public ValidationResult Validate(IBuildConfiguration buildConfiguration)
      {
         var validationResult = new ValidationResult();
         validateBuildingBlockWithFormulaCache(buildConfiguration.Molecules, validationResult);
         validateBuildingBlockWithFormulaCache(buildConfiguration.Reactions, validationResult);
         validateBuildingBlockWithFormulaCache(buildConfiguration.SpatialStructure, validationResult);
         validateBuildingBlockWithFormulaCache(buildConfiguration.PassiveTransports, validationResult);
         validateBuildingBlockWithFormulaCache(buildConfiguration.Observers, validationResult);
         validateEventGroupBuildingBlock(buildConfiguration.EventGroups, buildConfiguration.Molecules, validationResult);
         validateBuildingBlockWithFormulaCache(buildConfiguration.MoleculeStartValues, validationResult);
         validateBuildingBlockWithFormulaCache(buildConfiguration.ParameterStartValues, validationResult);
         buildConfiguration.AllCalculationMethods().Each(cm => validateBuildingBlockWithFormulaCache(cm, validationResult));
         return validationResult;
      }

      private void validateEventGroupBuildingBlock(IEventGroupBuildingBlock eventGroups, IMoleculeBuildingBlock moleculeBuildingBlock, ValidationResult validationResult)
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