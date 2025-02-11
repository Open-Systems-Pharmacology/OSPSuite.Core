using System;
using System.Collections.Generic;
using System.Linq;
using MPFitLib;
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

         var spatialStructures = simulationConfiguration.ModuleConfigurations.Select(x => x.Module.SpatialStructure).Where(x => x != null).ToList();
         validateNeighborhoods(spatialStructures, validationResult);

         simulationConfiguration.AllCalculationMethods.Each(cm => validateBuildingBlockWithFormulaCache(cm, validationResult));
         return validationResult;
      }

      private void validateNeighborhoods(List<SpatialStructure> spatialStructures, ValidationResult validationResult)
      {
         // tracking already invalidated neighborhoods ensures that the error message shows up only once per duplication. The user does not need
         // to see the same validation error for both (or up to n) duplicates
         var invalidNeighborhoods = new List<(SpatialStructure spatialStructure, NeighborhoodBuilder neighborhood)>();
         var allNeighborhoods = spatialStructures.SelectMany(allNeighborhoodsFrom).ToList();

         allNeighborhoods.Each(neighborhood =>
         {
            var equivalentNeighborhoods = allNeighborhoods.Except(invalidNeighborhoods).Where(x => isEquivalentButNotExtensionNeighborhood(x.neighborhood, neighborhood.neighborhood)).ToList();
            if (!equivalentNeighborhoods.Any()) 
               return;

            invalidNeighborhoods.AddRange(equivalentNeighborhoods);
            invalidNeighborhoods.Add(neighborhood);
            validationResult.AddMessage(NotificationType.Error, neighborhood.neighborhood, 
               Error.EquivalentNeighborhoodsAreDefinedFor(neighborhood.neighborhood.FirstNeighborPath, neighborhood.neighborhood.SecondNeighborPath), buildingBlock:neighborhood.spatialStructure, details: validationDetails(equivalentNeighborhoods));
         });
      }

      private static IEnumerable<(SpatialStructure spatialStructure, NeighborhoodBuilder neighborhood)> allNeighborhoodsFrom(SpatialStructure spatialStructure)
      {
         return spatialStructure.Neighborhoods.Select(neighborhood => (spatialStructure, neighborhood));
      }

      private IEnumerable<string> validationDetails(List<(SpatialStructure spatialStructure, NeighborhoodBuilder neighborhood)> equivalentNeighborhoods)
      {
         return equivalentNeighborhoods.Select(x => Error.NeighborhoodDefinition(x.neighborhood.Name, x.neighborhood.FirstNeighborPath, x.neighborhood.SecondNeighborPath, x.spatialStructure.DisplayName)).ToList();
      }

      private bool isEquivalentButNotExtensionNeighborhood(NeighborhoodBuilder neighborhood1, NeighborhoodBuilder neighborhood2)
      {
         // shared name means that one extends the other, or they are actually the same instance
         if (Equals(neighborhood1.Name, neighborhood2.Name))
            return false;

         return (Equals(neighborhood1.FirstNeighborPath, neighborhood2.FirstNeighborPath) && Equals(neighborhood1.SecondNeighborPath, neighborhood2.SecondNeighborPath)) ||
                (Equals(neighborhood1.FirstNeighborPath, neighborhood2.SecondNeighborPath) && Equals(neighborhood1.SecondNeighborPath, neighborhood2.FirstNeighborPath));
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