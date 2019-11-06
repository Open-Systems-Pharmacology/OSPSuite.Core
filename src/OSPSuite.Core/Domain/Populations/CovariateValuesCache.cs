using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Populations
{
   public class CovariateValuesCache
   {
      private readonly ICache<string, CovariateValues> _covariateValuesCache = new Cache<string, CovariateValues>(x => x.Name, x => null);

      public virtual IReadOnlyCollection<CovariateValues> AllCovariateValues => _covariateValuesCache;

      public virtual void AddIndividualValues(ICache<string, string> covariateValues)
      {
         foreach (var covariateValueKeyValue in covariateValues.KeyValues)
         {
            var covariateName = covariateValueKeyValue.Key;
            var covariateValue = covariateValueKeyValue.Value;
            var covariates = CovariateValuesFor(covariateName);
            if (covariates == null)
            {
               covariates = new CovariateValues(covariateName);
               Add(covariates);
            }

            covariates.Add(covariateValue);
         }
      }

      public virtual void Add(string covariateName, IReadOnlyList<string> values)
      {
         var covariates = new CovariateValues(covariateName, values.ToList());
         _covariateValuesCache[covariateName] = covariates;
      }

      public virtual void Add(CovariateValues covariateValues) => _covariateValuesCache.Add(covariateValues);

      public virtual int Count => !_covariateValuesCache.Any() ? 0 : _covariateValuesCache.First().Count;

      public virtual CovariateValuesCache Clone()
      {
         var clone = new CovariateValuesCache();
         _covariateValuesCache.Each(x => clone.Add(x.Clone()));
         return clone;
      }

      public virtual string[] AllCovariateNames() => _covariateValuesCache.Keys.ToArray();

      public virtual void Merge(CovariateValuesCache covariateValuesCache)
      {
         var numberOfNewItems = covariateValuesCache.Count;
         var currentCount = Count;

         foreach (var covariateName in covariateValuesCache.AllCovariateNames())
         {
            if (!Has(covariateName))
            {
               addDefaultValues(covariateName, currentCount);
            }

            CovariateValuesFor(covariateName).Merge(covariateValuesCache.CovariateValuesFor(covariateName));
         }

         //fill up the one missing
         foreach (var covariateName in AllCovariateNames())
         {
            if (!covariateValuesCache.Has(covariateName))
            {
               addDefaultValues(covariateName, numberOfNewItems);
            }
         }
      }

      private void addDefaultValues(string covariateName, int numberOfItems)
      {
         var covariateValues = GetOrAddCovariateValuesFor(covariateName);
         covariateValues.AddEmptyItems(numberOfItems);
      }

      public virtual bool Has(string covariateName) => _covariateValuesCache.Contains(covariateName);

      public virtual CovariateValues GetOrAddCovariateValuesFor(string covariateName)
      {
         if (!Has(covariateName))
            _covariateValuesCache.Add(new CovariateValues(covariateName));

         return CovariateValuesFor(covariateName);
      }

      public virtual CovariateValues CovariateValuesFor(string covariateName) => _covariateValuesCache[covariateName];

      /// <summary>
      /// Returns the values for the covariate <paramref name="covariateName"/> or null if the covariate does not exist
      /// </summary>
      public virtual IReadOnlyList<string> ValuesFor(string covariateName) => CovariateValuesFor(covariateName)?.Values;

      /// <summary>
      /// Returns the values for the covariate <paramref name="covariateName"/> or null if the covariate does not exist
      /// </summary>
      public string[] GetValues(string covariateName) => ValuesFor(covariateName)?.ToArray();

      public void Remove(string covariateName) => _covariateValuesCache.Remove(covariateName);

      public string[] AllCovariateValuesForIndividual(int individualId) => AllCovariateValues.Select(x => x.ValueAt(individualId)).ToArray();
   }
}