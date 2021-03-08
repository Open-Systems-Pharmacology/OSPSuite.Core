using System.Data;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.PKAnalyses;
using OSPSuite.Core.Domain.Populations;
using OSPSuite.R.Domain;

namespace OSPSuite.R.Services
{
   public abstract class concern_for_PKAnalysisTask : ContextForIntegration<IPKAnalysisTask>
   {
      protected string _pkParameterFile;
      protected Simulation _simulation;
      protected string _outputPath;
      protected ISimulationRunner _simulationRunner;
      protected IPKParameterTask _pkParameterTask;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _outputPath = "Organism|PeripheralVenousBlood|Caffeine|Plasma (Peripheral Venous Blood)";

         _pkParameterFile = HelperForSpecs.DataFile("20 Values for peripheral venous blood.csv");
         var simulationFile = HelperForSpecs.DataFile("S1.pkml");
         var simulationPersister = Api.GetSimulationPersister();
         _simulation = simulationPersister.LoadSimulation(simulationFile);
         _simulationRunner = Api.GetSimulationRunner();
         _pkParameterTask = Api.GetPKParameterTask();
         sut = Api.GetPKAnalysisTask();
      }
   }

   public class When_importing_a_valid_pk_parameter_files : concern_for_PKAnalysisTask
   {
      private PopulationSimulationPKAnalyses _result;

      protected override void Because()
      {
         _result = sut.ImportPKAnalysesFromCSV(_pkParameterFile, _simulation);
      }

      [Observation]
      public void should_return_a_pk_analysis_object_with_the_expected_data()
      {
         _result.All().Count().ShouldBeEqualTo(2);
         _result.HasPKParameterFor(_outputPath, "My PK-Parameter").ShouldBeTrue();
         _result.HasPKParameterFor(_outputPath, "C_max").ShouldBeTrue();
      }

      [Observation]
      public void should_have_converted_display_dimension_into_core_dimension()
      {
         var c_max = _result.PKParameterFor(_outputPath, "C_max");
         c_max.Dimension.Name.ShouldBeEqualTo(Constants.Dimension.MOLAR_CONCENTRATION);
      }
   }

   public class When_exporting_the_pk_analysis_to_data_frame_with_value_in_another_unit : concern_for_PKAnalysisTask
   {
      private SimulationResults _result;
      private PopulationSimulationPKAnalyses _pkAnalysis;
      private UserDefinedPKParameter _userDefinedPKParameter;
      private DataTable _table;

      protected override void Context()
      {
         base.Context();
         _result = _simulationRunner.Run(_simulation);
         _userDefinedPKParameter =
            _pkParameterTask.CreateUserDefinedPKParameter("MyCmax", StandardPKParameter.C_max, displayName: null, displayUnit: "mg/l");
         _pkParameterTask.AddUserDefinedPKParameter(_userDefinedPKParameter);
      }

      protected override void Because()
      {
         _pkAnalysis = sut.CalculateFor(new CalculatePKAnalysisArgs {Simulation = _simulation, SimulationResults = _result});
         _table = sut.ConvertToDataTable(_pkAnalysis, _simulation);
      }

      [Observation]
      public void should_return_a_pk_analysis_object_with_the_expected_data()
      {
         _table.Columns.Count.ShouldBeEqualTo(5);
         var mw = _simulation.MolWeightFor(_outputPath).GetValueOrDefault(double.NaN);
         var c_max = _table.Select($"QuantityPath = '\"{_outputPath}\"' AND Parameter = '\"C_max\"'");
         var my_cmax = _table.Select($"QuantityPath = '\"{_outputPath}\"' AND Parameter = '\"MyCmax\"'");
         var c_max_value = double.Parse(c_max[0]["Value"].ToString());
         var my_cmax_value = double.Parse(my_cmax[0]["Value"].ToString());
         //kg/l => mg/l
         my_cmax_value.ShouldBeEqualTo(c_max_value * mw * 1E6, 1e-2);
      }

      public override void Cleanup()
      {
         base.Cleanup();
         _pkParameterTask.RemoveAllUserDefinedPKParameters();
      }
   }

   public class When_calculating_the_pk_analysis_for_multiple_dosing_application : concern_for_PKAnalysisTask
   {
      private SimulationResults _result;
      private PopulationSimulationPKAnalyses _pkAnalysis;

      protected override void Context()
      {
         base.Context();
         var simulationFile = HelperForSpecs.DataFile("multiple_dosing.pkml");
         var simulationPersister = Api.GetSimulationPersister();
         _simulation = simulationPersister.LoadSimulation(simulationFile);
         _result = _simulationRunner.Run(_simulation);
      }

      protected override void Because()
      {
         _pkAnalysis = sut.CalculateFor(new CalculatePKAnalysisArgs {Simulation = _simulation, SimulationResults = _result});
      }

      [Observation]
      public void should_be_able_to_calculate_the_normalized_values()
      {
         var pkParameter = _pkAnalysis.PKParameterFor("Organism|PeripheralVenousBlood|C1|Plasma (Peripheral Venous Blood)", "C_max_tD1_tD2_norm");
         pkParameter.Values[0].ShouldBeGreaterThan(0);
      }
   }

   public class When_calculating_the_pk_analysis_for_a_population_simulation_run_for_which_one_or_more_individual_did_not_succeed : concern_for_PKAnalysisTask
   {
      private PopulationSimulationPKAnalyses _pkAnalysis;
      private IndividualValuesCache _population;
      private SimulationResults _result;

      public override void GlobalContext()
      {
         base.GlobalContext();
         var populationFile = HelperForSpecs.DataFile("pop_5.csv");
         var populationTask = Api.GetPopulationTask();
         _population = populationTask.ImportPopulation(populationFile);
         //negative volumes ensures that we have one simulation crashing
         _population.SetValues("Organism|Liver|Volume", new[] {2.3, 2.3, 2.3, -10, 2.3});
         _result = _simulationRunner.Run(_simulation, _population);
      }

      protected override void Because()
      {
         _pkAnalysis = sut.CalculateFor(new CalculatePKAnalysisArgs {Simulation = _simulation, SimulationResults = _result});
      }

      [Observation]
      public void should_be_able_to_calculate_the_pk_parameters_for_the_successful_simulations()
      {
         _pkAnalysis.ShouldNotBeNull();
         var values = _pkAnalysis.PKParameterFor(_outputPath, "C_max");
         values.Count.ShouldBeEqualTo(5);
         float.IsNaN(values.Values[3]).ShouldBeTrue();
      }
   }

   public class When_calculating_the_pk_analysis_for_a_population_with_inconsistent_id : concern_for_PKAnalysisTask
   {
      private PopulationSimulationPKAnalyses _pkAnalysis;
      private IndividualValuesCache _population;
      private SimulationResults _result;

      public override void GlobalContext()
      {
         base.GlobalContext();
         var populationFile = HelperForSpecs.DataFile("pop_5.csv");
         var populationTask = Api.GetPopulationTask();
         _population = populationTask.ImportPopulation(populationFile);
         _population.IndividualIds.Clear();
         _population.IndividualIds.AddRange(new []{ 2, 4, 6, 8, 10});
         //negative volumes ensures that we have one simulation crashing
         _population.SetValues("Organism|Liver|Volume", new[] { 2.3, 2.3, 2.3, -10, 2.3 });
         _result = _simulationRunner.Run(_simulation, _population);
      }

      protected override void Because()
      {
         _pkAnalysis = sut.CalculateFor(new CalculatePKAnalysisArgs { Simulation = _simulation, SimulationResults = _result });
      }

      [Observation]
      public void should_be_able_to_calculate_the_pk_parameters_for_the_successful_simulations()
      {
         _pkAnalysis.ShouldNotBeNull();
         var values = _pkAnalysis.PKParameterFor(_outputPath, "C_max");
         values.Count.ShouldBeEqualTo(11);
         float.IsNaN(values.Values[8]).ShouldBeTrue();
      }
   }

   public class When_calculating_the_pk_analysis_for_a_population_here_the_last_calculation_are_not_successful : concern_for_PKAnalysisTask
   {
      private PopulationSimulationPKAnalyses _pkAnalysis;
      private IndividualValuesCache _population;
      private SimulationResults _result;

      public override void GlobalContext()
      {
         base.GlobalContext();
         var populationFile = HelperForSpecs.DataFile("pop_5.csv");
         var populationTask = Api.GetPopulationTask();
         _population = populationTask.ImportPopulation(populationFile);
         //negative volumes ensures that we have one simulation crashing
         _population.SetValues("Organism|Liver|Volume", new[] { 2.3, 2.3, 2.3, 2.3, -10 });
         _result = _simulationRunner.Run(_simulation, _population);
      }

      protected override void Because()
      {
         _pkAnalysis = sut.CalculateFor(new CalculatePKAnalysisArgs { Simulation = _simulation, SimulationResults = _result });
      }

      [Observation]
      public void should_be_able_to_calculate_the_pk_parameters_for_the_successful_simulations()
      {
         _pkAnalysis.ShouldNotBeNull();
         var values = _pkAnalysis.PKParameterFor(_outputPath, "C_max");
         values.Count.ShouldBeEqualTo(4);
      }
   }
}