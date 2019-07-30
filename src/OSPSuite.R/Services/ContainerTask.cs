using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Utility.Extensions;
using static OSPSuite.Core.Domain.Constants;

namespace OSPSuite.R.Services
{
   public interface IContainerTask
   {
      IParameter[] AllParametersMatching(IModelCoreSimulation simulation, params string[] path);
      IContainer[] AllContainersMatching(IModelCoreSimulation simulation, params string[] path);

      IParameter[] AllParametersMatching(IContainer container, params string[] path);
      IContainer[] AllContainersMatching(IContainer container, params string[] path);
   }

   public class ContainerTask : IContainerTask
   {
      private readonly IEntityPathResolver _entityPathResolver;
      private static readonly string ALL_BUT_PATH_DELIMITER = $"[^{ObjectPath.PATH_DELIMITER}]*";
      private static readonly string PATH_DELIMITER = $"\\{ObjectPath.PATH_DELIMITER}";
      private static readonly string OPTIONAL_PATH_DELIMITER = $"(\\{PATH_DELIMITER})?";


      public ContainerTask(IEntityPathResolver entityPathResolver)
      {
         _entityPathResolver = entityPathResolver;
      }

      public IParameter[] AllParametersMatching(IModelCoreSimulation simulation, params string[] path) =>
         AllParametersMatching(simulation?.Model?.Root, path);

      public IContainer[] AllContainersMatching(IModelCoreSimulation simulation, params string[] path) =>
         AllContainersMatching(simulation?.Model?.Root, path);

      public IContainer[] AllContainersMatching(IContainer container, params string[] path) =>
         // Distributed parameters are also containers but should not be returned from the following method
         allEntitiesMatching<IContainer>(container, path).Where(c => !c.IsAnImplementationOf<DistributedParameter>()).ToArray();

      public IParameter[] AllParametersMatching(IContainer container, params string[] path) =>
         allEntitiesMatching<IParameter>(container, path);

      private T[] allEntitiesMatching<T>(IContainer container, string[] path) where T : class, IEntity
      {
         if (path == null || path.Length == 0)
            return Array.Empty<T>();

         validate(path);
         var pathAsString = path.ToPathString();

         // no wild cards => it's a single path and do not need to inspect 
         if (!pathAsString.Contains(WILD_CARD))
         {
            var entity = container.EntityAt<T>(path);
            return entity == null ? Array.Empty<T>() : new[] {entity};
         }

         var regex = new Regex(createSearchPattern(path), RegexOptions.IgnoreCase);
         var parentContainerPath = $"{_entityPathResolver.FullPathFor(container)}{ObjectPath.PATH_DELIMITER}";

         return container.GetAllChildren<T>(x => pathMatches(regex, parentContainerPath, x)).ToArray();
      }

      private void validate(string[] path)
      {
         var invalidEntries =  path.Where(x => !string.Equals(x, WILD_CARD_RECURSIVE)).Where(x => x.Contains(WILD_CARD_RECURSIVE)).ToList();
         if (!invalidEntries.Any())
            return;

         var correctedEntries = invalidEntries.Select(x => x.Replace(WILD_CARD_RECURSIVE, WILD_CARD)).ToList();
         throw new OSPSuiteException(Error.WildCardRecursiveCannotBePartOfPath(WILD_CARD_RECURSIVE,  invalidEntries, correctedEntries));
      }

      private string createSearchPattern(string[] path)
      {
         var pattern = new List<string>();
         foreach (var entry in path)
         {
            if (string.Equals(entry, WILD_CARD)) { 
               // At least one occurence of a path entry => anything except ObjectPath.PATH_DELIMITER, repeated once
               pattern.Add($"{ALL_BUT_PATH_DELIMITER}?");
               pattern.Add(PATH_DELIMITER);
            }
            else if (string.Equals(entry, WILD_CARD_RECURSIVE))
            {
               pattern.Add(".*"); //Match anything
               pattern.Add(OPTIONAL_PATH_DELIMITER);
            }
            else
            {
               pattern.Add(entry.Replace(WILD_CARD, ALL_BUT_PATH_DELIMITER));
               pattern.Add(PATH_DELIMITER);
            }
         }

         pattern.RemoveAt(pattern.Count-1);
         var searchPattern = pattern.ToString("");
         return $"^{searchPattern}$";
      }

      private bool pathMatches(Regex regex, string parentContainerPath, IEntity entity)
      {
         //Ensure that we remove the common path part between the parent container and the entity
         var entityPath = _entityPathResolver.FullPathFor(entity).Replace(parentContainerPath, "");
         return regex.IsMatch(entityPath);
      }
   }
}