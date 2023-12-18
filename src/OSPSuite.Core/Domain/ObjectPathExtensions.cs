using System;
using System.Collections.Generic;
using OSPSuite.Core.Extensions;

namespace OSPSuite.Core.Domain
{
   public static class ObjectPathExtensions
   {
      public static TObjectPath AndAdd<TObjectPath>(this TObjectPath objectPath, string entryToAdd) where TObjectPath : ObjectPath
      {
         objectPath.Add(entryToAdd);
         return objectPath;
      }

      public static TObjectPath AndAddAtFront<TObjectPath>(this TObjectPath objectPath, string entryToAddAtFront) where TObjectPath : ObjectPath
      {
         objectPath.AddAtFront(entryToAddAtFront);
         return objectPath;
      }

      /// <summary>
      ///    Returns the entity of type <typeparamref name="T" /> with the given path relative to the
      ///    <paramref name="refEntity" />.
      ///    If the entity could not be resolved, success will be set to false and the returned value is null.
      /// </summary>
      public static T TryResolve<T>(this ObjectPath objectPath, IEntity refEntity, out bool success) where T : class
      {
         try
         {
            var result = objectPath.Resolve<T>(refEntity);
            success = result != null;
            return result;
         }
         catch (Exception)
         {
            success = false;
            return null;
         }
      }

      /// <summary>
      ///    Returns the entity of type <typeparamref name="T" /> with the given path relative to the
      ///    <paramref name="refEntity" />. If the entity could not be resolved, returns null
      /// </summary>
      public static T TryResolve<T>(this ObjectPath objectPath, IEntity refEntity) where T : class
      {
         var result = TryResolve<T>(objectPath, refEntity, out var isFound);
         return isFound ? result : null;
      }

      public static ObjectPath ToObjectPath(this string pathAsString) => ToObjectPath(pathAsString.ToPathArray());

      public static ObjectPath ToObjectPath(this IReadOnlyCollection<string> pathAsArray) => new ObjectPath(pathAsArray);
   }
}