using System.Data;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.PKAnalyses;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Core.Services
{
   public abstract class concern_for_PopulationSimulationPKAnalysesToDataTableConverter : ContextSpecification<IPopulationSimulationPKAnalysesToDataTableConverter>
   {
      protected IDimensionFactory _dimensionFactory;
      protected IPKParameterRepository _pkParameterRepository;

      protected override void Context()
      {
         _dimensionFactory= new DimensionFactory();
         _pkParameterRepository = new PKParameterRepository();
         sut = new PopulationSimulationPKAnalysesToDataTableConverter(_dimensionFactory,_pkParameterRepository);
      }
   }


   public class When_creating_a_data_table_based_on_the_pk_analyses_of_a_population_simulation : concern_for_PopulationSimulationPKAnalysesToDataTableConverter
   {
      private DataTable _dataTable;
      private PopulationSimulationPKAnalyses _pkAnalysis;
      private IModelCoreSimulation _populationSimulation;

      protected override void Context()
      {
         base.Context();
         _pkAnalysis = new PopulationSimulationPKAnalyses();
         _populationSimulation = new ModelCoreSimulation().WithName("simulation");
         var quantityPKParameter = new QuantityPKParameter { QuantityPath = "Liver", Name = "P" };
         var anotherQuantityPKParameter = new QuantityPKParameter { QuantityPath = "Plasma", Name = "Vss (Plasma)" };
         var pkParameter = new PKParameter {DisplayUnit = "UNIT", Name = "P" };

         _pkParameterRepository.Add(pkParameter);

         var mergedDimension = new Dimension(new BaseDimensionRepresentation(), "mergedDimension", pkParameter.DisplayUnit)
         {
            DefaultUnit =
            {
               Factor = 0.1
            }
         };

         var anotherMergedDimension = new Dimension(new BaseDimensionRepresentation(), "overF", "baseUnit");
         quantityPKParameter.SetValue(0, 10);
         quantityPKParameter.SetValue(1, 11);
         anotherQuantityPKParameter.SetValue(0, 10);
         anotherQuantityPKParameter.SetValue(1, 11);
         anotherQuantityPKParameter.SetValue(0, 12);
         anotherQuantityPKParameter.SetValue(1, 13);

         quantityPKParameter.Dimension = mergedDimension;
         anotherQuantityPKParameter.Dimension = anotherMergedDimension;

         _dimensionFactory.AddDimension(mergedDimension);
         _dimensionFactory.AddDimension(anotherMergedDimension);

         var anotherUnit = new Unit("RealUnit", 10, 0);
         anotherMergedDimension.AddUnit(anotherUnit);
         anotherMergedDimension.DefaultUnit = anotherUnit;

         _pkAnalysis.AddPKAnalysis(quantityPKParameter);
         _pkAnalysis.AddPKAnalysis(anotherQuantityPKParameter);
      }

      protected override void Because()
      {
         _dataTable = sut.PKAnalysesToDataTable(_pkAnalysis, _populationSimulation);
      }

      [Observation]
      public void should_return_a_table_containing_the_expected_columns_in_the_expected_order()
      {
         _dataTable.Columns.Count.ShouldBeEqualTo(5);
         _dataTable.Columns[0].ColumnName.ShouldBeEqualTo(Constants.SimulationResults.INDIVIDUAL_ID);
         _dataTable.Columns[1].ColumnName.ShouldBeEqualTo(Constants.SimulationResults.QUANTITY_PATH);
         _dataTable.Columns[2].ColumnName.ShouldBeEqualTo(Constants.SimulationResults.PARAMETER);
         _dataTable.Columns[3].ColumnName.ShouldBeEqualTo(Constants.SimulationResults.VALUE);
         _dataTable.Columns[4].ColumnName.ShouldBeEqualTo(Constants.SimulationResults.UNIT);
      }

      [Observation]
      public void should_have_exported_the_value_in_the_default_unit()
      {
         _dataTable.Rows.Count.ShouldBeEqualTo(4);
         _dataTable.Rows[0][Constants.SimulationResults.VALUE].ShouldBeEqualTo("100");
         _dataTable.Rows[1][Constants.SimulationResults.VALUE].ShouldBeEqualTo("110");
         _dataTable.Rows[2][Constants.SimulationResults.VALUE].ShouldBeEqualTo("1.2");
         _dataTable.Rows[3][Constants.SimulationResults.VALUE].ShouldBeEqualTo("1.3");
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

}