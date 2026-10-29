using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Utility.Exceptions;
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
         return dataRepositories.SelectMany(repository => repository.ExtendedProperties)
            .Distinct(new ExtendedPropertyComparer(ep => ep.Name))
            .Where(x => dataRepositories.All(repos => repos.ExtendedProperties.Contains(x.Name)))
            .Select(x => new ExtendedProperty<string>
            {
               Name = x.Name,
               Value = valueMapper(dataRepositories.SelectMany(repository => repository.ExtendedProperties)
                  .Where(ep => ep.IsNamed(x.Name)), x)
            });
      }

      public static DataColumn FirstDataColumn(this DataRepository dataRepository)
      {
         return dataRepository.AllButBaseGrid().First(x => x.DataInfo.Origin != ColumnOrigins.ObservationAuxiliary);
      }

      private static string valueMapper(IEnumerable<IExtendedProperty> properties, IExtendedProperty value)
      {
         if (value.ValueAsObject == null)
            return string.Empty;
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

      /// <summary>
      ///    Returns true if <paramref name="subjectCount" /> is a whole number greater than or equal to
      ///    <see cref="Constants.ObservedData.MIN_SUBJECT_COUNT" />
      /// </summary>
      public static bool IsValidSubjectCount(string subjectCount) => tryParseSubjectCount(subjectCount, out _);

      /// <summary>
      ///    Returns the subject count stored in the meta data of the <paramref name="dataRepository" /> or <c>null</c> if the
      ///    meta data is not defined
      ///    <exception cref="OSPSuiteException">is thrown if the stored subject count is invalid</exception>
      /// </summary>
      public static int? SubjectCount(this DataRepository dataRepository)
      {
         var subjectCount = dataRepository.ExtendedPropertyValueFor(Constants.ObservedData.SUBJECT_COUNT);
         if (subjectCount == null)
            return null;

         if (!tryParseSubjectCount(subjectCount, out var value))
            throw new OSPSuiteException(Error.InvalidSubjectCount(subjectCount, Constants.ObservedData.MIN_SUBJECT_COUNT));

         return value;
      }

      /// <summary>
      ///    Stores the <paramref name="subjectCount" /> in the meta data of the <paramref name="dataRepository" />. An existing
      ///    subject count is overwritten
      ///    <exception cref="OSPSuiteException">is thrown if <paramref name="subjectCount" /> is invalid</exception>
      /// </summary>
      public static void SetSubjectCount(this DataRepository dataRepository, int subjectCount)
      {
         var subjectCountAsString = subjectCount.ToString(CultureInfo.InvariantCulture);
         if (!IsValidSubjectCount(subjectCountAsString))
            throw new OSPSuiteException(Error.InvalidSubjectCount(subjectCountAsString, Constants.ObservedData.MIN_SUBJECT_COUNT));

         dataRepository.ExtendedProperties[Constants.ObservedData.SUBJECT_COUNT] = new ExtendedProperty<string> {Name = Constants.ObservedData.SUBJECT_COUNT, Value = subjectCountAsString};
      }

      private static bool tryParseSubjectCount(string subjectCount, out int value)
      {
         return int.TryParse(subjectCount, NumberStyles.Integer, CultureInfo.InvariantCulture, out value) && value >= Constants.ObservedData.MIN_SUBJECT_COUNT;
      }

      public static void AddColumns(this DataRepository repository, IEnumerable<DataColumn> columns) => columns.Each(repository.Add);

      public static bool ColumnIsInRelatedColumns(this DataRepository repository, DataColumn column) => repository.SelectMany(x => x.RelatedColumns).Contains(column);
   }
}