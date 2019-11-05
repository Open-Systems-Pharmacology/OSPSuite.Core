using System;
using System.IO;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Populations;
using OSPSuite.Utility.Container;

namespace OSPSuite.R.Services
{
   public abstract class concern_for_PopulationTask : ContextSpecification<IPopulationTask>
   {
      public override void GlobalContext()
      {
         base.GlobalContext();
         Api.InitializeOnce(new ApiConfig
         {
            DimensionFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Constants.Files.DIMENSIONS_FILE_NAME),
            PKParametersFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Constants.Files.PK_PARAMETERS_FILE_NAME),
         });
      }

      protected override void Context()
      {
         sut = IoC.Resolve<IPopulationTask>();
      }
   }

   public class When_importing_a_population_from_file_that_matches_a_simulation_structure : concern_for_PopulationTask
   {
      private string _populationFile;
      private IndividualValuesCache _individualValuesCache;

      protected override void Context()
      {
         base.Context();
         _populationFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "Data", "pop_10.csv");
      }

      protected override void Because()
      {
         _individualValuesCache = sut.ImportPopulation(_populationFile);
      }

      [Observation]
      public void should_return_a_population_having_one_entry_per_individual_in_the_population()
      {
         _individualValuesCache.Count.ShouldBeEqualTo(10);
      }

      [Observation]
      public void should_have_loaded_the_covariates_as_expected()
      {
         _individualValuesCache.AllCovariatesNames().ShouldOnlyContain("Gender", "RaceIndex", "Population Name");
      }

      [Observation]
      public void should_be_able_to_retrieve_covariates_by_index()
      {
         var cov = _individualValuesCache.CovariateValuesFor("Gender");
         cov.ValueAt(6).ShouldBeEqualTo("2");
      }
   }
}