using System;
using System.Linq;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Services
{
   internal interface IReactionCreator
   {
      bool CreateReaction(ReactionBuilder reactionBuilder, ModelConfiguration modelConfiguration);
   }

   internal class ReactionCreator : IReactionCreator
   {
      private readonly IReactionBuilderToReactionMapper _reactionMapper;
      private readonly IKeywordReplacerTask _keywordReplacerTask;
      private readonly IContainerTask _containerTask;
      private readonly IParameterBuilderCollectionToParameterCollectionMapper _parameterMapper;

      public ReactionCreator(IReactionBuilderToReactionMapper reactionMapper, IKeywordReplacerTask keywordReplacerTask,
         IContainerTask containerTask,
         IParameterBuilderCollectionToParameterCollectionMapper parameterMapper)
      {
         _reactionMapper = reactionMapper;
         _keywordReplacerTask = keywordReplacerTask;
         _containerTask = containerTask;
         _parameterMapper = parameterMapper;
      }

      public bool CreateReaction(ReactionBuilder reactionBuilder, ModelConfiguration modelConfiguration)
      {
         var (model, simulationBuilder) = modelConfiguration;
         var canCreateReaction = canCreateReactionInDef(reactionBuilder);
         var createLocalReaction = createLocalReactionDef(reactionBuilder, simulationBuilder);
         var allParentContainersForReaction = model.Root.GetAllContainersAndSelf<IContainer>().Where(canCreateReaction).ToList();
         if (!allParentContainersForReaction.Any())
            return false;

         //First create the global reaction container for this reaction
         createReactionPropertiesContainer(reactionBuilder, modelConfiguration);

         //then add the reaction to all containers where it can be created
         allParentContainersForReaction.Each(x => x.Add(createLocalReaction(x)));

         return true;
      }

      private void createReactionPropertiesContainer(ReactionBuilder reactionBuilder, ModelConfiguration modelConfiguration)
      {
         var (model, simulationBuilder, replacementContext) = modelConfiguration;
         
         var globalReactionContainer = _containerTask.CreateOrRetrieveSubContainerByName(model.Root, reactionBuilder.Name)
            .WithContainerType(ContainerType.Reaction)
            .WithIcon(reactionBuilder.Icon)
            .WithDescription(reactionBuilder.Description);

         simulationBuilder.AddBuilderReference(globalReactionContainer, reactionBuilder);

         //"Local"-Parameters will be filled in elsewhere (by the Reaction-Mapper)
         _parameterMapper.MapGlobalOrPropertyFrom(reactionBuilder, simulationBuilder).Each(globalReactionContainer.Add);

         _keywordReplacerTask.ReplaceIn(globalReactionContainer, reactionBuilder.Name, replacementContext);
      }

      private Func<IContainer, Reaction> createLocalReactionDef(ReactionBuilder reactionBuilder, SimulationBuilder simulationBuilder) => container
         => _reactionMapper.MapFromLocal(reactionBuilder, container, simulationBuilder);

      private Func<IContainer, bool> canCreateReactionInDef(ReactionBuilder reactionBuilder) => container =>
      {
         var allMoleculeNames = container.GetChildren<MoleculeAmount>().AllNames();

         return container.Mode == ContainerMode.Physical
                && allMoleculeNames.ContainsAll(reactionBuilder.Educts.Select(x => x.MoleculeName))
                && allMoleculeNames.ContainsAll(reactionBuilder.Products.Select(x => x.MoleculeName))
                && allMoleculeNames.ContainsAll(reactionBuilder.ModifierNames)
                && containerMatchesReactionCriteria(container, reactionBuilder);
      };

      private bool containerMatchesReactionCriteria(IContainer container, ReactionBuilder reactionBuilder)
      {
         //No criteria defined in the reaction => the container is a match
         return !reactionBuilder.ContainerCriteria.Any() || reactionBuilder.ContainerCriteria.IsSatisfiedBy(container);
      }
   }
}