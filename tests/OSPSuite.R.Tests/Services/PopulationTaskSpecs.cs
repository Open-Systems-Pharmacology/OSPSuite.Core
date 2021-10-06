using System.Collections.Generic;
using System.Data;
using System.IO;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Populations;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.R.Services
{
   public abstract class concern_for_PopulationTask : ContextForIntegration<IPopulationTask>
   {
      protected string _populationFile;
      protected ISimulationPersister _simulationPersister;
      protected string _simulationFile;
      protected string _populationFileWithUnitInParameterName;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _populationFile = HelperForSpecs.DataFile("pop_10.csv");
         _populationFileWithUnitInParameterName = HelperForSpecs.DataFile("pop_10_parameter_with_unit.csv");
         _simulationFile = HelperForSpecs.DataFile("S1.pkml");
         _simulationPersister = Api.GetSimulationPersister();
         sut = Api.GetPopulationTask();
      }
   }

   public class When_importing_a_population_from_file_that_matches_a_simulation_structure : concern_for_PopulationTask
   {
      private IndividualValuesCache _individualValuesCache;

      public override void GlobalContext()
      {
         base.GlobalContext();
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

   public class When_exporting_a_population_to_data_table_for_calculation : concern_for_PopulationTask
   {
      private IModelCoreSimulation _simulation;
      private DataTable _dataTable;
      private IndividualValuesCache _individualValuesCache;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _simulation = _simulationPersister.LoadSimulation(_simulationFile);
         _individualValuesCache = sut.ImportPopulation(_populationFileWithUnitInParameterName);
      }

      protected override void Because()
      {
         _dataTable = sut.PopulationTableFrom(_individualValuesCache, _simulation);
      }

      [Observation]
      public void should_return_a_data_table_with_one_row_per_individual()
      {
         _dataTable.Rows.Count.ShouldBeEqualTo(10);
      }

      [Observation]
      public void should_have_removed_the_units_from_the_parameter_path()
      {
         _dataTable.Columns["Organism|Weight"].ShouldNotBeNull();
         _dataTable.Columns["Organism|Weight [kg]"].ShouldBeNull();
      }

      [Observation]
      public void should_not_remove_the_units_from_parameter_with_path_in_their_name()
      {
         _dataTable.Columns["Acidic phospholipids [mg/g] - RR"].ShouldNotBeNull();
         _dataTable.Columns["Acidic phospholipids - RR"].ShouldBeNull();
      }
   }

   public class When_splitting_a_population_file_into_multiple_files : concern_for_PopulationTask
   {
      private IReadOnlyList<string> _result;
      private string _outputFolder;
      private int _numberOfCores = 5;
      public override void GlobalContext()
      {
         base.GlobalContext();
         var tmpFile = FileHelper.GenerateTemporaryFileName();
         _outputFolder = new FileInfo(tmpFile).DirectoryName;
      }

      protected override void Because()
      {
         _result = sut.SplitPopulation(_populationFile, _numberOfCores, _outputFolder, "PopFile");
      }

      [Observation]
      public void should_create_one_file_per_requested_core()
      {
         _result.Count.ShouldBeEqualTo(_numberOfCores);
         for (int i = 0; i < _result.Count; i++)
         {
            _result.ShouldContain(Path.Combine(_outputFolder, $"PopFile_{i+1}.csv"));
         }
      }

      public override void GlobalCleanup()
      {
         base.GlobalCleanup();
         _result.Each(FileHelper.DeleteFile);
      }
   }

   public class When_splitting_a_population_file_into_multiple_files_resulting_in_a_bucket_size_leaving_empty_core: concern_for_PopulationTask
   {
      private IReadOnlyList<string> _result;
      private string _outputFolder;
      private int _numberOfCores = 4;
      public override void GlobalContext()
      {
         base.GlobalContext();
         var tmpFile = FileHelper.GenerateTemporaryFileName();
         _outputFolder = new FileInfo(tmpFile).DirectoryName;
         _populationFile = HelperForSpecs.DataFile("pop_5.csv");
      }

      protected override void Because()
      {
         _result = sut.SplitPopulation(_populationFile, _numberOfCores, _outputFolder, "PopFile");
      }

      [Observation]
      public void should_create_the_expected_number_of_files()
      {
         _result.Count.ShouldBeEqualTo(_numberOfCores - 1 );
         for (int i = 0; i < _result.Count; i++)
         {
            _result.ShouldContain(Path.Combine(_outputFolder, $"PopFile_{i + 1}.csv"));
         }
      }

      public override void GlobalCleanup()
      {
         base.GlobalCleanup();
         _result.Each(FileHelper.DeleteFile);
      }
   }

   public class When_splitting_a_population_file_into_multiple_files_and_the_number_of_individuals_is_less_than_the_number_of_cores : concern_for_PopulationTask
   {
      private IReadOnlyList<string> _result;
      private string _outputFolder;
      private int _numberOfCores = 30;
      private IndividualValuesCache _individualValuesCache;

      public override void GlobalContext()
      {
         base.GlobalContext();
         var tmpFile = FileHelper.GenerateTemporaryFileName();
         _outputFolder = new FileInfo(tmpFile).DirectoryName;
         _individualValuesCache = sut.ImportPopulation(_populationFile);
      }

      protected override void Because()
      {
         _result = sut.SplitPopulation(_populationFile, _numberOfCores, _outputFolder, "PopFile");
      }

      [Observation]
      public void should_only_create_one_file_per_individual()
      {
         _result.Count.ShouldBeEqualTo(_individualValuesCache.Count);
         for (int i = 0; i < _individualValuesCache.Count; i++)
         {
            _result.ShouldContain(Path.Combine(_outputFolder, $"PopFile_{i + 1}.csv"));
         }
      }

      public override void GlobalCleanup()
      {
         base.GlobalCleanup();
         _result.Each(FileHelper.DeleteFile);
      }
   }

 
}