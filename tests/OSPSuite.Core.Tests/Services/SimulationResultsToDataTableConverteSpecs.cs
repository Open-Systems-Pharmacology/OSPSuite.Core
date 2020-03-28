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

      protected override void Context()
      {
         _quantityRetriever = A.Fake<IEntitiesInSimulationRetriever>();
         _dimensionRepository = A.Fake<IDimensionFactory>();
         _displayUnitRetriever = A.Fake<IDisplayUnitRetriever>();
         sut = new SimulationResultsToDataTableConverter(_dimensionRepository, _quantityRetriever);
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