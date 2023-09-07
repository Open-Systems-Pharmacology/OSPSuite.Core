using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Services
{
   public interface IContainerTask
   {
      /// <summary>
      ///    Create or retrieve a sub container with the name <paramref name="subContainerName" /> and set the mode to Logical
      /// </summary>
      /// <param name="parentContainer">parent container</param>
      /// <param name="subContainerName">name of container to be retrieved (and created if necessary)</param>
      IContainer CreateOrRetrieveSubContainerByName(IContainer parentContainer, string subContainerName);

      /// <summary>
      ///    Create or retrieve a sub container with the name <paramref name="subContainerName" /> and set the mode to
      ///    <paramref
      ///       name="containerMode" />
      ///    if the container happens to be created in the function call
      /// </summary>
      /// <param name="parentContainer">parent container</param>
      /// <param name="subContainerName">name of container to be retrieved (and created if necessary)</param>
      /// <param name="containerMode">Mode of created container</param>
      IContainer CreateOrRetrieveSubContainerByName(IContainer parentContainer, string subContainerName, ContainerMode containerMode);

      /// <summary>
      ///    Removes the container from the Spatial Structure, also all Neighborhoods connecting to the container.
      /// </summary>
      /// <remarks> No unregister at a repository is performed</remarks>
      /// <param name="spatialStructure">The spatial structure.</param>
      /// <param name="containerToRemove">The container to remove.</param>
      void RemoveContainerFrom(SpatialStructure spatialStructure, IContainer containerToRemove);

      /// <summary>
      ///    Returns a unique child name in the parent container with the suffix baseName.
      ///    e.g  baseName_* where * is a number so that the returned value does not exist in the container
      ///    if <paramref name="canUseBaseName" /> is true, the provided base name can be used as returned value if the name does
      ///    not
      ///    exist in the container.
      /// </summary>
      string CreateUniqueName(IContainer parentContainer, string baseName, bool canUseBaseName = false);

      /// <summary>
      ///    Returns a unique child name with the suffix baseName.
      ///    e.g baseName_* where * is a number so that the returned value does not exist in <paramref name="usedNames" />.
      ///    if <paramref name="canUseBaseName" /> is true, the provided base name can be used as returned value if the name does
      ///    not
      ///    exist in <paramref name="usedNames" />.
      /// </summary>
      string CreateUniqueName(IReadOnlyList<string> usedNames, string baseName, bool canUseBaseName = false);

      /// <summary>
      ///    Returns a unique child name with the suffix baseName.
      ///    e.g baseName_* where * is a number so that the returned value does not exist in <paramref name="usedNames" />.
      ///    if <paramref name="canUseBaseName" /> is true, the provided base name can be used as returned value if the name does
      ///    not
      ///    exist in <paramref name="usedNames" />.
      /// </summary>
      string CreateUniqueName(IEnumerable<IWithName> usedNames, string baseName, bool canUseBaseName = false);

      /// <summary>
      ///    Returns a cache of all children satisfying <paramref name="predicate" /> by path defined in the
      ///    <paramref name="parentContainer" />
      /// </summary>
      PathCache<TChildren> CacheAllChildrenSatisfying<TChildren>(IContainer parentContainer, Func<TChildren, bool> predicate) where TChildren : class, IEntity;

      /// <summary>
      ///    Returns a cache of all children by path defined in the <paramref name="parentContainer" />
      /// </summary>
      PathCache<TChildren> CacheAllChildren<TChildren>(IContainer parentContainer) where TChildren : class, IEntity;

      /// <summary>
      ///    Returns a cache of all elements in the <paramref name="enumerable" />
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <param name="enumerable"></param>
      /// <returns></returns>
      PathCache<T> PathCacheFor<T>(IEnumerable<T> enumerable) where T : class, IEntity;
   }

   public class ContainerTask : IContainerTask
   {
      private readonly IEntityPathResolver _entityPathResolver;
      private readonly IObjectPathFactory _objectPathFactory;
      private readonly IObjectBaseFactory _objectBaseFactory;

      //format used to generate the unique name
      private const string _uniqueNameSeparator = " ";

      public ContainerTask(IObjectBaseFactory objectBaseFactory, IEntityPathResolver entityPathResolver, IObjectPathFactory objectPathFactory)
      {
         _objectBaseFactory = objectBaseFactory;
         _entityPathResolver = entityPathResolver;
         _objectPathFactory = objectPathFactory;
      }

      public IContainer CreateOrRetrieveSubContainerByName(IContainer parentContainer, string subContainerName)
      {
         return CreateOrRetrieveSubContainerByName(parentContainer, subContainerName, ContainerMode.Logical);
      }

      public IContainer CreateOrRetrieveSubContainerByName(IContainer parentContainer, string subContainerName, ContainerMode containerMode)
      {
         if (!parentContainer.ContainsName(subContainerName))
            parentContainer.Add(_objectBaseFactory.Create<IContainer>()
               .WithName(subContainerName)
               .WithMode(containerMode));

         return parentContainer.GetSingleChildByName<IContainer>(subContainerName);
      }

      public void RemoveContainerFrom(SpatialStructure spatialStructure, IContainer containerToRemove)
      {
         var containerPath = _objectPathFactory.CreateAbsoluteObjectPath(containerToRemove);
         spatialStructure.AllNeighborhoodBuildersConnectedWith(containerPath).Each(spatialStructure.RemoveNeighborhood);
         containerToRemove.ParentContainer.RemoveChild(containerToRemove);
      }
      /// <summary>
      ///    Returns a unique child name with the suffix baseName.
      ///    e.g baseName_* where * is a number so that the returned value does not exist in <paramref name="usedNames" />.
      ///    if <paramref name="canUseBaseName" /> is true, the provided base name can be used as returned value if the name does
      ///    not  exist in <paramref name="usedNames" />.
      /// </summary>
      public static string RetrieveUniqueName(IReadOnlyList<string> usedNames, string baseName, bool canUseBaseName = false, string uniqueNameSeparator = _uniqueNameSeparator)
      {
         if (!usedNames.Contains(baseName) && canUseBaseName)
            return baseName;

         var baseFormat = $"{baseName}{uniqueNameSeparator}";

         //get all endings
         var allUsedNamesMatchingBaseFormat = usedNames.Where(n => n.StartsWith(baseFormat))
            .Select(n => n.Substring(baseFormat.Length));

         //try to convert them to an int
         return $"{baseFormat}{getNextAvailableIndexBasedOn(allUsedNamesMatchingBaseFormat)}";
      }

      public string CreateUniqueName(IEnumerable<IWithName> usedNames, string baseName, bool canUseBaseName = false) => CreateUniqueName(usedNames.Select(x => x.Name).ToList(), baseName, canUseBaseName);

      public string CreateUniqueName(IReadOnlyList<string> usedNames, string baseName, bool canUseBaseName = false) => RetrieveUniqueName(usedNames, baseName, canUseBaseName);

      private static int getNextAvailableIndexBasedOn(IEnumerable<string> allUsedNamesMatchingBaseFormat)
      {
         var allValues = new List<int>();
         foreach (var suffix in allUsedNamesMatchingBaseFormat)
            if (int.TryParse(suffix, out var value))
               allValues.Add(value);

         if (allValues.Count == 0)
            return 1;

         return allValues.Max() + 1;
      }

      public string CreateUniqueName(IContainer parentContainer, string baseName, bool canUseBaseName = false)
      {
         return CreateUniqueName(parentContainer.Children, baseName, canUseBaseName);
      }

      public PathCache<TChildren> CacheAllChildrenSatisfying<TChildren>(IContainer parentContainer, Func<TChildren, bool> predicate) where TChildren : class, IEntity
      {
         return PathCacheFor(parentContainer?.GetAllChildren(predicate));
      }

      public PathCache<TChildren> CacheAllChildren<TChildren>(IContainer parentContainer) where TChildren : class, IEntity
      {
         return CacheAllChildrenSatisfying<TChildren>(parentContainer, x => true);
      }

      public PathCache<T> PathCacheFor<T>(IEnumerable<T> enumerable) where T : class, IEntity
      {
         var pathCache = new PathCache<T>(_entityPathResolver);
         return pathCache.For(enumerable);
      }
   }
}