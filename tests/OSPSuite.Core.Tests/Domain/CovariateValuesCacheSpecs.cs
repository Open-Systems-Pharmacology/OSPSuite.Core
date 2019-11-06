using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Populations;
using OSPSuite.Helpers;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_CovariateValuesCache : ContextSpecification<CovariateValuesCache>
   {
      protected override void Context()
      {
         sut = new CovariateValuesCache();
      }
   }

   public class When_adding_individual_covariate_values_to_a_covariate_value_cache : concern_for_CovariateValuesCache
   {

      protected override void Because()
      {
         sut.AddIndividualValues(new Cache<string, string>{{"Gender", "Male"}, {"Head", "Big"}});
         sut.AddIndividualValues(new Cache<string, string>{{"Gender", "Female"}, {"Head", "Small"}});
      }

      [Observation]
      public void should_store_the_value_for_the_given_covariate_and_be_able_to_retrieve_the_covariate_afterwards()
      {
         sut.AllCovariateNames().ShouldOnlyContainInOrder("Gender", "Head");
      }

      [Observation]
      public void should_return_true_if_asked_if_it_contains_some_values_for_the_added_covariate()
      {
         sut.Has("Gender").ShouldBeTrue();
         sut.Has("Head").ShouldBeTrue();
      }


      [Observation]
      public void should_be_able_to_retrieve_the_values_for_the_individual_path()
      {
         sut.ValuesFor("Gender").ShouldOnlyContainInOrder("Male", "Female");
      }

      [Observation]
      public void should_be_able_to_retrieve_the_values_for_the_individual_path_as_array()
      {
         sut.GetValues("Gender").ShouldOnlyContainInOrder("Male", "Female");
      }

      [Observation]
      public void should_be_able_to_retrieve_the_values_for_the_individual_path_without_unit()
      {
         sut.GetValues("Head").ShouldOnlyContainInOrder("Big", "Small"); 
      }

      [Observation]
      public void should_be_able_to_retrieve_all_values_for_a_given_individual()
      {
         sut.AllCovariateValuesForIndividual(0).ShouldOnlyContainInOrder("Male", "Big");
         sut.AllCovariateValuesForIndividual(1).ShouldOnlyContainInOrder("Female", "Small");
         sut.AllCovariateValuesForIndividual(100).ShouldOnlyContainInOrder(Constants.UNKNOWN, Constants.UNKNOWN);
      }
   }



   public class When_removing_a_covariate_by_path_from_the_covariate_cache : concern_for_CovariateValuesCache
   {
      protected override void Context()
      {
         base.Context();
         var covariate = new CovariateValues("Gender");
         sut.Add(covariate);
      }

      protected override void Because()
      {
         sut.Remove("Gender");
      }

      [Observation]
      public void should_not_contain_any_values_for_the_given_path_anymore()
      {
         sut.Has("Gender").ShouldBeFalse();
      }

      [Observation]
      public void should_have_deleted_all_rows_if_resulting_cache_as_no_parameter()
      {
         sut.AllCovariateValues.Count.ShouldBeEqualTo(0);
      }
   }

   public class When_cloning_a_covariate_values_cache : concern_for_CovariateValuesCache
   {
      private CovariateValues _covariateValues1;
      private CovariateValues _covariateValues2;
      private CovariateValuesCache _result;

      protected override void Context()
      {
         base.Context();
         _covariateValues1 = new CovariateValues("Gender", new List<string>{"Male", "Female"});
         _covariateValues2 = new CovariateValues("Race", new List<string> { "Japanese", "European" });

         sut.Add(_covariateValues1);
         sut.Add(_covariateValues2);
      }

      protected override void Because()
      {
         _result = sut.Clone();
      }

      [Observation]
      public void should_return_a_new_cache_containing_the_same_values_as_the_original_cache()
      {
         _result.Has("Gender").ShouldBeTrue();
         _result.Has("Race").ShouldBeTrue();
         _result.ValuesFor("Gender").ShouldOnlyContain("Male", "Female");
         _result.ValuesFor("Race").ShouldOnlyContain("Japanese", "European");
         _result.GetValues("Race").ShouldOnlyContain("Japanese", "European");
      }
   }



   public class When_merging_two_covariate_values_caches : concern_for_CovariateValuesCache
   {
      private CovariateValuesCache _cacheToMerge;

      protected override void Context()
      {
         base.Context();
         sut.Add("Gender", new List<string> { "Male", "Female" });
         sut.Add("Race", new List<string> { "Japanese", "European" });
      

         _cacheToMerge = new CovariateValuesCache();
         _cacheToMerge.Add("Gender", new List<string> { "Female", "Female" });
         _cacheToMerge.Add("Head", new List<string> { "Big", "Small" });
      }

      protected override void Because()
      {
         sut.Merge(_cacheToMerge);
      }

      [Observation]
      public void should_simply_expand_the_existing_values_with_the_new_ones()
      {
         sut.ValuesFor("Gender").ShouldOnlyContainInOrder("Male", "Female", "Female", "Female");
      }

      [Observation]
      public void should_fill_up_the_missing_values_for_the_existing_keys()
      {
         sut.ValuesFor("Race").ShouldOnlyContainInOrder("Japanese", "European", Constants.UNKNOWN, Constants.UNKNOWN);
      }

      [Observation]
      public void should_fill_up_the_missing_values_in_the_with_unknown_for_the_new_keys()
      {
         sut.ValuesFor("Head").ShouldOnlyContainInOrder( Constants.UNKNOWN, Constants.UNKNOWN, "Big", "Small");
      }
   }

}