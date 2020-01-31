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

      /// <summary>
      ///    Returns the values for the parameter with path <paramref name="parameterPath" /> or null if the no parameter with
      ///    path <paramref name="parameterPath" /> was found
      /// </summary>
      IReadOnlyList<double> ValuesFor(string parameterPath);

      /// <summary>
      ///    Returns the percentiles for the parameter with path <paramref name="parameterPath" /> or null if the no parameter
      ///    with path <paramref name="parameterPath" /> was found
      /// </summary>
      IReadOnlyList<double> PercentilesFor(string parameterPath);

      string[] AllParameterPaths();

      /// <summary>
      ///    Returns the values for the parameter with path <paramref name="parameterPath" /> or null if the no parameter with
      ///    path <paramref name="parameterPath" /> was found
      /// </summary>
      double[] GetValues(string parameterPath);

      /// <summary>
      ///    Returns the percentiles for the parameter with path <paramref name="parameterPath" /> or null if the no parameter
      ///    with path <paramref name="parameterPath" /> was found
      /// </summary>
      double[] GetPercentiles(string parameterPath);

      void Remove(string parameterPath);
      void SetValues(string parameterPath, IReadOnlyList<RandomValue> newValues);
      void SetValues(string parameterPath, IReadOnlyList<double> newValues);
      IReadOnlyCollection<ParameterValues> AllParameterValues { get; }

      /// <summary>
      ///    Returns the <see cref="ParameterValues" /> defined with path <paramref name="parameterPath" /> or NULL if not found
      /// </summary>
      /// <param name="parameterPath">Parameter path used to retrieve the <see cref="ParameterValues" /></param>
      ParameterValues ParameterValuesFor(string parameterPath);

      /// <summary>
      ///    Returns the <see cref="ParameterValues" /> defined with path <paramref name="parameterPath" />. It it does not
      ///    exist, an empty value will be added
      /// </summary>
      /// <param name="parameterPath">Parameter path used to retrieve the <see cref="ParameterValues" /></param>
      ParameterValues GetOrAddParameterValuesFor(string parameterPath);

      void Add(ParameterValues parameterValues);
      void Add(IReadOnlyCollection<ParameterValue> parameterValues);
      void RenamePath(string oldPath, string newPath);

      /// <summary>
      ///    Returns an array containing all the parameter values for the individual row at index
      ///    <paramref name="indexOfIndividual" />
      /// </summary>
      ParameterValue[] AllParameterValuesAt(int indexOfIndividual);
   }

   /// <summary>
   ///    Represents a cache containing the population values. Values are stored like so:
   ///    1 - One column per parameter path
   ///    2 - All values for one parameter are stored in one column. That means that one individual can be read as a row
   /// </summary>
   public class ParameterValuesCache : IParameterValueCache
   {
      private readonly Cache<string, ParameterValues> _parameterValuesCache = new Cache<string, ParameterValues>(x => x.ParameterPath);

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

      private int numberOfValuesPerPath => !_parameterValuesCache.Any() ? 0 : _parameterValuesCache.First().Count;

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

      public virtual IReadOnlyList<double> ValuesFor(string parameterPath) => ParameterValuesFor(parameterPath)?.Values;

      public double[] GetValues(string parameterPath) => ValuesFor(parameterPath)?.ToArray();

      /// <summary>
      ///    Returns the percentiles for the parameter with path <paramref name="parameterPath" /> or null if the no parameter
      ///    with path <paramref name="parameterPath" /> was found
      /// </summary>
      public virtual IReadOnlyList<double> PercentilesFor(string parameterPath) => ParameterValuesFor(parameterPath)?.Percentiles;

      /// <summary>
      ///    Returns the percentiles for the parameter with path <paramref name="parameterPath" /> or null if the no parameter
      ///    with path <paramref name="parameterPath" /> was found
      /// </summary>
      public double[] GetPercentiles(string parameterPath) => PercentilesFor(parameterPath)?.ToArray();

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
         if (numberOfValuesPerPath == 0)
            return;

         if (numberOfValuesPerPath == values.Count)
            return;

         throw new OSPSuiteException(Error.ParameterValuesDoNotHaveTheExpectedCount(parameterPath, numberOfValuesPerPath, values.Count));
      }

      public virtual IReadOnlyCollection<ParameterValues> AllParameterValues => _parameterValuesCache;

      public virtual void Merge(ParameterValuesCache parameterValuesCache, PathCache<IParameter> parameterCache)
      {
         var numberOfNewItems = parameterValuesCache.numberOfValuesPerPath;
         var currentCount = numberOfValuesPerPath;

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

      public ParameterValue[] AllParameterValuesAt(int indexOfIndividual)
      {
         if (indexOfIndividual <= 0 || indexOfIndividual >= numberOfValuesPerPath)
            throw new ArgumentOutOfRangeException(nameof(indexOfIndividual));

         //FOR Now: Strip units so that we have path without units. This should be changed

         return _parameterValuesCache.KeyValues.Select(kv =>
               new ParameterValue(kv.Key.StripUnit(), kv.Value.Values[indexOfIndividual], kv.Value.Percentiles[indexOfIndividual]))
            .ToArray();
      }
   }
}