using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Domain
{
   public interface IObjectPathFactory
   {
      /// <summary>
      /// Creates the AbsoluteObjectPath to the specified entity.
      /// </summary>
      /// <param name="entity">The ref entity.</param>
      IFormulaUsablePath CreateAbsoluteFormulaUsablePath(IFormulaUsable entity);

      /// <summary>
      /// Creates a object path containing the given path entries
      /// </summary>
      /// <param name="entries">entries used to create the path</param>
      IFormulaUsablePath CreateFormulaUsablePathFrom(params string[] entries);

      IFormulaUsablePath CreateFormulaUsablePathFrom(IEnumerable<string> entries);

      /// <summary>
      /// Creates an object path representing the Time Parameter
      /// </summary>
      TimePath CreateTimePath(IDimension timeDimension);

      IObjectPath CreateObjectPathFrom(params string[] entries);
      IObjectPath CreateObjectPathFrom(IEnumerable<string> entries);

      IObjectPath CreateAbsoluteObjectPath(IEntity entity);

      /// <summary>
      /// Creates a ObjectPath representing the relative Position of usedObject according to usingObject
      /// </summary>
      IObjectPath CreateRelativeObjectPath(IEntity usingObject, IEntity usedObject);

      IFormulaUsablePath CreateRelativeFormulaUsablePath(IEntity usingObject, IFormulaUsable usedObject);
   }

   public class ObjectPathFactory : IObjectPathFactory
   {
      private readonly IAliasCreator _aliasCreator;

      public ObjectPathFactory(IAliasCreator aliasCreator)
      {
         _aliasCreator = aliasCreator;
      }

      public IFormulaUsablePath CreateAbsoluteFormulaUsablePath(IFormulaUsable entity)
      {
         var newFormulaUseablePath = new FormulaUsablePath();
         addPathEntry(entity, newFormulaUseablePath);
         newFormulaUseablePath.Alias = createAliasFrom(entity.Name);
         newFormulaUseablePath.Dimension = entity.Dimension;
         return newFormulaUseablePath;
      }

      public IObjectPath CreateAbsoluteObjectPath(IEntity entity)
      {
         var newObjectPath = new ObjectPath();
         addPathEntry(entity, newObjectPath);
         return newObjectPath;
      }

      private void addPathEntry(IEntity entity, ObjectPath objectPath)
      {
         var rootContainer = entity.RootContainer;
         var currentEntity = entity;

         //add path entry until we reach the root container or the top container
         while (currentEntity != rootContainer)
         {
            objectPath.AddAtFront(currentEntity.Name);
            currentEntity = currentEntity.ParentContainer;
         }

         //Add the root container unless our entity is already the root container
         if (rootContainer != null)
            objectPath.AddAtFront(rootContainer.Name);
      }

      private string createAliasFrom(string name)
      {
         return _aliasCreator.CreateAliasFrom(name);
      }

      public IFormulaUsablePath CreateFormulaUsablePathFrom(params string[] entries)
      {
         return CreateFormulaUsablePathFrom(entries.AsEnumerable());
      }

      public IFormulaUsablePath CreateFormulaUsablePathFrom(IEnumerable<string> entries)
      {
         var newFormulaUseablePath = new FormulaUsablePath();
         entries.Each(newFormulaUseablePath.Add);
         newFormulaUseablePath.Alias = createAliasFrom(entries.Last());
         return newFormulaUseablePath;
      }

      public IObjectPath CreateObjectPathFrom(params string[] entries)
      {
         return CreateObjectPathFrom(entries.AsEnumerable());
      }

      public IObjectPath CreateObjectPathFrom(IEnumerable<string> entries)
      {
         var newObjectPath = new ObjectPath();
         entries.Each(newObjectPath.Add);
         return newObjectPath;
      }

      public TimePath CreateTimePath(IDimension timeDimension)
      {
         return new TimePath { TimeDimension = timeDimension, Alias = Constants.TIME };
      }

      /// <summary>
      /// Creates a ObjectPath representing the relative Position of usedObject according to usingObject
      /// </summary>
      public IObjectPath CreateRelativeObjectPath(IEntity usingObject, IEntity usedObject)
      {
         var objectPath = new ObjectPath();
         addRelativePathEntries(usingObject, usedObject, objectPath);
         return objectPath;
      }

      public IFormulaUsablePath CreateRelativeFormulaUsablePath(IEntity usingObject, IFormulaUsable usedObject)
      {
         var newFormulaUseablePath = new FormulaUsablePath();
         addRelativePathEntries(usingObject, usedObject, newFormulaUseablePath);
         newFormulaUseablePath.Alias = createAliasFrom(usedObject.Name);
         newFormulaUseablePath.Dimension = usedObject.Dimension;
         return newFormulaUseablePath;
      }

      private void addRelativePathEntries(IEntity usingObject, IEntity usedObject, ObjectPath objectPath)
      {
         if(usedObject.Equals(usingObject))
         {
            objectPath.Add(ObjectPath.PARENT_CONTAINER);
            objectPath.Add(usedObject.Name);
         }
         else
         {
            var usingPath = new List<string>(CreateAbsoluteObjectPath(usingObject));
            var usedPath = new List<string>(CreateAbsoluteObjectPath(usedObject));

            while (usingPath.Count() > 0 && usedPath.Count() > 0 && usingPath.First().Equals(usedPath.First()))
            {
               usingPath.RemoveAt(0);
               usedPath.RemoveAt(0);
            }
            foreach (var pathSegment in usingPath)
            {
               objectPath.Add(ObjectPath.PARENT_CONTAINER);
            }
            foreach (var pathSegment in usedPath)
            {
               objectPath.Add(pathSegment);
            }
         }
      }
   }
}