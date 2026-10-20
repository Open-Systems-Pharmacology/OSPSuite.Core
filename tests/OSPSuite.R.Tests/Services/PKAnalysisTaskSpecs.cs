using System;
using System.Data;
using System.IO;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.PKAnalyses;
using OSPSuite.Core.Domain.Populations;
using OSPSuite.R.Domain;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;
using static OSPSuite.Core.Domain.Constants.PKParameters;

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
         _result = _simulationRunner.Run(new SimulationRunArgs { Simulation = _simulation });
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

   public class When_converting_the_pk_analysis_to_a_csv_string : concern_for_PKAnalysisTask
   {
      private PopulationSimulationPKAnalyses _pkAnalysis;
      private string _csvFile;
      private string _csv;

      public override void GlobalContext()
      {
         base.GlobalContext();
         var result = _simulationRunner.Run(new SimulationRunArgs { Simulation = _simulation });
         _pkAnalysis = sut.CalculateFor(new CalculatePKAnalysisArgs { Simulation = _simulation, SimulationResults = result });
         _csvFile = FileHelper.GenerateTemporaryFileName();
         sut.ExportPKAnalysesToCSV(_pkAnalysis, _simulation, _csvFile);
      }

      protected override void Because()
      {
         _csv = sut.PKAnalysesToCSV(_pkAnalysis, _simulation);
      }

      [Observation]
      public void should_return_the_same_content_as_the_exported_csv_file()
      {
         _csv.ShouldBeEqualTo(File.ReadAllText(_csvFile));
      }

      [Observation]
      public void should_return_the_quoted_header_followed_by_one_line_for_each_pk_parameter_value()
      {
         var allLines = _csv.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
         allLines[0].ShouldBeEqualTo("\"IndividualId\",\"QuantityPath\",\"Parameter\",\"Value\",\"Unit\"");
         allLines.Length.ShouldBeEqualTo(_pkAnalysis.All().Sum(x => x.Count) + 1);
      }

      public override void GlobalCleanup()
      {
         base.GlobalCleanup();
         FileHelper.DeleteFile(_csvFile);
      }
   }

   public abstract class concern_for_PKAnalysisTask_converting_selected_pk_analyses_to_csv : concern_for_PKAnalysisTask
   {
      protected const string ARTERIAL_PLASMA_PATH = "Organism|ArterialBlood|Plasma|Caffeine|Concentration in container";
      protected const string WHOLE_BLOOD_PATH = "Organism|ArterialBlood|Caffeine|Whole Blood";
      private PopulationSimulationPKAnalyses _calculatedPKAnalyses;
      protected PopulationSimulationPKAnalyses _pkAnalyses;
      protected PKAnalysesSelection _selection;
      protected string _csv;

      public override void GlobalContext()
      {
         base.GlobalContext();
         var result = _simulationRunner.Run(new SimulationRunArgs { Simulation = _simulation });
         _calculatedPKAnalyses = sut.CalculateFor(new CalculatePKAnalysisArgs { Simulation = _simulation, SimulationResults = result });

         //Ordered by PK parameter and not by output, to show that the selection keeps the order of the PK-Analyses
         _pkAnalyses = PKAnalysesWith(
            (_outputPath, C_max), (ARTERIAL_PLASMA_PATH, C_max), (WHOLE_BLOOD_PATH, C_max),
            (_outputPath, Tmax), (ARTERIAL_PLASMA_PATH, Tmax), (WHOLE_BLOOD_PATH, Tmax),
            (_outputPath, AUC_inf), (ARTERIAL_PLASMA_PATH, AUC_inf), (WHOLE_BLOOD_PATH, AUC_inf));
      }

      protected override void Because()
      {
         _csv = sut.PKAnalysesToCSV(_pkAnalyses, _simulation, _selection);
      }

      protected PopulationSimulationPKAnalyses PKAnalysesWith(params (string quantityPath, string pkParameterName)[] pkParameters)
      {
         var pkAnalyses = new PopulationSimulationPKAnalyses();
         pkParameters.Each(x => pkAnalyses.AddPKAnalysis(_calculatedPKAnalyses.PKParameterFor(x.quantityPath, x.pkParameterName)));
         return pkAnalyses;
      }

      protected string CSVFor(params (string quantityPath, string pkParameterName)[] pkParameters) => sut.PKAnalysesToCSV(PKAnalysesWith(pkParameters), _simulation);
   }

   public class When_converting_the_pk_analyses_of_some_outputs_and_pk_parameters_to_a_csv_string : concern_for_PKAnalysisTask_converting_selected_pk_analyses_to_csv
   {
      protected override void Context()
      {
         base.Context();
         _selection = new PKAnalysesSelection
         {
            QuantityPaths = new[] { WHOLE_BLOOD_PATH, _outputPath },
            PKParameterNames = new[] { Tmax, C_max }
         };
      }

      [Observation]
      public void should_return_the_selected_pk_analyses_in_the_order_of_the_pk_analyses()
      {
         _csv.ShouldBeEqualTo(CSVFor((_outputPath, C_max), (WHOLE_BLOOD_PATH, C_max), (_outputPath, Tmax), (WHOLE_BLOOD_PATH, Tmax)));
      }

      [Observation]
      public void should_not_change_the_pk_analyses()
      {
         _pkAnalyses.All().Count().ShouldBeEqualTo(9);
      }
   }

   public class When_converting_the_pk_analyses_of_one_output_and_one_pk_parameter_to_a_csv_string : concern_for_PKAnalysisTask_converting_selected_pk_analyses_to_csv
   {
      protected override void Context()
      {
         base.Context();
         _selection = new PKAnalysesSelection
         {
            QuantityPath = ARTERIAL_PLASMA_PATH,
            PKParameterName = AUC_inf
         };
      }

      [Observation]
      public void should_return_the_pk_analysis_of_this_output_and_pk_parameter()
      {
         _csv.ShouldBeEqualTo(CSVFor((ARTERIAL_PLASMA_PATH, AUC_inf)));
      }
   }

   public class When_converting_the_pk_analyses_of_one_pk_parameter_to_a_csv_string : concern_for_PKAnalysisTask_converting_selected_pk_analyses_to_csv
   {
      protected override void Context()
      {
         base.Context();
         _selection = new PKAnalysesSelection { PKParameterName = Tmax };
      }

      [Observation]
      public void should_return_this_pk_parameter_for_all_outputs()
      {
         _csv.ShouldBeEqualTo(CSVFor((_outputPath, Tmax), (ARTERIAL_PLASMA_PATH, Tmax), (WHOLE_BLOOD_PATH, Tmax)));
      }
   }

   public class When_converting_the_pk_analyses_of_some_outputs_to_a_csv_string : concern_for_PKAnalysisTask_converting_selected_pk_analyses_to_csv
   {
      protected override void Context()
      {
         base.Context();
         _selection = new PKAnalysesSelection { QuantityPaths = new[] { ARTERIAL_PLASMA_PATH, _outputPath } };
      }

      [Observation]
      public void should_return_all_pk_parameters_of_these_outputs()
      {
         _csv.ShouldBeEqualTo(CSVFor(
            (_outputPath, C_max), (ARTERIAL_PLASMA_PATH, C_max),
            (_outputPath, Tmax), (ARTERIAL_PLASMA_PATH, Tmax),
            (_outputPath, AUC_inf), (ARTERIAL_PLASMA_PATH, AUC_inf)));
      }
   }

   public class When_converting_the_pk_analyses_with_a_selection_where_nothing_is_set_to_a_csv_string : concern_for_PKAnalysisTask_converting_selected_pk_analyses_to_csv
   {
      protected override void Context()
      {
         base.Context();
         _selection = new PKAnalysesSelection();
      }

      [Observation]
      public void should_return_all_pk_analyses()
      {
         _csv.ShouldBeEqualTo(sut.PKAnalysesToCSV(_pkAnalyses, _simulation));
      }
   }

   public class When_converting_the_pk_analyses_without_a_selection_to_a_csv_string : concern_for_PKAnalysisTask_converting_selected_pk_analyses_to_csv
   {
      protected override void Context()
      {
         base.Context();
         _selection = null;
      }

      [Observation]
      public void should_return_all_pk_analyses()
      {
         _csv.ShouldBeEqualTo(sut.PKAnalysesToCSV(_pkAnalyses, _simulation));
      }
   }

   public class When_converting_the_pk_analyses_of_an_output_that_does_not_exist_to_a_csv_string : concern_for_PKAnalysisTask_converting_selected_pk_analyses_to_csv
   {
      protected override void Context()
      {
         base.Context();
         _selection = new PKAnalysesSelection
         {
            QuantityPath = "Organism|DoesNotExist",
            PKParameterName = C_max
         };
      }

      [Observation]
      public void should_only_return_the_header()
      {
         _csv.ShouldBeEqualTo(CSVFor());
      }
   }

   public class When_converting_the_pk_analyses_of_an_empty_list_of_pk_parameters_to_a_csv_string : concern_for_PKAnalysisTask_converting_selected_pk_analyses_to_csv
   {
      protected override void Context()
      {
         base.Context();
         _selection = new PKAnalysesSelection
         {
            QuantityPath = _outputPath,
            PKParameterNames = Array.Empty<string>()
         };
      }

      [Observation]
      public void should_only_return_the_header()
      {
         _csv.ShouldBeEqualTo(CSVFor());
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
         _result = _simulationRunner.Run(new SimulationRunArgs { Simulation = _simulation });
      }

      protected override void Because()
      {
         _pkAnalysis = sut.CalculateFor(new CalculatePKAnalysisArgs {Simulation = _simulation, SimulationResults = _result});
      }

      [Observation]
      public void should_be_able_to_calculate_the_normalized_values()
      {
         var pkParameter = _pkAnalysis.PKParameterFor("Organism|PeripheralVenousBlood|C1|Plasma (Peripheral Venous Blood)", "C_max_tD1_tD2_norm");
         pkParameter.ValueFor(0).ShouldBeGreaterThan(0);
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
         _result = _simulationRunner.Run(new SimulationRunArgs { Simulation = _simulation, Population = _population});
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
         values.Count.ShouldBeEqualTo(4);
      }
   }

   public class When_calculating_the_pk_analysis_for_a_population_with_sparse_ids : concern_for_PKAnalysisTask
   {
      private PopulationSimulationPKAnalyses _pkAnalysis;
      private IndividualValuesCache _population;
      private SimulationResults _result;

      public override void GlobalContext()
      {
         base.GlobalContext();
         var populationFile = HelperForSpecs.DataFile("pop_5_sparse_id.csv");
         var populationTask = Api.GetPopulationTask();
         _population = populationTask.ImportPopulation(populationFile);
         _result = _simulationRunner.Run(new SimulationRunArgs { Simulation = _simulation, Population = _population });
      }

      protected override void Because()
      {
         _pkAnalysis = sut.CalculateFor(new CalculatePKAnalysisArgs { Simulation = _simulation, SimulationResults = _result });
      }

      [Observation]
      public void should_not_create_more_entries_in_the_returns_value_that_expected()
      {
         _pkAnalysis.ShouldNotBeNull();
         var values = _pkAnalysis.PKParameterFor(_outputPath, "C_max");
         values.Count.ShouldBeEqualTo(5);
         float.IsNaN(values.ValueFor(3)).ShouldBeTrue();
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
         _result = _simulationRunner.Run(new SimulationRunArgs { Simulation = _simulation, Population = _population});
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
         _result = _simulationRunner.Run(new SimulationRunArgs { Simulation = _simulation, Population = _population});
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