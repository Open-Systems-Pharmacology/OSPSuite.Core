using System.Collections.Generic;

namespace OSPSuite.Core.Domain.Populations
{
   public class IndividualValuesCache : IParameterValueCache
   {
      public ParameterValuesCache ParameterValuesCache { get; }
      public CovariateValuesCache CovariateValuesCache { get; }

      public IndividualValuesCache() : this(new ParameterValuesCache(), new CovariateValuesCache())
      {
      }

      public IndividualValuesCache(ParameterValuesCache parameterValuesCache, CovariateValuesCache covariateValuesCache)
      {
         ParameterValuesCache = parameterValuesCache;
         CovariateValuesCache = covariateValuesCache;
      }

      public void Add(IndividualValues individualValues)
      {
         CovariateValuesCache.AddIndividualValues(individualValues.Covariates);
         Add(individualValues.ParameterValues);
      }

      public virtual bool Has(string parameterPath) => ParameterValuesCache.Has(parameterPath);

      public virtual IReadOnlyList<double> ValuesFor(string parameterPath) => ParameterValuesCache.ValuesFor(parameterPath);

      public virtual IReadOnlyList<double> PercentilesFor(string parameterPath) => ParameterValuesCache.PercentilesFor(parameterPath);

      /// <summary>
      ///    Returns the number of individuals
      /// </summary>
      public virtual int Count => ParameterValuesCache.Count;

      public virtual string[] AllParameterPaths() => ParameterValuesCache.AllParameterPaths();

      public double[] GetValues(string parameterPath) => ParameterValuesCache.GetValues(parameterPath);

      public double[] GetPercentiles(string parameterPath) => ParameterValuesCache.GetPercentiles(parameterPath);

      public string[] GetCovariateValues(string covariateName) => CovariateValuesCache.GetValues(covariateName);

      public virtual IndividualValuesCache Clone()
      {
         return new IndividualValuesCache(ParameterValuesCache.Clone(), CovariateValuesCache.Clone());
      }

      public virtual void Remove(string parameterPath) => ParameterValuesCache.Remove(parameterPath);

      public virtual void SetValues(string parameterPath, IReadOnlyList<RandomValue> newValues) => ParameterValuesCache.SetValues(parameterPath, newValues);

      public virtual void SetValues(string parameterPath, IReadOnlyList<double> newValues) => ParameterValuesCache.SetValues(parameterPath, newValues);

      public virtual IReadOnlyCollection<ParameterValues> AllParameterValues => ParameterValuesCache.AllParameterValues;

      public virtual ParameterValues ParameterValuesFor(string parameterPath) => ParameterValuesCache.ParameterValuesFor(parameterPath);

      public ParameterValues GetOrAddParameterValuesFor(string parameterPath) => ParameterValuesCache.GetOrAddParameterValuesFor(parameterPath);

      public virtual ParameterValues ParameterValuesAt(int index) => ParameterValuesCache.ParameterValuesAt(index);

      public virtual void Add(ParameterValues parameterValues) => ParameterValuesCache.Add(parameterValues);

      public virtual void Add(IReadOnlyCollection<ParameterValue> parameterValues) => ParameterValuesCache.Add(parameterValues);

      public virtual void RenamePath(string oldPath, string newPath) => ParameterValuesCache.RenamePath(oldPath, newPath);

      public virtual void AddCovariate(string covariateName, IReadOnlyList<string> values) => CovariateValuesCache.Add(covariateName, values);

      public virtual void Merge(IndividualValuesCache individualValuesCache, PathCache<IParameter> parameterCache)
      {
         CovariateValuesCache.Merge(individualValuesCache.CovariateValuesCache);
         ParameterValuesCache.Merge(individualValuesCache.ParameterValuesCache, parameterCache);
      }

      /// <summary>
      ///    Returns the covariates with the given <paramref name="covariateName" /> or null if not defined
      /// </summary>
      /// <param name="covariateName"></param>
      /// <returns></returns>
      public virtual CovariateValues CovariateValuesFor(string covariateName) => CovariateValuesCache.CovariateValuesFor(covariateName);

      public string[] AllCovariatesNames() => CovariateValuesCache.AllCovariateNames();

      public IReadOnlyList<string> AllCovariateValuesFor(string covariateName) => CovariateValuesCache.ValuesFor(covariateName);

      public string[] AllCovariateValuesForIndividual(int individualId) => CovariateValuesCache.AllCovariateValuesForIndividual(individualId);

      public string CovariateValueFor(string covariateName, int individualId) => CovariateValuesCache.CovariateValueFor(covariateName, individualId);
   }
}