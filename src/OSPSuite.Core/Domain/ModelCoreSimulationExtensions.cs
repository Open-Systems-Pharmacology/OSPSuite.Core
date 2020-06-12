using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Domain
{
   public static class ModelCoreSimulationExtensions
   {
      public static IReadOnlyList<ApplicationParameters> AllApplicationParametersOrderedByStartTimeFor(this IModelCoreSimulation simulation,
         string moleculeName)
      {
         var endTime = simulation.EndTime ?? 0;
         var allApplications = allApplicationsForMolecule(simulation, moleculeName);

         return allApplicationParametersIn(allApplications, Constants.Parameters.START_TIME)
            .Where(x => x.Value <= endTime)
            .OrderBy(x => x.Value)
            .Select(x => new ApplicationParameters(x))
            .ToArray(); // Return array here to be compatible with R
      }

      private static IReadOnlyList<IContainer> allApplicationsForMolecule(IModelCoreSimulation simulation, string moleculeName)
      {
         var applicationEventGroup = simulation.Model.Root.GetChildren<IEventGroup>().ToList();
         if (!applicationEventGroup.Any())
            return new List<IContainer>();

         var allApplications = applicationEventGroup.SelectMany(x => x.GetAllChildren<IContainer>(c => c.ContainerType == ContainerType.Application))
            .ToList();

         return getApplicationsForAppliedAncestorMolecule(simulation.Reactions, moleculeName, allApplications);
      }

      private static IReadOnlyList<IContainer> getApplicationsForAppliedAncestorMolecule(IEnumerable<IReactionBuilder> reactions, string moleculeName,
         IReadOnlyList<IContainer> allApplications)
      {
         if (reactions == null)
            return new List<IContainer>();

         var reactionsList = reactions.ToList();
         var applicationsForAppliedAncestorMolecule =
            allApplications.Where(c => c.GetSingleChildByName<IMoleculeAmount>(moleculeName) != null).ToList();

         // If there are any applications of this molecule, use them
         if (applicationsForAppliedAncestorMolecule.Any())
            return applicationsForAppliedAncestorMolecule;

         // If more than one reaction produces the molecule, count the unique educts. If there is more than one, then these reactions aren't considered
         var producingMolecule = reactionsProducingMolecule(reactionsList, moleculeName);
         var distinctEductsFromReactions = eductNamesFromReactions(producingMolecule);

         if (distinctEductsFromReactions.Count != 1)
            return new List<IContainer>();

         // Otherwise find 'parent applications' by looking recursively at reactions involving the named molecule where there is one educt and one product.
         return getApplicationsForAppliedAncestorMolecule(reactionsList, distinctEductsFromReactions[0], allApplications).ToList();
      }

      private static IReadOnlyList<string> eductNamesFromReactions(IEnumerable<IReactionBuilder> reactions)
      {
         return reactions
            .Where(reaction => reaction.Educts.Count() == 1)
            .SelectMany(reaction => reaction.Educts.Select(educt => educt.MoleculeName))
            .Distinct()
            .ToList();
      }

      private static IReadOnlyList<IReactionBuilder> reactionsProducingMolecule(IEnumerable<IReactionBuilder> reactions, string moleculeName)
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
   }
}