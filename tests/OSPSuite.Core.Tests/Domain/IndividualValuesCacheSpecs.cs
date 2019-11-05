using System.Collections.Generic;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Populations;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_IndividualValuesCache : ContextSpecification<IndividualValuesCache>
   {
      protected override void Context()
      {
         sut = new IndividualValuesCache();
      }
   }

   public class When_adding_an_individual_properties_to_the_cache : concern_for_IndividualValuesCache
   {
      private IndividualValues _individualValues;

      protected override void Context()
      {
         base.Context();
         _individualValues = new IndividualValues();
         _individualValues.AddParameterValue(new ParameterValue("PATH1", 5, 0.5));
         _individualValues.AddParameterValue(new ParameterValue("PATH2", 10, 0.8));
      }

      protected override void Because()
      {
         sut.Add(_individualValues);
      }

      [Observation]
      public void should_be_able_to_retrieve_the_parameter_values()
      {
         _individualValues.ParameterValue("PATH1").Value.ShouldBeEqualTo(5);
         _individualValues.ParameterValue("PATH2").Value.ShouldBeEqualTo(10);
      }
   }

   public class When_merging_an_individual_properties_cache_with_another_one : concern_for_IndividualValuesCache
   {
      private IndividualValuesCache _individualPropertiesCacheToMerge;
      private ParameterValuesCache _originalValueCache;
      private ParameterValuesCache _cacheToMerge;
      private List<Covariates> _originalCovariates;
      private List<Covariates> _covariatesToMerge;
      private Covariates _cov1;
      private Covariates _cov2;
      private Covariates _cov3;
      private Covariates _cov4;
      private PathCache<IParameter> _parameterCache;

      protected override void Context()
      {
         base.Context();
         _originalValueCache = A.Fake<ParameterValuesCache>();
         _parameterCache = A.Fake<PathCache<IParameter>>();
         _cov1 = new Covariates();
         _cov2 = new Covariates();
         _cov3 = new Covariates();
         _cov4 = new Covariates();
         _cacheToMerge = A.Fake<ParameterValuesCache>();
         _originalCovariates = new List<Covariates> {_cov1, _cov2};
         _covariatesToMerge = new List<Covariates> {_cov3, _cov4};
         sut = new IndividualValuesCache(_originalValueCache, _originalCovariates);
         _individualPropertiesCacheToMerge = new IndividualValuesCache(_cacheToMerge, _covariatesToMerge);
      }

      protected override void Because()
      {
         sut.Merge(_individualPropertiesCacheToMerge, _parameterCache);
      }

      [Observation]
      public void should_add_the_covariates_from_the_properties_cache()
      {
         sut.AllCovariates.ShouldOnlyContain(_cov1, _cov2, _cov3, _cov4);
      }

      [Observation]
      public void should_merge_the_parameter_values()
      {
         A.CallTo(() => _originalValueCache.Merge(_cacheToMerge, _parameterCache)).MustHaveHappened();
      }
   }
}