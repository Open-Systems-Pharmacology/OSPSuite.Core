using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain
{
   public static class ModelCoreSimulationExtensions
   {
      public static IReadOnlyList<ApplicationParameters> AllApplicationParametersOrderedByStartTimeFor(this IModelCoreSimulation simulation,
         string moleculeName)
      {
         if (string.IsNullOrEmpty(moleculeName))
            return Array.Empty<ApplicationParameters>();

         var endTime = simulation.EndTime ?? 0;
         var allApplications = allApplicationsForMolecule(simulation, moleculeName);

         return allApplicationParametersIn(allApplications, Constants.Parameters.START_TIME)
            .Where(x => x.Value <= endTime)
            .OrderBy(x => x.Value)
            .Select(x => new ApplicationParameters(x))
            .ToArray(); // Return array here to be compatible with R
      }

      public static IReadOnlyList<ApplicationParameters> AllApplicationParametersOrderedByStartTimeForQuantityPath(
         this IModelCoreSimulation simulation, string quantityPath)
      {
         var moleculeName = simulation.Model?.MoleculeNameFor(quantityPath);
         return AllApplicationParametersOrderedByStartTimeFor(simulation, moleculeName);
      }

      private static IReadOnlyList<IContainer> allApplicationsForMolecule(IModelCoreSimulation simulation, string moleculeName)
      {
         // Exclude organism containers since they will not contain applications and represent the majority of containers in a simulation
         var containers = simulation.Model.Root.GetChildren<IContainer>(x => x.ContainerType != ContainerType.Organism);
         var allApplications = containers.SelectMany(x => x.GetAllChildren<IContainer>(c => c.ContainerType == ContainerType.Application))
            .ToList();

         if (!allApplications.Any())
            return new List<IContainer>();

         var reactionCache = new ObjectBaseCache<ReactionBuilder>();
         reactionCache.AddRange(simulation.Reactions.SelectMany(x => x));
         return getApplicationsForAppliedAncestorMolecule(reactionCache, moleculeName, allApplications, new List<string>());
      }

      private static IReadOnlyList<IContainer> getApplicationsForAppliedAncestorMolecule(IEnumerable<ReactionBuilder> reactions, string moleculeName,
         IReadOnlyList<IContainer> allApplications, List<string> alreadyCalculatedMolecules)
      {
         var applicationsForAppliedAncestorMolecule =
            allApplications.Where(c => c.GetSingleChildByName<MoleculeAmount>(moleculeName) != null).ToList();

         // If there are any applications of this molecule, use them
         if (applicationsForAppliedAncestorMolecule.Any())
            return applicationsForAppliedAncestorMolecule;

         if (reactions == null)
            return new List<IContainer>();

         var reactionsList = reactions.ToList();

         // If more than one reaction produces the molecule, count the unique educts. If there is more than one, then these reactions aren't considered
         var producingMolecule = reactionsProducingMolecule(reactionsList, moleculeName);
         var distinctEductsFromReactions = eductNamesFromReactions(producingMolecule);

         if (distinctEductsFromReactions.Count != 1 || alreadyCalculatedMolecules.Contains(distinctEductsFromReactions[0]))
            return new List<IContainer>();

         alreadyCalculatedMolecules.Add(distinctEductsFromReactions[0]);
         // Otherwise find 'parent applications' by looking recursively at reactions involving the named molecule where there is one educt and one product.
         return getApplicationsForAppliedAncestorMolecule(reactionsList, distinctEductsFromReactions[0], allApplications, alreadyCalculatedMolecules).ToList();
      }

      private static IReadOnlyList<string> eductNamesFromReactions(IEnumerable<ReactionBuilder> reactions)
      {
         return reactions
            .Where(reaction => reaction.Educts.Count() == 1)
            .SelectMany(reaction => reaction.Educts.Select(educt => educt.MoleculeName))
            .Distinct()
            .ToList();
      }

      private static IReadOnlyList<ReactionBuilder> reactionsProducingMolecule(IEnumerable<ReactionBuilder> reactions, string moleculeName)
      {
         return reactions
            .Where(reaction => reaction.Products.Count() == 1 && string.Equals(reaction.Products.First().MoleculeName, moleculeName))
            .ToList();
      }

      private static IEnumerable<IParameter> allApplicationParametersIn(IReadOnlyList<IContainer> allApplicationContainers, string parameterName)
      {
         return allApplicationContainers.SelectMany(x => x.GetAllChildren<IParameter>())
            .Where(x => x.IsNamed(parameterName));
      }

      public static void AddEntitySources(this IModelCoreSimulation simulation, IEnumerable<SimulationEntitySource> entitySources)
      {
         entitySources?.Each(simulation.EntitySources.Add);
      }
   }
}