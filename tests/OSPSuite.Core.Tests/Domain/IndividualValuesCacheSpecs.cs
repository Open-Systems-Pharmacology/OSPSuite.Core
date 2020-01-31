using System;
using System.Collections.Generic;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Populations;
using OSPSuite.Helpers;
using OSPSuite.Utility.Exceptions;

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
      private CovariateValuesCache _originalCovariates;
      private CovariateValuesCache _covariatesToMerge;
      private PathCache<IParameter> _parameterCache;
      private List<int> _originalIndividualIds;
      private List<int> _individualIdsToMerge;

      protected override void Context()
      {
         base.Context();
         _originalValueCache = new ParameterValuesCache();

         //3 individuals to in original pop
         _originalIndividualIds = new List<int> {1, 2, 3};

         var parameterValues1 = new ParameterValues("Path1");
         parameterValues1.Add(new double[] {2, 3, 4});

         var parameterValues2 = new ParameterValues("Path2");
         parameterValues2.Add(new double[] {4, 5, 6});

         _originalValueCache.Add(parameterValues1);
         _originalValueCache.Add(parameterValues2);

         _originalCovariates = new CovariateValuesCache();
         _originalCovariates.Add("Gender", new[] {"Male", "Female", "Female"});

         _parameterCache = new PathCacheForSpecs<IParameter>();

         _cacheToMerge = new ParameterValuesCache();
         var parameterValuesToMerge1 = new ParameterValues("Path1");
         parameterValuesToMerge1.Add(new double[] {10, 20});
         _cacheToMerge.Add(parameterValuesToMerge1);

         var parameterValuesToMerge2 = new ParameterValues("Path3");
         parameterValuesToMerge2.Add(new double[] {30, 40});
         _cacheToMerge.Add(parameterValuesToMerge2);

         _covariatesToMerge = new CovariateValuesCache();
         _covariatesToMerge.Add("Gender", new[] {"Female", "Female"});
         _covariatesToMerge.Add("Population", new[] {"European", "American"});


         _individualIdsToMerge = new List<int> {10, 20};
         sut = new IndividualValuesCache(_originalValueCache, _originalCovariates, _originalIndividualIds);
         _individualPropertiesCacheToMerge = new IndividualValuesCache(_cacheToMerge, _covariatesToMerge, _individualIdsToMerge);
      }

      protected override void Because()
      {
         sut.Merge(_individualPropertiesCacheToMerge, _parameterCache);
      }

      [Observation]
      public void should_merge_the_ids_together()
      {
         sut.IndividualIds.ShouldOnlyContainInOrder(1, 2, 3, 10, 20);
      }

      [Observation]
      public void should_add_the_covariates_from_the_properties_cache()
      {
         sut.CovariateValuesCache.AllCovariateValues.Count.ShouldBeEqualTo(2);
         sut.AllCovariateValuesFor("Gender").ShouldOnlyContainInOrder("Male", "Female", "Female", "Female", "Female");
         sut.CovariateValueFor("Gender", 1).ShouldBeEqualTo("Male");
         sut.CovariateValueFor("Gender", 2).ShouldBeEqualTo("Female");
         sut.CovariateValueFor("Gender", 3).ShouldBeEqualTo("Female");
         sut.CovariateValueFor("Gender", 10).ShouldBeEqualTo("Female");
         sut.CovariateValueFor("Gender", 20).ShouldBeEqualTo("Female");

         sut.AllCovariateValuesFor("Population").ShouldOnlyContainInOrder(Constants.UNKNOWN, Constants.UNKNOWN, Constants.UNKNOWN, "European", "American");
         sut.CovariateValueFor("Population", 1).ShouldBeEqualTo(Constants.UNKNOWN);
         sut.CovariateValueFor("Population", 2).ShouldBeEqualTo(Constants.UNKNOWN);
         sut.CovariateValueFor("Population", 3).ShouldBeEqualTo(Constants.UNKNOWN);
         sut.CovariateValueFor("Population", 10).ShouldBeEqualTo("European");
         sut.CovariateValueFor("Population", 20).ShouldBeEqualTo("American");
      }

      [Observation]
      public void should_merge_the_parameter_values()
      {
         sut.ParameterValuesCache.AllParameterValues.Count.ShouldBeEqualTo(3);
         sut.GetValues("Path1").ShouldBeEqualTo(new[] {2.0, 3, 4, 10, 20});
         sut.GetValues("Path2").ShouldBeEqualTo(new[] {4.0, 5, 6, double.NaN, double.NaN});
         sut.GetValues("Path3").ShouldBeEqualTo(new[] {double.NaN, double.NaN, double.NaN, 30, 40});
      }
   }

   public class When_retrieving_all_parameter_value_for_a_given_individual_by_id : concern_for_IndividualValuesCache
   {
      protected override void Context()
      {
         base.Context();
         //3 individuals to in original pop
         var ids = new List<int> {1, 2, 3};

         var parameterValues1 = new ParameterValues("Path1 [ml]");
         parameterValues1.Add(new double[] {2, 3, 4});

         var parameterValues2 = new ParameterValues("Path2");
         parameterValues2.Add(new double[] {4, 5, 6});

         sut.Add(parameterValues1);
         sut.Add(parameterValues2);

         sut.IndividualIds.AddRange(ids);

         sut.CovariateValuesCache.Add("Gender", new[] {"Male", "Female", "Female"});
      }

      [Observation]
      public void should_return_the_expected_values_for_a_valid_id()
      {
         var values = sut.AllParameterValuesForIndividual(2);

         values.Length.ShouldBeEqualTo(2);
         values[0].ParameterPath.ShouldBeEqualTo("Path1");
         values[0].Value.ShouldBeEqualTo(3);

         values[1].ParameterPath.ShouldBeEqualTo("Path2");
         values[1].Value.ShouldBeEqualTo(5);
      }

      [Observation]
      public void should_throw_an_exception_if_the_individual_id_is_not_found()
      {
         The.Action(() => sut.AllParameterValuesForIndividual(5)).ShouldThrowAn<OSPSuiteException>();

         The.Action(() => sut.AllParameterValuesAt(4)).ShouldThrowAn<ArgumentOutOfRangeException>();
      }
   }

   public class When_retrieving_the_individual_index_of_an_individual_id : concern_for_IndividualValuesCache
   {
      protected override void Context()
      {
         base.Context();
         //3 individuals to in original pop
         var ids = new List<int> {1, 2, 3};

         sut.IndividualIds.AddRange(ids);
      }

      [Observation]
      public void should_return_the_expected_index_if_the_individual_id_exists()
      {
         sut.IndexOfIndividual(3).ShouldBeEqualTo(2);
      }

      [Observation]
      public void should_return_a_negative_value_if_the_individual_id_is_not_found_an_no_exception_should_be_raised()
      {
         sut.IndexOfIndividual(5, throwExceptionIfNotFound: false).ShouldBeEqualTo(-1);
      }

      [Observation]
      public void should_throw_an_exception_if_the_individual_id_is_not_found_and_an_exception_should_be_raised()
      {
         The.Action(() => sut.IndexOfIndividual(5, throwExceptionIfNotFound: true)).ShouldThrowAn<OSPSuiteException>();
      }
   }
}