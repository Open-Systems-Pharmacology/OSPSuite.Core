using System;
using System.Collections.Generic;
using System.Linq;

namespace OSPSuite.Core.Domain
{
   public static class ObjectBaseExtensions
   {
      /// <summary>
      ///    Finds the first item that fulfils the predicate.
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <param name="enumerable">The enumerable to search in.</param>
      /// <param name="predicate">The predicate the item should fulfil.</param>
      /// <returns> The First item that fulfils the predicate</returns>
      public static T Find<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
      {
         return enumerable.FirstOrDefault(predicate);
      }

      /// <summary>
      ///    Finds the first item with the given.
      /// </summary>
      /// <typeparam name="T"></typeparam>
      /// <param name="enumerable">The enumerable of IWithName to find in.</param>
      /// <param name="objectName">Name of the object to find.</param>
      /// <returns> The First item with the given Name</returns>
      public static T FindByName<T>(this IEnumerable<T> enumerable, string objectName) where T : IWithName
      {
         return enumerable.Find(item => string.Equals(item.Name, objectName));
      }

      public static bool ExistsByName<T>(this IEnumerable<T> enumerable, string objectName) where T : IWithName
      {
         return enumerable.Any(item => string.Equals(item.Name, objectName));
      }

      public static T FindById<T>(this IEnumerable<T> enumerable, string objectId) where T : IWithId
      {
         return enumerable.Find(item => string.Equals(item.Id, objectId));
      }

      public static bool ExistsById<T>(this IEnumerable<T> enumerable, string objectId) where T : IWithId
      {
         return enumerable.Any(item => string.Equals(item.Id, objectId));
      }

      public static IReadOnlyList<string> AllNames<T>(this IEnumerable<T> enumerable) where T : IWithName
      {
         return enumerable.Select(item => item.Name).ToList();
      }

      public static T WithIcon<T>(this T objectBase, string iconName) where T : IObjectBase
      {
         objectBase.Icon = iconName;
         return objectBase;
      }
   }
}