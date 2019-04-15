using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Descriptors;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Extensions
{
   public static class EnumerableExtensions
   {
      public static IEnumerable<TIn> DistinctBy<TIn, TCompare>(this IEnumerable<TIn> list, Func<TIn, TCompare> func)
      {
         return list.GroupBy(func).Select(grouping => grouping.First());
      }

      /// <summary>
      ///    Returns true if the <paramref name="enumeration" /> contains ALL items defined in <paramref name="itemsToCheck" />
      ///    otherwise false.
      /// </summary>
      public static bool ContainsAll<T>(this IEnumerable<T> enumeration, IEnumerable<T> itemsToCheck)
      {
         return itemsToCheck.Aggregate(true, (current, item) => current && enumeration.ContainsItem(item));
      }

      /// <summary>
      ///    Returns true if the <paramref name="enumeration" /> contains AT LEAST ONE item defined in
      ///    <paramref name="itemsToCheck" /> otherwise false.
      /// </summary>
      public static bool ContainsAny<T>(this IEnumerable<T> enumeration, IEnumerable<T> itemsToCheck)
      {
         return itemsToCheck.Any(enumeration.Contains);
      }

      /// <summary>
      ///    Returns all distinct values using the projection <paramref name="valueRetriever" /> given as parameter
      /// </summary>
      public static IReadOnlyList<string> AllDistinctValues<T>(this IEnumerable<T> enumerable, Func<T, string> valueRetriever)
      {
         return (from parameter in enumerable
            select valueRetriever(parameter)).Distinct().ToList();
      }

      public static T MinimumBy<T, TKey>(this IEnumerable<T> theEnumerable, Func<T, TKey> func)
      {
         return theEnumerable.OrderBy(func).First();
      }

      public static EntityDescriptorMapList<T> ToEntityDescriptorMapList<T>(this IEnumerable<T> entities) where T : class, IEntity
      {
         return new EntityDescriptorMapList<T>(entities.Select(x => new EntityDescriptorMap<T>(x)));
      }
   }
}