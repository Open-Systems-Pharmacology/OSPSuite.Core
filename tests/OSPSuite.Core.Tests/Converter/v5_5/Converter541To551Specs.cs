using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Container;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Helpers;

namespace OSPSuite.Converter.v5_5
{
   public abstract class concern_for_Converter541To551 : ContextForModelConstructorIntegration
   {
      protected IModelCoreSimulation _simulation;
      protected IBuildConfiguration _simulationConfiguration;
      protected IDimensionFactory _dimensionFactory;

      public override void GlobalContext()
      {
         base.GlobalContext();
         _simulation = LoadPKMLFile("simple_IV_53").Simulation;
         _simulationConfiguration = _simulation.BuildConfiguration;
         _dimensionFactory = IoC.Resolve<IDimensionFactory>();
      }
   }

   public class When_converting_the_simple_IV_53_simulation_to_a_55_project : concern_for_Converter541To551
   {
      [Observation]
      public void should_have_converted_the_parameter_start_value_building_block()
      {
         _simulationConfiguration.ParameterStartValues.Any().ShouldBeTrue();
         _simulationConfiguration.ParameterStartValues.Each(x => string.IsNullOrEmpty(x.Name).ShouldBeFalse());
      }

      [Observation]
      public void should_have_converted_the_molecule_start_value_building_block()
      {
         _simulationConfiguration.MoleculeStartValues.Any().ShouldBeTrue();
      }

      [Observation]
      public void should_have_set_the_scale_factor_of_all_molecule_start_value_to_the_default_scale_factor()
      {
         foreach (var moleculeStartValue in _simulationConfiguration.MoleculeStartValues)
         {
            moleculeStartValue.ScaleDivisor.ShouldBeEqualTo(Constants.DEFAULT_SCALE_DIVISOR);
         }
      }

      [Observation]
      public void should_have_set_the_dimension_and_display_unit_of_all_molecule_builder()
      {
         foreach (var molecule in _simulationConfiguration.Molecules)
         {
            var amountDimension = _dimensionFactory.Dimension(Constants.Dimension.AMOUNT);
            molecule.Dimension.ShouldBeEqualTo(amountDimension);
            molecule.DisplayUnit.ShouldBeEqualTo(amountDimension.DefaultUnit);
         }
      }

      [Observation]
      public void should_have_updated_the_observer_type_of_all_observers_defined_in_the_simulation()
      {
         foreach (var observer in _simulation.Model.Root.GetAllChildren<IObserver>())
         {
            //not only observer but observer AND sthg else (for instance drug)
            observer.QuantityType.ShouldNotBeEqualTo(QuantityType.Observer);
            observer.QuantityType.Is(QuantityType.Observer).ShouldBeTrue();
         }
      }

      [Observation]
      public void should_have_created_a_simulation_settings_building_block_in_the_build_configuration_containing_the_solver_settings_and_output_schema()
      {
         var simulationSettings = _simulationConfiguration.SimulationSettings;
         simulationSettings.ShouldNotBeNull();
         simulationSettings.Solver.ShouldNotBeNull();
         simulationSettings.OutputSchema.Intervals.Any().ShouldBeTrue();
         simulationSettings.Name.ShouldBeEqualTo(_simulation.Name);
      }
   }
}