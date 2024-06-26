﻿using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Utility.Exceptions;

namespace OSPSuite.Core.Domain.Populations
{
   public class IndividualValuesCache : IParameterValueCache
   {
      public ParameterValuesCache ParameterValuesCache { get; }
      public CovariateValuesCache CovariateValuesCache { get; }
      public List<int> IndividualIds { get; }

      public IndividualValuesCache() : this(new ParameterValuesCache(), new CovariateValuesCache(), new List<int>())
      {
      }

      public IndividualValuesCache(ParameterValuesCache parameterValuesCache, CovariateValuesCache covariateValuesCache, List<int> individualIds)
      {
         ParameterValuesCache = parameterValuesCache;
         CovariateValuesCache = covariateValuesCache;
         IndividualIds = individualIds;
      }

      public void Add(IndividualValues individualValues)
      {
         CovariateValuesCache.AddIndividualValues(individualValues.Covariates);
         Add(individualValues.ParameterValues);
         IndividualIds.Add(getNextIndividualId());
      }

      private int getNextIndividualId()
      {
         if (IndividualIds.Any())
            return IndividualIds.Max() + 1;

         return 0;
      }

      public virtual bool Has(string parameterPath) => ParameterValuesCache.Has(parameterPath);

      public virtual IReadOnlyList<double> ValuesFor(string parameterPath) => ParameterValuesCache.ValuesFor(parameterPath);

      public virtual IReadOnlyList<double> PercentilesFor(string parameterPath) => ParameterValuesCache.PercentilesFor(parameterPath);

      /// <summary>
      ///    Returns the number of individuals
      /// </summary>
      public virtual int Count => IndividualIds.Count;

      public virtual string[] AllParameterPaths() => ParameterValuesCache.AllParameterPaths();

      public double[] GetValues(string parameterPath) => ParameterValuesCache.GetValues(parameterPath);

      public double[] GetPercentiles(string parameterPath) => ParameterValuesCache.GetPercentiles(parameterPath);

      public string[] GetCovariateValues(string covariateName) => CovariateValuesCache.GetValues(covariateName);

      public virtual IndividualValuesCache Clone()
      {
         return new IndividualValuesCache(ParameterValuesCache.Clone(), CovariateValuesCache.Clone(), new List<int>(IndividualIds));
      }

      public virtual void Remove(string parameterPath) => ParameterValuesCache.Remove(parameterPath);

      public virtual void SetValues(string parameterPath, IReadOnlyList<RandomValue> newValues) => ParameterValuesCache.SetValues(parameterPath, newValues);

      public virtual void SetValues(string parameterPath, IReadOnlyList<double> newValues) => ParameterValuesCache.SetValues(parameterPath, newValues);

      public virtual IReadOnlyCollection<ParameterValues> AllParameterValues => ParameterValuesCache.AllParameterValues;

      public virtual ParameterValues ParameterValuesFor(string parameterPath) => ParameterValuesCache.ParameterValuesFor(parameterPath);

      public ParameterValues GetOrAddParameterValuesFor(string parameterPath) => ParameterValuesCache.GetOrAddParameterValuesFor(parameterPath);

      public virtual void Add(ParameterValues parameterValues) => ParameterValuesCache.Add(parameterValues);

      public virtual void Add(IReadOnlyCollection<ParameterValue> parameterValues) => ParameterValuesCache.Add(parameterValues);

      public virtual void RenamePath(string oldPath, string newPath) => ParameterValuesCache.RenamePath(oldPath, newPath);

      public ParameterValue[] AllParameterValuesAt(int indexOfIndividual) => ParameterValuesCache.AllParameterValuesAt(indexOfIndividual);

      public virtual void AddCovariate(string covariateName, IReadOnlyList<string> values) => CovariateValuesCache.Add(covariateName, values);

      public virtual void Merge(IndividualValuesCache individualValuesCache, PathCache<IParameter> parameterCache)
      {
         IndividualIds.AddRange(individualValuesCache.IndividualIds);
         CovariateValuesCache.Merge(individualValuesCache.CovariateValuesCache);
         ParameterValuesCache.Merge(individualValuesCache.ParameterValuesCache, parameterCache);
      }

      /// <summary>
      ///    Returns the covariates with the given <paramref name="covariateName" /> or null if not defined
      /// </summary>
      /// <param name="covariateName"></param>
      /// <returns></returns>
      public virtual CovariateValues CovariateValuesFor(string covariateName) => CovariateValuesCache.CovariateValuesFor(covariateName);

      public virtual int[] AllIndividualIds() => IndividualIds.ToArray();

      public virtual string[] AllCovariatesNames() => CovariateValuesCache.AllCovariateNames();

      public virtual IReadOnlyList<string> AllCovariateValuesFor(string covariateName) => CovariateValuesCache.ValuesFor(covariateName);

      public virtual string[] AllCovariateValuesForIndividual(int individualId) => CovariateValuesCache.AllCovariateValuesAt(IndexOfIndividual(individualId));

      public virtual string CovariateValueFor(string covariateName, int individualId) => CovariateValuesCache.CovariateValueFor(covariateName, IndexOfIndividual(individualId));

      public virtual ParameterValue[] AllParameterValuesForIndividual(int individualId) => AllParameterValuesAt(IndexOfIndividual(individualId));

      public virtual int IndexOfIndividual(int individualId, bool throwExceptionIfNotFound = true)
      {
         var index = IndividualIds.IndexOf(individualId);
         if (index < 0 && throwExceptionIfNotFound)
            throw new OSPSuiteException(Error.IndividualWithIdNotFound(individualId));

         return index;
      }
   }
}