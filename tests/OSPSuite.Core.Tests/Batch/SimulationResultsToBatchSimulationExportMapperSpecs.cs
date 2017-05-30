using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Batch;
using OSPSuite.Core.Batch.Mappers;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.Mappers;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Helpers;

namespace OSPSuite.Batch
{
   public abstract class concern_for_SimulationResultsToBatchSimulationExportMapper : ContextSpecification<ISimulationResultsToBatchSimulationExportMapper>
   {
      protected IObjectPathFactory _objectPathFactory;

      protected override void Context()
      {
         _objectPathFactory = A.Fake<IObjectPathFactory>();
         sut = new SimulationResultsToBatchSimulationExportMapper( _objectPathFactory);
      }
   }

   public class When_mapping_simulation_results_to_batch_simulation_export : concern_for_SimulationResultsToBatchSimulationExportMapper
   {
      private DataRepository _dataRepository;
      private ISimulation _simulation;
      private BatchSimulationExport _simulationExport;
      private BaseGrid _baseGrid;
      private DataColumn _col1;
      private DataColumn _col2;
      private Parameter _parameter;
      private double _absTol = 1e-2;
      private double _relTol = 1e-3;

      protected override void Context()
      {
         base.Context();
         _simulation = A.Fake<ISimulation>().WithName("SIM");
         _parameter = new Parameter().WithName("P1").WithFormula(new ConstantFormula(10));
         _simulation.Model = new Model
         {
            Root = new Container
            {
               _parameter
            }
         };

         _simulation.SimulationSettings.Solver.AbsTol = _absTol;
         _simulation.SimulationSettings.Solver.RelTol = _relTol;
         
         _baseGrid = new BaseGrid("Time", DomainHelperForSpecs.TimeDimensionForSpecs()) {Values = new[] {1f, 2f, 3f}};
         _col1 = new DataColumn("Drug1", DomainHelperForSpecs.ConcentrationDimensionForSpecs(), _baseGrid)
         {
            Values = new[] {10f, 20f, 30f},
            QuantityInfo = {Path = new[] {"P1", "P2"}},
            DataInfo = {ComparisonThreshold = 1e-2f}
         };
         _col2 = new DataColumn("Drug2", DomainHelperForSpecs.ConcentrationDimensionForSpecs(), _baseGrid)
         {
            Values = new[] {100f, 200f, 300f},
            QuantityInfo = {Path = new[] {"P3", "P4"}}
         };
         _dataRepository = new DataRepository {_col1, _col2};

         
         A.CallTo(() => _objectPathFactory.CreateAbsoluteObjectPath(_parameter)).Returns(new ObjectPath("Sim", "P1"));
      }

      protected override void Because()
      {
         _simulationExport = sut.MapFrom(_simulation, _dataRepository);
      }

      [Observation]
      public void should_return_an_object_having_the_expected_simulation_properties()
      {
         _simulationExport.Name.ShouldBeEqualTo(_simulation.Name);
         _simulationExport.AbsTol.ShouldBeEqualTo(_absTol);
         _simulationExport.RelTol.ShouldBeEqualTo(_relTol);
      }

      [Observation]
      public void should_have_created_one_output_value_for_each_output_results()
      {
         _simulationExport.OutputValues.Count.ShouldBeEqualTo(2);
         verifyOutputExport(_simulationExport.OutputValues[0], _col1, _col1.PathAsString, _col1.DataInfo.ComparisonThreshold.Value, _col1.Dimension);
         verifyOutputExport(_simulationExport.OutputValues[1], _col2, _col2.PathAsString, 0, _col2.Dimension);
      }

      private void verifyOutputExport(BatchOutputValues outputValues, DataColumn column, string path, double comparisonThreshold, IDimension dimension)
      {
         outputValues.Path.ShouldBeEqualTo(path);
         outputValues.Values.ShouldBeEqualTo(column.ConvertToDisplayValues(column.Values));
         outputValues.Dimension.ShouldBeEqualTo(dimension.Name);
         outputValues.ComparisonThreshold.ShouldBeEqualTo(comparisonThreshold);
      }

      [Observation]
      public void should_have_set_the_time_values_using_display_units()
      {
         _simulationExport.Times.ShouldBeEqualTo(_baseGrid.ConvertToDisplayValues(_baseGrid.Values));
      }

      [Observation]
      public void should_have_exported_parameter_values()
      {
         _simulationExport.ParameterValues.Count.ShouldBeEqualTo(1);
         _simulationExport.ParameterValues[0].Path.ShouldBeEqualTo(_objectPathFactory.CreateAbsoluteObjectPath(_parameter).PathAsString);
         _simulationExport.ParameterValues[0].Value.ShouldBeEqualTo(_parameter.Value);
      }
   }
}