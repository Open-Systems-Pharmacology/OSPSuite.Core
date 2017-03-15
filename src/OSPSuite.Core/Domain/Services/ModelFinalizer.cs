using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Services
{
   public interface IModelFinalizer
   {
      void FinalizeClone(IModel cloneModel, IModel sourceModel);
   }

   public class ModelFinalizer : IModelFinalizer
   {
      private readonly IObjectPathFactory _objectPathFactory;
      private readonly IReferencesResolver _referencesResolver;

      public ModelFinalizer(IObjectPathFactory objectPathFactory, IReferencesResolver referencesResolver)
      {
         _objectPathFactory = objectPathFactory;
         _referencesResolver = referencesResolver;
      }

      public void FinalizeClone(IModel cloneModel, IModel sourceModel)
      {
         _referencesResolver.ResolveReferencesIn(cloneModel);

         var cloneNeighborhoods = createNeighborhoodCache(cloneModel);
         var sourceNeighborhoods = createNeighborhoodCache(sourceModel);
         finalizeNeighborhoods(cloneModel, sourceNeighborhoods, cloneNeighborhoods);

         var sourceEventGroups = createEventGroupCache(sourceModel);
         var cloneEventGroups = createEventGroupCache(cloneModel);
         finalizeEventTransports(cloneModel, sourceEventGroups, cloneEventGroups);

         var sourceReactionContainerCache = createReactionContainerCache(sourceModel);
         var cloneReactionContainerCache = createReactionContainerCache(cloneModel);
         finalizeReactions(sourceReactionContainerCache, cloneReactionContainerCache);
      }

      private void finalizeReactions(ICache<IObjectPath, IContainer> sourceReactionContainerCache, ICache<IObjectPath, IContainer> cloneReactionContainerCache)
      {
         foreach (var keyValue in sourceReactionContainerCache.KeyValues)
         {
            var sourceContainer = keyValue.Value;
            var cloneContainer = cloneReactionContainerCache[keyValue.Key];
            finalizeReactionsInContainer(sourceContainer, cloneContainer);
         }
      }

      private void finalizeReactionsInContainer(IContainer sourceContainer, IContainer cloneContainer)
      {
         foreach (var sourceReaction in sourceContainer.GetChildren<IReaction>())
         {
            var cloneReaction = cloneContainer.GetSingleChildByName<IReaction>(sourceReaction.Name);
            finalizePartners(cloneContainer, sourceReaction.Products, cloneReaction.AddProduct);
            finalizePartners(cloneContainer, sourceReaction.Educts, cloneReaction.AddEduct);
         }
      }

      private void finalizePartners(IContainer cloneContainer, IEnumerable<IReactionPartner> sourcePartners, Action<IReactionPartner> addPartner)
      {
         foreach (var reactionPartner in sourcePartners)
         {
            var clonePartner = cloneContainer.GetSingleChildByName<IMoleculeAmount>(reactionPartner.Partner.Name);
            addPartner(new ReactionPartner(reactionPartner.StoichiometricCoefficient, clonePartner));
         }
      }

      private Cache<IObjectPath, IContainer> createReactionContainerCache(IModel model)
      {
         var cache = new Cache<IObjectPath, IContainer>();
         var reactionContainer = model.Root.GetAllChildren<IContainer>(x => x.Children.Any(child => child.IsAnImplementationOf<IReaction>()));
         reactionContainer.Each(cont => cache.Add(_objectPathFactory.CreateAbsoluteObjectPath(cont), cont));
         return cache;
      }

      private void finalizeEventTransports(IModel cloneModel, ICache<IObjectPath, IEventGroup> sourceEventGroups, ICache<IObjectPath, IEventGroup> cloneEventGroups)
      {
         foreach (var sourceKeyValues in sourceEventGroups.KeyValues)
         {
            var sourceEventGroup = sourceKeyValues.Value;
            var cloneEventGroup = cloneEventGroups[sourceKeyValues.Key];
            finalizeEventContainer(sourceEventGroup, cloneEventGroup, cloneModel);
         }
      }

      private void finalizeEventContainer(IContainer sourceEventGroup, IContainer cloneEventGroup, IModel cloneModel)
      {
         var sourceContainer = sourceEventGroup.GetChildren<IContainer>();
         var cloneContainer = new Cache<string, IContainer>(x => x.Name);
         cloneContainer.AddRange(cloneEventGroup.GetChildren<IContainer>());
         foreach (var source in sourceContainer)
         {
            var clone = cloneContainer[source.Name];
            finalizeEventContainer(source, clone, cloneModel);
         }

         finalizeTransportsInMoleculeParentContainer(cloneModel, sourceEventGroup, cloneEventGroup);
      }

      private Cache<IObjectPath, IEventGroup> createEventGroupCache(IModel model)
      {
         var cache = new Cache<IObjectPath, IEventGroup>();
         var eventGroups = model.Root.GetAllChildren<IEventGroup>();
         eventGroups.Each(eg => cache.Add(_objectPathFactory.CreateAbsoluteObjectPath(eg), eg));
         return cache;
      }

      private void finalizeNeighborhoods(IModel cloneModel, IEnumerable<INeighborhood> sourceNeighborhoods, ICache<string, INeighborhood> cloneNeighborhoods)
      {
         foreach (var sourceNeighborhood in sourceNeighborhoods)
         {
            var cloneNeighborhood = cloneNeighborhoods[sourceNeighborhood.Name];
            resolveNeighbors(cloneModel, sourceNeighborhood, cloneNeighborhood);
            finalizeTransportsInNeighborhood(cloneModel, sourceNeighborhood, cloneNeighborhood);
         }
      }

      private void finalizeTransportsInNeighborhood(IModel cloneModel, INeighborhood sourceNeighborhood, INeighborhood cloneNeighborhood)
      {
         finalizeTransportsInMoleculeParentContainer(cloneModel, sourceNeighborhood, cloneNeighborhood);
         foreach (var sourceMoleculeContainer in sourceNeighborhood.GetChildren<IContainer>())
         {
            var cloneMoleculeContainer = cloneNeighborhood.GetSingleChildByName<IContainer>(sourceMoleculeContainer.Name);
            finalizeTransportsInMoleculeParentContainer(cloneModel, sourceMoleculeContainer, cloneMoleculeContainer);
         }
      }

      /// <summary>
      ///    Finalizes the transports in molecule parent container. Transport are
      ///    alway created in a Molecule specific sub container, that's why we start
      ///    with them. This could be a Event Group or Neighborhood
      /// </summary>
      /// <param name="cloneModel">The clone model.</param>
      /// <param name="sourceContainer">The source container.</param>
      /// <param name="cloneContainer">The clone container.</param>
      private void finalizeTransportsInMoleculeParentContainer(IModel cloneModel, IContainer sourceContainer, IContainer cloneContainer)
      {
         foreach (var moleculeContainer in sourceContainer.GetChildren<IContainer>())
         {
            var cloneMoleculeContainer = cloneContainer.GetSingleChildByName<IContainer>(moleculeContainer.Name);

            var sourceTransports = moleculeContainer.GetChildren<ITransport>();
            var tmp = cloneMoleculeContainer.GetChildren<ITransport>();
            finalizeTranports(cloneModel, tmp, sourceTransports);
         }
      }

      private void finalizeTranports(IModel cloneModel, IEnumerable<ITransport> tmp, IEnumerable<ITransport> sourceTransports)
      {
         var cloneTransports = new Cache<string, ITransport>(x => x.Name);

         cloneTransports.AddRange(tmp);

         foreach (var transport in sourceTransports)
         {
            var cloneTransport = cloneTransports[transport.Name];
            resolveAmounts(cloneModel, transport, cloneTransport);
         }
      }

      private void resolveAmounts(IModel cloneModel, ITransport sourcerTransport, ITransport cloneTransport)
      {
         var sourceAmountPath = _objectPathFactory.CreateAbsoluteObjectPath(sourcerTransport.SourceAmount);
         var targetAmountPath = _objectPathFactory.CreateAbsoluteObjectPath(sourcerTransport.TargetAmount);

         cloneTransport.SourceAmount = sourceAmountPath.Resolve<IMoleculeAmount>(cloneModel.Root);
         cloneTransport.TargetAmount = targetAmountPath.Resolve<IMoleculeAmount>(cloneModel.Root);
      }

      private void resolveNeighbors(IModel cloneModel, INeighborhood sourceNeighborhood, INeighborhood cloneNeighborhood)
      {
         var firstNeighborPath = _objectPathFactory.CreateAbsoluteObjectPath(sourceNeighborhood.FirstNeighbor);
         var secondNeighborPath = _objectPathFactory.CreateAbsoluteObjectPath(sourceNeighborhood.SecondNeighbor);

         cloneNeighborhood.FirstNeighbor = firstNeighborPath.Resolve<IContainer>(cloneModel.Root);
         cloneNeighborhood.SecondNeighbor = secondNeighborPath.Resolve<IContainer>(cloneModel.Root);
      }

      private ICache<string, INeighborhood> createNeighborhoodCache(IModel model)
      {
         var cloneNeighborhoods = new Cache<string, INeighborhood>(nh => nh.Name);
         cloneNeighborhoods.AddRange(model.Neighborhoods.GetChildren<INeighborhood>());
         return cloneNeighborhoods;
      }
   }
}