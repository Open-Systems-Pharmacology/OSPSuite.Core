using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Populations
{
   public interface IParameterValueCache
   {
      bool Has(string parameterPath);
      IReadOnlyList<double> ValuesFor(string parameterPath);
      IReadOnlyList<double> PercentilesFor(string parameterPath);
      string[] AllParameterPaths();

      /// <summary>
      ///    Similar to <see cref="ValuesFor" /> but returns an array for R compatibility
      /// </summary>
      double[] GetValues(string parameterPath);

      void Remove(string parameterPath);
      void SetValues(string parameterPath, IReadOnlyList<RandomValue> newValues);
      void SetValues(string parameterPath, IReadOnlyList<double> newValues);
      IReadOnlyCollection<ParameterValues> AllParameterValues { get; }

      /// <summary>
      /// Returns the <see cref="ParameterValues"/> defined with path <paramref name="parameterPath"/> or NULL if not found
      /// </summary>
      /// <param name="parameterPath">Parameter path used to retrieve the <see cref="ParameterValues"/></param>
      ParameterValues ParameterValuesFor(string parameterPath);

      /// <summary>
      /// Returns the <see cref="ParameterValues"/> defined with path <paramref name="parameterPath"/>. It it does not exist, an empty value will be added
      /// </summary>
      /// <param name="parameterPath">Parameter path used to retrieve the <see cref="ParameterValues"/></param>
      ParameterValues GetOrAddParameterValuesFor(string parameterPath);

      ParameterValues ParameterValuesAt(int index);
      void Add(ParameterValues parameterValues);
      void Add(IReadOnlyCollection<ParameterValue> parameterValues);
      void RenamePath(string oldPath, string newPath);
      int Count { get; }
   }

   /// <summary>
   ///    Represents a cache containing the population values. Values are stored like so:
   ///    1 - One column per parameter path
   ///    2 - All values for one parameter are stored in one column. That means that one individual can be read as a row
   /// </summary>
   public class ParameterValuesCache : IParameterValueCache
   {
      private readonly ICache<string, ParameterValues> _parameterValuesCache = new Cache<string, ParameterValues>(x => x.ParameterPath);

      public virtual void Add(IReadOnlyCollection<ParameterValue> parameterValues) => addValues(parameterValues);

      public virtual void RenamePath(string oldPath, string newPath)
      {
         var possibleKey = internalKeyFor(oldPath);
         if (string.IsNullOrEmpty(possibleKey))
            return;

         var values = _parameterValuesCache[possibleKey];
         Remove(possibleKey);
         _parameterValuesCache.Add(newPath, values);
      }

      public virtual int Count => !_parameterValuesCache.Any() ? 0 : _parameterValuesCache.First().Count;

      private string internalKeyFor(string parameterPath)
      {
         if (_parameterValuesCache.Contains(parameterPath))
            return parameterPath;

         var matchingPath = Array.FindAll(AllParameterPaths(), x => x.StartsWith(parameterPath))
            .FirstOrDefault(p => string.Equals(p.StripUnit(), parameterPath));

         return matchingPath ?? string.Empty;
      }

      public virtual ParameterValues GetOrAddParameterValuesFor(string parameterPath)
      {
         var possibleKey = internalKeyFor(parameterPath);
         if (string.IsNullOrEmpty(possibleKey))
         {
            possibleKey = parameterPath;
            Add(new ParameterValues(parameterPath));
         }

         return _parameterValuesCache[possibleKey];
      }

      public virtual ParameterValues ParameterValuesFor(string parameterPath)
      {
         var possibleKey = internalKeyFor(parameterPath);
         if (string.IsNullOrEmpty(possibleKey))
            return null;

         return _parameterValuesCache[possibleKey];
      }

      public virtual ParameterValues ParameterValuesAt(int index) => _parameterValuesCache.ElementAt(index);

      public double[] GetValues(string parameterPath) => ValuesFor(parameterPath).ToArray();

      public virtual void Add(ParameterValues parameterValues) => _parameterValuesCache.Add(parameterValues);

      private void addValues(IEnumerable<ParameterValue> parameterValues)
      {
         parameterValues.Each(pv => GetOrAddParameterValuesFor(pv.ParameterPath).Add(pv));
      }

      public virtual bool Has(string parameterPath)
      {
         var possibleKey = internalKeyFor(parameterPath);
         return !string.IsNullOrEmpty(possibleKey);
      }

      public virtual IReadOnlyList<double> ValuesFor(string parameterPath) => ParameterValuesFor(parameterPath)?.Values ?? new List<double>();

      public virtual IReadOnlyList<double> PercentilesFor(string parameterPath) => ParameterValuesFor(parameterPath)?.Percentiles ?? new List<double>();

      public virtual string[] AllParameterPaths() => _parameterValuesCache.Keys.ToArray();

      public virtual ParameterValuesCache Clone()
      {
         var clone = new ParameterValuesCache();
         AllParameterValues.Each(x => clone.Add(x.Clone()));
         return clone;
      }

      public virtual void Remove(string parameterPath) => _parameterValuesCache.Remove(parameterPath);

      public virtual void SetValues(string parameterPath, IReadOnlyList<RandomValue> newValues) => setValues(parameterPath, newValues, x => x.Add);

      public virtual void SetValues(string parameterPath, IReadOnlyList<double> newValues) => setValues(parameterPath, newValues, x => x.Add);

      private void setValues<T>(string parameterPath, IReadOnlyList<T> newValues, Func<ParameterValues, Action<IReadOnlyList<T>>> addValuesFunc)
      {
         validateCount(parameterPath, newValues);
         var parameterValues = GetOrAddParameterValuesFor(parameterPath);
         parameterValues.Clear();
         addValuesFunc(parameterValues)(newValues);
      }

      private void validateCount<T>(string parameterPath, IReadOnlyList<T> values)
      {
         // nothing added yet
         if (Count == 0)
            return;

         if (Count == values.Count)
            return;

         throw new OSPSuiteException(Error.ParameterValuesDoNotHaveTheExpectedCount(parameterPath, Count, values.Count));
      }

      public virtual IReadOnlyCollection<ParameterValues> AllParameterValues => _parameterValuesCache;

      public virtual void Merge(ParameterValuesCache parameterValuesCache, PathCache<IParameter> parameterCache)
      {
         var numberOfNewItems = parameterValuesCache.Count;
         var currentCount = Count;

         foreach (var parameterPath in parameterValuesCache.AllParameterPaths())
         {
            if (!Has(parameterPath))
            {
               addDefaultValues(parameterCache, parameterPath, currentCount);
            }

            ParameterValuesFor(parameterPath).Merge(parameterValuesCache.ParameterValuesFor(parameterPath));
         }

         //fill up the one missing
         foreach (var parameterPath in AllParameterPaths())
         {
            if (!parameterValuesCache.Has(parameterPath))
            {
               addDefaultValues(parameterCache, parameterPath, numberOfNewItems);
            }
         }
      }

      private void addDefaultValues(PathCache<IParameter> parameterCache, string parameterPath, int numberOfItems)
      {
         var parameterValues = GetOrAddParameterValuesFor(parameterPath);
         var parameter = parameterCache[parameterPath];
         var defaultValue = double.NaN;
         if (parameter != null)
            defaultValue = parameter.Value;

         parameterValues.AddEmptyItems(numberOfItems, defaultValue);
      }
   }
}