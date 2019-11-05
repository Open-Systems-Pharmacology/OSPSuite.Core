using System.Collections.Generic;
using System.Linq;

namespace OSPSuite.Core.Domain.Populations
{
   public class IndividualValuesCache : IParameterValueCache
   {
      // One entry per individual. Each Covariate item contains all the covariates defined for one individual such as gender, race, populationName 
      public List<Covariates> AllCovariates { get; }

      public ParameterValuesCache ParameterValuesCache { get; }

      public IndividualValuesCache() : this(new ParameterValuesCache(), new List<Covariates>())
      {
      }

      public IndividualValuesCache(ParameterValuesCache parameterValuesCache, IEnumerable<Covariates> allCovariates)
      {
         ParameterValuesCache = parameterValuesCache;
         AllCovariates = new List<Covariates>(allCovariates);
      }

      public void Add(IndividualValues individualValues)
      {
         AllCovariates.Add(individualValues.Covariates);
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

      public virtual IndividualValuesCache Clone()
      {
         return new IndividualValuesCache(ParameterValuesCache.Clone(), AllCovariates);
      }

      public virtual void Remove(string parameterPath) => ParameterValuesCache.Remove(parameterPath);

      public virtual void SetValues(string parameterPath, IReadOnlyList<RandomValue> newValues) => ParameterValuesCache.SetValues(parameterPath, newValues);

      public virtual void SetValues(string parameterPath, IReadOnlyList<double> newValues) => ParameterValuesCache.SetValues(parameterPath, newValues);

      public virtual IReadOnlyCollection<ParameterValues> AllParameterValues => ParameterValuesCache.AllParameterValues;

      public virtual ParameterValues ParameterValuesFor(string parameterPath) => ParameterValuesCache.ParameterValuesFor(parameterPath);

      public virtual ParameterValues ParameterValuesAt(int index) => ParameterValuesCache.ParameterValuesAt(index);

      public virtual void Add(ParameterValues parameterValues) => ParameterValuesCache.Add(parameterValues);

      public virtual void Add(IReadOnlyCollection<ParameterValue> parameterValues) => ParameterValuesCache.Add(parameterValues);

      public virtual void RenamePath(string oldPath, string newPath) => ParameterValuesCache.RenamePath(oldPath, newPath);

      public virtual void AddCovariate(string covariate, IReadOnlyList<string> values) => addCovariates(covariate, values);

      private void addCovariates<T>(string covariate, IReadOnlyList<T> values)
      {
         addCovariatesIfRequired(values.Count);
         for (int i = 0; i < values.Count; i++)
         {
            AllCovariates[i].AddCovariate(covariate, values[i]);
         }
      }

      private void addCovariatesIfRequired(int numberOfItems)
      {
         for (int i = AllCovariates.Count; i < numberOfItems; i++)
         {
            AllCovariates.Add(new Covariates());
         }
      }

      public virtual void Merge(IndividualValuesCache individualValuesCache, PathCache<IParameter> parameterCache)
      {
         AllCovariates.AddRange(individualValuesCache.AllCovariates);
         ParameterValuesCache.Merge(individualValuesCache.ParameterValuesCache, parameterCache);
      }

      public IReadOnlyList<string> AllCovariatesNames() => AllCovariates.SelectMany(x => x.Attributes.Keys).Distinct().ToArray();

      public Covariates CovariatesAt(int index) => AllCovariates[index];

      public IReadOnlyList<string> AllCovariateValuesFor(string covariateName)
      {
         return AllCovariates.Select(x => x.Covariate(covariateName)).ToArray();
      }
   }
}