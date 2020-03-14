using System.Data;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.PKAnalyses;
using OSPSuite.Core.Domain.SensitivityAnalyses;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Services
{
   public abstract class concern_for_SimulationResultsToDataTableConverter : ContextSpecification<ISimulationResultsToDataTableConverter>
   {
      private IEntitiesInSimulationRetriever _quantityRetriever;
      private IDimensionFactory _dimensionRepository;
      protected IDisplayUnitRetriever _displayUnitRetriever;
      private IPKParameterRepository _pkParameterRepository;

      protected override void Context()
      {
         _quantityRetriever = A.Fake<IEntitiesInSimulationRetriever>();
         _dimensionRepository = A.Fake<IDimensionFactory>();
         _displayUnitRetriever = A.Fake<IDisplayUnitRetriever>();
         _pkParameterRepository= A.Fake<IPKParameterRepository>();   
         sut = new SimulationResultsToDataTableConverter(_dimensionRepository, _quantityRetriever, _displayUnitRetriever, _pkParameterRepository);
      }
   }

   public class When_creating_a_data_table_based_on_the_pk_analyses_of_a_population_simulation : concern_for_SimulationResultsToDataTableConverter
   {
      private DataTable _dataTable;
      private PopulationSimulationPKAnalyses _pkAnalysis;
      private IModelCoreSimulation _populationSimulation;

      protected override void Context()
      {
         base.Context();
         _pkAnalysis = new PopulationSimulationPKAnalyses();
         _populationSimulation = A.Fake<IModelCoreSimulation>();
         var pkParameter = new QuantityPKParameter { QuantityPath = "Liver", Name = "P" };
         var dimension = A.Fake<IDimension>();
         var unit = A.Fake<Unit>();
         A.CallTo(() => unit.Name).Returns("UNIT");
         A.CallTo(() => _displayUnitRetriever.PreferredUnitFor(pkParameter, null)).Returns(unit);

         pkParameter.Dimension = dimension;
         pkParameter.SetNumberOfIndividuals(2);
         pkParameter.SetValue(0, 10);
         pkParameter.SetValue(1, 11);

         A.CallTo(() => dimension.BaseUnitValueToUnitValue(unit, 10)).Returns(100.10);
         A.CallTo(() => dimension.BaseUnitValueToUnitValue(unit, 11)).Returns(110.20);

         _pkAnalysis.AddPKAnalysis(pkParameter);
      }

      protected override void Because()
      {
         _dataTable = sut.PKAnalysesToDataTable(_pkAnalysis, _populationSimulation);
      }

      [Observation]
      public void should_return_a_table_containing_the_expected_columns_in_the_expected_order()
      {
         _dataTable.Columns.Count.ShouldBeEqualTo(6);
         _dataTable.Columns[0].ColumnName.ShouldBeEqualTo(Constants.SimulationResults.INDIVIDUAL_ID);
         _dataTable.Columns[1].ColumnName.ShouldBeEqualTo(Constants.SimulationResults.QUANTITY_PATH);
         _dataTable.Columns[2].ColumnName.ShouldBeEqualTo(Constants.SimulationResults.PARAMETER);
         _dataTable.Columns[3].ColumnName.ShouldBeEqualTo(Constants.SimulationResults.VALUE);
         _dataTable.Columns[4].ColumnName.ShouldBeEqualTo(Constants.SimulationResults.UNIT);
         _dataTable.Columns[5].ColumnName.ShouldBeEqualTo(Constants.SimulationResults.DISPLAY);
      }

      [Observation]
      public void should_have_exported_the_value_in_the_default_unit()
      {
         _dataTable.Rows.Count.ShouldBeEqualTo(2);
         _dataTable.Rows[0][Constants.SimulationResults.VALUE].ShouldBeEqualTo("100.1");
         _dataTable.Rows[1][Constants.SimulationResults.VALUE].ShouldBeEqualTo("110.2");
      }

      [Observation]
      public void should_have_set_the_expected_value_in_other_columns()
      {
         _dataTable.Rows[0][Constants.SimulationResults.INDIVIDUAL_ID].ShouldBeEqualTo(0);
         _dataTable.Rows[0][Constants.SimulationResults.QUANTITY_PATH].ShouldBeEqualTo("\"Liver\"");
         _dataTable.Rows[0][Constants.SimulationResults.PARAMETER].ShouldBeEqualTo("\"P\"");
         _dataTable.Rows[0][Constants.SimulationResults.UNIT].ShouldBeEqualTo("UNIT");
         _dataTable.Rows[1][Constants.SimulationResults.INDIVIDUAL_ID].ShouldBeEqualTo(1);
         _dataTable.Rows[1][Constants.SimulationResults.QUANTITY_PATH].ShouldBeEqualTo("\"Liver\"");
         _dataTable.Rows[1][Constants.SimulationResults.PARAMETER].ShouldBeEqualTo("\"P\"");
         _dataTable.Rows[1][Constants.SimulationResults.UNIT].ShouldBeEqualTo("UNIT");
      }
   }

   public class When_creating_a_data_table_based_on_the_sensitivity_analysis_run_result: concern_for_SimulationResultsToDataTableConverter
   {
      private DataTable _dataTable;
      private SensitivityAnalysisRunResult _sensitivityAnalysisRunResult;
      private IModelCoreSimulation _simulation;

      protected override void Context()
      {
         base.Context();
         _sensitivityAnalysisRunResult = new SensitivityAnalysisRunResult();
         _simulation = A.Fake<IModelCoreSimulation>().WithName("Sim");
         var pkParameter = new PKParameterSensitivity { QuantityPath = "Liver", PKParameterName = "AUC", ParameterName = "P1", Value = 0.5};


         _sensitivityAnalysisRunResult.AddPKParameterSensitivity(pkParameter);
      }

      protected override void Because()
      {
         _dataTable = sut.SensitivityAnalysisResultsToDataTable(_sensitivityAnalysisRunResult, _simulation);
      }

      [Observation]
      public void should_return_a_table_containing_the_expected_columns_in_the_expected_order()
      {
         _dataTable.Columns.Count.ShouldBeEqualTo(4);
         _dataTable.Columns[0].ColumnName.ShouldBeEqualTo(Constants.SensitivityAnalysisResults.QUANTITY_PATH);
         _dataTable.Columns[1].ColumnName.ShouldBeEqualTo(Constants.SensitivityAnalysisResults.PARAMETER);
         _dataTable.Columns[2].ColumnName.ShouldBeEqualTo(Constants.SensitivityAnalysisResults.PK_PARAMETER);
         _dataTable.Columns[3].ColumnName.ShouldBeEqualTo(Constants.SensitivityAnalysisResults.VALUE);
      }


      [Observation]
      public void should_have_set_the_expected_value_in_other_columns()
      {
         _dataTable.Rows[0][Constants.SensitivityAnalysisResults.QUANTITY_PATH].ShouldBeEqualTo("\"Liver\"");
         _dataTable.Rows[0][Constants.SensitivityAnalysisResults.PARAMETER].ShouldBeEqualTo("\"P1\"");
         _dataTable.Rows[0][Constants.SensitivityAnalysisResults.PK_PARAMETER].ShouldBeEqualTo("\"AUC\"");
         _dataTable.Rows[0][Constants.SensitivityAnalysisResults.VALUE].ShouldBeEqualTo("0.5");
      }
   }

}