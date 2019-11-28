using System;
using System.Data;
using System.IO;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Populations;
using OSPSuite.Utility.Container;

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
         _populationFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "Data", "pop_10.csv");
         _populationFileWithUnitInParameterName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "Data", "pop_10_parameter_with_unit.csv");
         _simulationFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "Data", "S1.pkml");
         _simulationPersister = IoC.Resolve<ISimulationPersister>();
         sut = IoC.Resolve<IPopulationTask>();
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
}