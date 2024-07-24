using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain
{
   public interface IObjectPathFactory
   {
      /// <summary>
      ///    Creates the AbsoluteObjectPath to the specified entity.
      /// </summary>
      /// <param name="entity">The ref entity.</param>
      FormulaUsablePath CreateAbsoluteFormulaUsablePath(IFormulaUsable entity);

      /// <summary>
      ///    Creates a object path containing the given path entries
      /// </summary>
      /// <param name="entries">entries used to create the path</param>
      FormulaUsablePath CreateFormulaUsablePathFrom(params string[] entries);

      FormulaUsablePath CreateFormulaUsablePathFrom(IReadOnlyCollection<string> entries);
      FormulaUsablePath CreateFormulaUsablePathFrom(ObjectPath objectPath);

      /// <summary>
      ///    Creates an object path representing the Time Parameter
      /// </summary>
      TimePath CreateTimePath(IDimension timeDimension);

      ObjectPath CreateAbsoluteObjectPath(IEntity entity);

      /// <summary>
      ///    Creates a ObjectPath representing the relative Position of usedObject according to usingObject
      /// </summary>
      ObjectPath CreateRelativeObjectPath(IEntity usingObject, IEntity usedObject);

      FormulaUsablePath CreateRelativeFormulaUsablePath(IEntity usingObject, IFormulaUsable usedObject);
   }

   public class ObjectPathFactory : IObjectPathFactory
   {
      private readonly IAliasCreator _aliasCreator;

      public ObjectPathFactory(IAliasCreator aliasCreator)
      {
         _aliasCreator = aliasCreator;
      }

      public FormulaUsablePath CreateAbsoluteFormulaUsablePath(IFormulaUsable entity)
      {
         var newFormulaUseablePath = new FormulaUsablePath();
         addPathEntry(entity, newFormulaUseablePath);
         newFormulaUseablePath.Alias = createAliasFrom(entity.Name);
         newFormulaUseablePath.Dimension = entity.Dimension;
         return newFormulaUseablePath;
      }

      public ObjectPath CreateAbsoluteObjectPath(IEntity entity)
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
         {
            objectPath.AddAtFront(rootContainer.Name);

            if (rootContainer.ParentPath != null)
               objectPath.AddAtFront(rootContainer.ParentPath);
         }
      }

      private string createAliasFrom(string name)
      {
         return _aliasCreator.CreateAliasFrom(name);
      }

      public FormulaUsablePath CreateFormulaUsablePathFrom(params string[] entries)
      {
         return CreateFormulaUsablePathFrom(entries.ToList());
      }

      public FormulaUsablePath CreateFormulaUsablePathFrom(IReadOnlyCollection<string> entries)
      {
         return new FormulaUsablePath(entries)
         {
            Alias = createAliasFrom(entries.Last())
         };
      }

      public FormulaUsablePath CreateFormulaUsablePathFrom(ObjectPath objectPath)
      {
         return CreateFormulaUsablePathFrom(objectPath.DowncastTo<IReadOnlyCollection<string>>());
      }

      public TimePath CreateTimePath(IDimension timeDimension)
      {
         return new TimePath { TimeDimension = timeDimension, Alias = Constants.TIME };
      }

      /// <summary>
      ///    Creates a ObjectPath representing the relative Position of usedObject according to usingObject
      /// </summary>
      public ObjectPath CreateRelativeObjectPath(IEntity usingObject, IEntity usedObject)
      {
         var objectPath = new ObjectPath();
         addRelativePathEntries(usingObject, usedObject, objectPath);
         return objectPath;
      }

      public FormulaUsablePath CreateRelativeFormulaUsablePath(IEntity usingObject, IFormulaUsable usedObject)
      {
         var newFormulaUseablePath = new FormulaUsablePath();
         addRelativePathEntries(usingObject, usedObject, newFormulaUseablePath);
         newFormulaUseablePath.Alias = createAliasFrom(usedObject.Name);
         newFormulaUseablePath.Dimension = usedObject.Dimension;
         return newFormulaUseablePath;
      }

      private void addRelativePathEntries(IEntity usingObject, IEntity usedObject, ObjectPath objectPath)
      {
         if (usedObject.Equals(usingObject))
         {
            objectPath.Add(ObjectPath.PARENT_CONTAINER);
            objectPath.Add(usedObject.Name);
         }
         else
         {
            var usingPath = new List<string>(CreateAbsoluteObjectPath(usingObject));
            var usedPath = new List<string>(CreateAbsoluteObjectPath(usedObject));

            while (usingPath.Any() && usedPath.Any() && usingPath.First().Equals(usedPath.First()))
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