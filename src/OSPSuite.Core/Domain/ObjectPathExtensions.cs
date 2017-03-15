using System;

namespace OSPSuite.Core.Domain
{
   public static class ObjectPathExtensions
   {
      public static TObjectPath AndAdd<TObjectPath>(this TObjectPath objectPath, string entryToAdd) where TObjectPath : IObjectPath
      {
         objectPath.Add(entryToAdd);
         return objectPath;
      }

      public static TObjectPath AndAddAtFront<TObjectPath>(this TObjectPath objectPath, string entryToAddAtFront) where TObjectPath : IObjectPath
      {
         objectPath.AddAtFront(entryToAddAtFront);
         return objectPath;
      }

      /// <summary>
      ///    Returns the entity of type <typeparamref name="T" /> with the given path relatve to the
      ///    <paramref name="refEntity" />.
      ///    If the entity could not be resolved, success will be set to false and the returned value is null.
      /// </summary>
      public static T TryResolve<T>(this IObjectPath objectPath, IEntity refEntity, out bool success) where T : class
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
      ///    Returns the entity of type <typeparamref name="T" /> with the given path relatve to the
      ///    <paramref name="refEntity" />. If the entity could not be resolved, returns null
      /// </summary>
      public static T TryResolve<T>(this IObjectPath objectPath, IEntity refEntity) where T : class
      {
         bool isFound;
         var result = TryResolve<T>(objectPath, refEntity, out isFound);
         return isFound ? result : null;
      }
   }
}