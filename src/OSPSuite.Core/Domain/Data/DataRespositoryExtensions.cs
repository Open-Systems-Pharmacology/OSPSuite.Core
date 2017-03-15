using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Data
{
   public static class DataRepositoryExtensions
   {
      public static IEnumerable<DataColumn> ColumnsForPath(this IEnumerable<DataColumn> dataRepository, string path)
      {
         return dataRepository.Where(col => col.DataInfo.Origin == ColumnOrigins.Calculation)
            .Where(col => col.QuantityInfo.PathAsString.Contains(path));
      }

      public static bool IsNull(this DataRepository dataRepository)
      {
         return dataRepository == null || dataRepository.IsAnImplementationOf<NullDataRepository>();
      }

      /// <summary>
      ///    Gets the intersecting metadata for an enumeration of data repositories. The intersection is all metadata with keys
      ///    contained in all repositories meta data
      /// </summary>
      /// <param name="dataRepositories">The repositories being scanned for intersecting metadata</param>
      /// <returns>An enumerable of IExtendedProperty which is a key-value pairing of metadata</returns>
      public static IEnumerable<IExtendedProperty> IntersectingMetaData(this IReadOnlyList<DataRepository> dataRepositories)
      {
         return dataRepositories.SelectMany(repository => repository.ExtendedProperties).
            Distinct(new ExtendedPropertyComparer(ep => ep.Name)).
            Where(x => dataRepositories.All(repos => repos.ExtendedProperties.Contains(x.Name))).
            Select(x => new ExtendedProperty<string>
            {
               Name = x.Name,
               Value = valueMapper(dataRepositories.SelectMany(repository => repository.ExtendedProperties)
                  .Where(ep => ep.IsNamed(x.Name)), x)
            });
      }

      private static string valueMapper(IEnumerable<IExtendedProperty> properties, IExtendedProperty value)
      {
         return valueMapper(properties, value.ValueAsObject.ToString());
      }

      private static string valueMapper(IEnumerable<IExtendedProperty> properties, string value)
      {
         return properties.All(x => x.ValueAsObject.ToString().Equals(value)) ? value : string.Empty;
      }

      private class ExtendedPropertyComparer : IEqualityComparer<IExtendedProperty>
      {
         private readonly Func<IExtendedProperty, object> _funcDistinct;

         public ExtendedPropertyComparer(Func<IExtendedProperty, object> funcDistinct)
         {
            _funcDistinct = funcDistinct;
         }

         public bool Equals(IExtendedProperty x, IExtendedProperty y)
         {
            return _funcDistinct(x).Equals(_funcDistinct(y));
         }

         public int GetHashCode(IExtendedProperty obj)
         {
            return _funcDistinct(obj).GetHashCode();
         }
      }
   }
}