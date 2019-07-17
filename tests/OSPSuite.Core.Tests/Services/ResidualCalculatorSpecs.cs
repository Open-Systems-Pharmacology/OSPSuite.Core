using System;
using System.Collections.Generic;
using System.Linq;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Helpers;

namespace OSPSuite.Core.Services
{
   public abstract class concern_for_ResidualCalculator : ContextSpecification<IResidualCalculator>
   {
      protected IDimensionFactory _dimensionFactory;
      protected ITimeGridRestrictor _timeGridRestrictor;

      protected List<SimulationRunResults> _simulationRunResultsList;
      protected List<OutputMapping> _outputMappings;
      protected DataRepository _simulationResults;
      protected DataRepository _observedData;
      protected OutputMapping _outputMapping;
      protected DataColumn _simulationDataColumn;
      protected string _fullOutputPath;
      protected DataColumn _observedDataColumn;
      protected float[] _weights;
      protected IDimension _mergedDimension;

      protected override void Context()
      {
         //Only testing the common behavior of Residual Calculator. Specific tests needs to be written for each calculator implementation
         //sut = new ResidualCalculatorForOnlyObservedData(_timeGridRestrictor, _dimensionFactory);

         _simulationResults = DomainHelperForSpecs.SimulationDataRepositoryFor("Sim");
         _simulationDataColumn = _simulationResults.AllButBaseGrid().First();
         _simulationRunResultsList = new List<SimulationRunResults> {new SimulationRunResults(true, Enumerable.Empty<SolverWarning>(), _simulationResults)};

         _observedData = DomainHelperForSpecs.ObservedDataRepository2WithLLOQ().WithName("Obs");
         _observedDataColumn = _observedData.AllButBaseGrid().First();

         UpdateObservedDataValues();

         _timeGridRestrictor = A.Fake<ITimeGridRestrictor>();
         //Returns all values of the observed data array. We do not test the time grid restrictor here!
         A.CallTo(_timeGridRestrictor).WithReturnType<IReadOnlyList<int>>().Returns(Enumerable.Range(0, _observedDataColumn.Values.Count).ToList());

         _mergedDimension = A.Fake<IDimension>();
         _dimensionFactory = A.Fake<IDimensionFactory>();
         A.CallTo(() => _dimensionFactory.MergedDimensionFor(_simulationDataColumn)).Returns(_mergedDimension);


         _outputMappings = new List<OutputMapping>();
         _outputMapping = A.Fake<OutputMapping>();
         _fullOutputPath = _simulationDataColumn.QuantityInfo.PathAsString;
         A.CallTo(() => _outputMapping.FullOutputPath).Returns(_fullOutputPath);

         _outputMapping.Weight = 1f;
         _outputMapping.WeightedObservedData = new WeightedObservedData(_observedData);
         _weights = _outputMapping.WeightedObservedData.Weights;

         _outputMappings.Add(_outputMapping);
         for (int i = 0; i < _observedDataColumn.Values.Count; i++)
         {
            _outputMapping.WeightedObservedData.Weights[i] = 1f;
            A.CallTo(() => _mergedDimension.UnitValueToBaseUnitValue(_observedDataColumn.Dimension.BaseUnit, _observedDataColumn.Values[i])).Returns(_observedDataColumn.Values[i]);
         }

         A.CallTo(() => _mergedDimension.UnitValueToBaseUnitValue(_observedDataColumn.Dimension.BaseUnit, float.NaN)).Returns(float.NaN);
         A.CallTo(() => _mergedDimension.UnitValueToBaseUnitValue(_observedDataColumn.Dimension.BaseUnit, _observedDataColumn.DataInfo.LLOQ.Value)).Returns(_observedDataColumn.DataInfo.LLOQ.Value);
      }

      protected virtual void UpdateObservedDataValues()
      {
         //Update observed data value here in tests requiring special values
      }

      protected double ToDouble(float x)
      {
         return Convert.ToDouble(x);
      }

      protected float Max(float x, float lloq)
      {
         return x < lloq ? lloq : x;
      }
   }

   public class When_calculating_the_residuals_for_a_simulation_results_containing_more_than_one_output : concern_for_ResidualCalculator
   {
      private OutputMapping _outputMapping2;
      private ResidualsResult _residualResult;

      protected override void Context()
      {
         base.Context();
         var otherSimulationDataColumn = new DataColumn("Col", DomainHelperForSpecs.ConcentrationDimensionForSpecs(), _simulationDataColumn.BaseGrid)
         {
            Values = new[] {0f, 2.5f, 0.9f, 0.9f, 0.5f},
            DataInfo = {Origin = ColumnOrigins.Calculation},
            QuantityInfo = new QuantityInfo("Concentration", new[] {"Sim", "Organism", "Liver", "Plasma", "Concentration"}, QuantityType.Drug)
         };

         _simulationResults.Add(otherSimulationDataColumn);

         _outputMapping2 = A.Fake<OutputMapping>();
         A.CallTo(() => _outputMapping2.FullOutputPath).Returns(otherSimulationDataColumn.PathAsString);
         var observedData2 = DomainHelperForSpecs.ObservedData().WithName("OBS2");
         _outputMapping2.WeightedObservedData = new WeightedObservedData(observedData2);
         _outputMappings.Add(_outputMapping2);

         sut = new ResidualCalculatorForOnlyObservedData(_timeGridRestrictor, _dimensionFactory);
      }

      protected override void Because()
      {
         _residualResult = sut.Calculate(_simulationRunResultsList, _outputMappings);
      }

      [Observation]
      public void should_add_one_residual_for_each_simulated_output()
      {
         _residualResult.AllOutputResiduals.Count.ShouldBeEqualTo(2);
      }
   }

   public class When_calculating_the_residuals_for_a_set_of_output_results_and_output_mappings : concern_for_ResidualCalculator
   {
      private ResidualsResult _residualResult;
      private OutputResiduals _outputResiduals;

      protected override void Context()
      {
         base.Context();
         sut = new ResidualCalculatorForOnlyObservedData(_timeGridRestrictor, _dimensionFactory);
         _outputMapping.Scaling = Scalings.Linear;

         _outputMapping.Weight = 5;
         _outputMapping.WeightedObservedData.Weights[0] = 2f;
         _outputMapping.WeightedObservedData.Weights[1] = 4f;
         _outputMapping.WeightedObservedData.Weights[2] = 6f;

         for (int i = 0; i < _observedDataColumn.Values.Count; i++)
         {
            A.CallTo(() => _mergedDimension.UnitValueToBaseUnitValue(_observedDataColumn.Dimension.BaseUnit, _observedDataColumn.Values[i])).Returns(_observedDataColumn.Values[i] * 10f);
         }
      }

      protected override void Because()
      {
         _residualResult = sut.Calculate(_simulationRunResultsList, _outputMappings);
         _outputResiduals = _residualResult.AllOutputResidualsFor(_fullOutputPath).First();
      }

      [Observation]
      public void should_add_one_residuals_output_for_each_simulated_output()
      {
         _residualResult.AllOutputResiduals.Count.ShouldBeEqualTo(1);
         _residualResult.AllOutputResidualsFor(_fullOutputPath).Count.ShouldBeEqualTo(1);
      }

      [Observation]
      public void the_created_residuals_output_should_contain_one_residual_for_each_observed_data_time_point()
      {
         _outputResiduals.Residuals.Count.ShouldBeEqualTo(_observedDataColumn.Values.Count);
      }

      [Observation]
      public void should_calculate_the_residual_values_using_the_corresponding_weight_and_scaling_and_the_corresponding_merged_dimension_for_observed_data_values()
      {
         for (int i = 0; i < _outputResiduals.Residuals.Count; i++)
         {
            _outputResiduals.Residuals[i].Value.ShouldBeEqualTo((ToDouble(_simulationDataColumn.GetValue(_observedDataColumn.BaseGrid[i])) - ToDouble(_observedDataColumn.Values[i] * 10f)) * _weights[i] * _outputMapping.Weight);
         }
      }
   }

   public class When_calculating_the_residuals_for_an_output_mapped_with_two_observed_data : concern_for_ResidualCalculator
   {
      private OutputMapping _outputMapping2;
      private DataRepository _observedData2;
      private ResidualsResult _residualResult;

      protected override void Context()
      {
         base.Context();
         sut = new ResidualCalculatorForOnlyObservedData(_timeGridRestrictor, _dimensionFactory);
         _outputMapping.Scaling = Scalings.Linear;

         _outputMapping2 = A.Fake<OutputMapping>();
         A.CallTo(() => _outputMapping2.FullOutputPath).Returns(_fullOutputPath);
         _observedData2 = DomainHelperForSpecs.ObservedData().WithName("OBS2");
         _outputMapping2.WeightedObservedData = new WeightedObservedData(_observedData2);
         _outputMappings.Add(_outputMapping2);
      }

      protected override void Because()
      {
         _residualResult = sut.Calculate(_simulationRunResultsList, _outputMappings);
      }

      [Observation]
      public void should_create_two_output_residuals()
      {
         _residualResult.AllOutputResidualsFor(_fullOutputPath).Count.ShouldBeEqualTo(2);
      }
   }

   public class When_calculating_the_residuals_for_more_simulation_data_than_observed_data : concern_for_ResidualCalculator
   {
      private ResidualsResult _residualResult;
      private OutputResiduals _outputResiduals;

      protected override void Context()
      {
         base.Context();
         sut = new ResidualCalculatorForOnlyObservedData(_timeGridRestrictor, _dimensionFactory);
         _outputMapping.Scaling = Scalings.Linear;

         _simulationDataColumn.BaseGrid.Values = new[] {0f, 1f, 1.5f, 2f, 3f, 4f};
         _simulationDataColumn.Values = new[] {0f, 2.5f, 1.3f, 0.9f, 0.9f, 0.5f};
      }

      protected override void Because()
      {
         _residualResult = sut.Calculate(_simulationRunResultsList, _outputMappings);
         _outputResiduals = _residualResult.AllOutputResidualsFor(_fullOutputPath).First();
      }

      [Observation]
      public void created_residuals_output_should_contain_one_residual_for_each_observed_data_time_point()
      {
         _outputResiduals.Residuals.Count.ShouldBeEqualTo(_observedDataColumn.Values.Count);
      }

      [Observation]
      public void should_calculate_the_correct_residual_values_for_observed_data_values()
      {
         for (int i = 0; i < _outputResiduals.Residuals.Count; i++)
         {
            _outputResiduals.Residuals[i].Value.ShouldBeEqualTo(_simulationDataColumn.GetValue(_observedDataColumn.BaseGrid[i]) - _observedDataColumn[i]);
         }
      }
   }

   public class When_calculating_the_residuals_by_ResidualCalculatorForOnlyObservedData_with_lin_scaling : concern_for_ResidualCalculator
   {
      private ResidualsResult _residualResult;
      private OutputResiduals _outputResiduals;

      protected override void Context()
      {
         base.Context();
         sut = new ResidualCalculatorForOnlyObservedData(_timeGridRestrictor, _dimensionFactory);
         _outputMapping.Scaling = Scalings.Linear;
      }

      protected override void Because()
      {
         _residualResult = sut.Calculate(_simulationRunResultsList, _outputMappings);
         _outputResiduals = _residualResult.AllOutputResidualsFor(_fullOutputPath).First();
      }

      [Observation]
      public void created_residuals_output_should_contain_one_residual_for_each_observed_data_time_point()
      {
         _outputResiduals.Residuals.Count.ShouldBeEqualTo(_observedDataColumn.Values.Count);
      }

      [Observation]
      public void should_calculate_the_correct_residual_values_for_observed_data_values()
      {
         for (int i = 0; i < _outputResiduals.Residuals.Count; i++)
         {
            float simValue = _simulationDataColumn.GetValue(_observedDataColumn.BaseGrid[i]);
            _outputResiduals.Residuals[i].Value.ShouldBeEqualTo(ToDouble(simValue) - ToDouble(_observedDataColumn[i]));
         }
      }
   }

   public class When_calculating_the_residuals_by_ResidualCalculatorForOnlyObservedData_with_log_scaling : concern_for_ResidualCalculator
   {
      private ResidualsResult _residualResult;
      private OutputResiduals _outputResiduals;

      protected override void Context()
      {
         base.Context();
         sut = new ResidualCalculatorForOnlyObservedData(_timeGridRestrictor, _dimensionFactory);
         _outputMapping.Scaling = Scalings.Log;
      }

      protected override void Because()
      {
         _residualResult = sut.Calculate(_simulationRunResultsList, _outputMappings);
         _outputResiduals = _residualResult.AllOutputResidualsFor(_fullOutputPath).First();
      }

      [Observation]
      public void created_residuals_output_should_contain_one_residual_for_each_observed_data_time_point()
      {
         _outputResiduals.Residuals.Count.ShouldBeEqualTo(_observedDataColumn.Values.Count);
      }

      [Observation]
      public void should_calculate_the_correct_residual_values_for_observed_data_values()
      {
         for (int i = 0; i < _outputResiduals.Residuals.Count; i++)
         {
            float simValue = _simulationDataColumn.GetValue(_observedDataColumn.BaseGrid[i]);
            _outputResiduals.Residuals[i].Value.ShouldBeEqualTo(Math.Log10(simValue) - Math.Log10(_observedDataColumn[i]));
         }
      }
   }

   public class When_calculating_the_residuals_by_ResidualCalculatorForOnlyObservedData_with_log_scaling_for_0 : concern_for_ResidualCalculator
   {
      private ResidualsResult _residualResult;
      private OutputResiduals _outputResiduals;

      protected override void Context()
      {
         base.Context();
         sut = new ResidualCalculatorForOnlyObservedData(_timeGridRestrictor, _dimensionFactory);
         _outputMapping.Scaling = Scalings.Log;

         _simulationDataColumn.BaseGrid.Values = new[] {1f, 2f, 3f};
         _simulationDataColumn.Values = new[] {0f, 0f, 0f};
      }

      protected override void Because()
      {
         _residualResult = sut.Calculate(_simulationRunResultsList, _outputMappings);
         _outputResiduals = _residualResult.AllOutputResidualsFor(_fullOutputPath).First();
      }

      [Observation]
      public void created_residuals_output_should_contain_one_residual_for_each_observed_data_time_point()
      {
         _outputResiduals.Residuals.Count.ShouldBeEqualTo(_observedDataColumn.Values.Count);
      }

      [Observation]
      public void should_calculate_the_correct_residual_values_for_observed_data_values()
      {
         for (int i = 0; i < _outputResiduals.Residuals.Count; i++)
         {
            _outputResiduals.Residuals[i].Value.ShouldBeEqualTo(Math.Log10(1E-20f) - Math.Log10(_observedDataColumn[i]));
         }
      }
   }

   public class When_calculating_the_residuals_by_ResidualCalculatorForSimulationOutputAsObservedDataLLOQ_with_lin_scaling : concern_for_ResidualCalculator
   {
      private ResidualsResult _residualResult;
      private OutputResiduals _outputResiduals;

      protected override void Context()
      {
         base.Context();
         sut = new ResidualCalculatorForSimulationOutputAsObservedDataLLOQ(_timeGridRestrictor, _dimensionFactory);
         _outputMapping.Scaling = Scalings.Linear;
      }

      protected override void Because()
      {
         _residualResult = sut.Calculate(_simulationRunResultsList, _outputMappings);
         _outputResiduals = _residualResult.AllOutputResidualsFor(_fullOutputPath).First();
      }

      [Observation]
      public void should_calculate_the_correct_residual_values_for_observed_data_values()
      {
         float lloq = _observedDataColumn.DataInfo.LLOQ ?? 0;

         lloq = Convert.ToSingle(_mergedDimension.UnitValueToBaseUnitValue(_observedDataColumn.Dimension.BaseUnit, lloq));

         for (int i = 0; i < _outputResiduals.Residuals.Count; i++)
         {
            float simValue = _simulationDataColumn.GetValue(_observedDataColumn.BaseGrid[i]);
            _outputResiduals.Residuals[i].Value.ShouldBeEqualTo(ToDouble(Max(simValue, lloq)) - ToDouble(Max(_observedDataColumn[i], lloq)));
         }
      }
   }

   public class When_calculating_the_residuals_by_ResidualCalculatorForSimulationOutputAsObservedDataLLOQ_with_log_scaling : concern_for_ResidualCalculator
   {
      private ResidualsResult _residualResult;
      private OutputResiduals _outputResiduals;

      protected override void Context()
      {
         base.Context();
         sut = new ResidualCalculatorForSimulationOutputAsObservedDataLLOQ(_timeGridRestrictor, _dimensionFactory);
         _outputMapping.Scaling = Scalings.Log;
      }

      protected override void Because()
      {
         _residualResult = sut.Calculate(_simulationRunResultsList, _outputMappings);
         _outputResiduals = _residualResult.AllOutputResidualsFor(_fullOutputPath).First();
      }

      [Observation]
      public void should_calculate_the_correct_residual_values_for_observed_data_values()
      {
         float lloq = _observedDataColumn.DataInfo.LLOQ ?? 0;

         lloq = Convert.ToSingle(_mergedDimension.UnitValueToBaseUnitValue(_observedDataColumn.Dimension.BaseUnit, lloq));

         for (int i = 0; i < _outputResiduals.Residuals.Count; i++)
         {
            float simValue = _simulationDataColumn.GetValue(_observedDataColumn.BaseGrid[i]);
            _outputResiduals.Residuals[i].Value.ShouldBeEqualTo(Math.Log10(Max(simValue, lloq)) - Math.Log10(Max(_observedDataColumn[i], lloq)));
         }
      }
   }

   public class When_calculating_the_residuals_with_lloq_usage_none : concern_for_ResidualCalculator
   {
      private ResidualsResult _residualResult;
      private OutputResiduals _outputResiduals;

      protected override void Context()
      {
         base.Context();
         sut = new ResidualCalculatorForOnlyObservedData(new TimeGridRestrictor(), _dimensionFactory);
         sut.Initialize(RemoveLLOQModes.Always);
         _outputMapping.Scaling = Scalings.Linear;
      }

      protected override void UpdateObservedDataValues()
      {
         _observedDataColumn.BaseGrid.Values = new[] { 1f, 2f, 3f };
         _observedDataColumn.Values = new[] { 0.5f, 1.1f, 0.5f };
      }

      protected override void Because()
      {
         _residualResult = sut.Calculate(_simulationRunResultsList, _outputMappings);
         _outputResiduals = _residualResult.AllOutputResidualsFor(_fullOutputPath).First();
      }

      [Observation]
      public void created_residuals_output_should_contain_one_residual_for_each_observed_data_time_point_ge_lloq()
      {
         _outputResiduals.Residuals.Count.ShouldBeEqualTo(1);
      }

      [Observation]
      public void should_calculate_the_correct_residual_values()
      {
         _outputResiduals.Residuals[0].Value.ShouldBeEqualTo(_simulationDataColumn.GetValue(_observedDataColumn.BaseGrid[1]) - _observedDataColumn[1]);
      }
   }

   public class When_calculating_the_residuals_with_lloq_usage_notrailing : concern_for_ResidualCalculator
   {
      private ResidualsResult _residualResult;
      private OutputResiduals _outputResiduals;

      protected override void Context()
      {
         base.Context();
         sut = new ResidualCalculatorForOnlyObservedData(new TimeGridRestrictor(), _dimensionFactory);
         sut.Initialize(RemoveLLOQModes.NoTrailing);
         _outputMapping.Scaling = Scalings.Linear;
      }

      protected override void UpdateObservedDataValues()
      {
         _observedDataColumn.BaseGrid.Values = new[] { 1f, 2f, 3f };
         _observedDataColumn.Values = new[] { 1.1f, 0.5f, 0.5f };
      }

      protected override void Because()
      {
         _residualResult = sut.Calculate(_simulationRunResultsList, _outputMappings);
         _outputResiduals = _residualResult.AllOutputResidualsFor(_fullOutputPath).First();
      }

      [Observation]
      public void created_residuals_output_should_contain_one_residual_for_each_observed_data_time_point_except_trailing_lloq()
      {
         _outputResiduals.Residuals.Count.ShouldBeEqualTo(2);
      }

      [Observation]
      public void should_calculate_the_correct_residual_values()
      {
         _outputResiduals.Residuals[0].Value.ShouldBeEqualTo(_simulationDataColumn.GetValue(_observedDataColumn.BaseGrid[0]) - _observedDataColumn[0]);
         _outputResiduals.Residuals[1].Value.ShouldBeEqualTo(_simulationDataColumn.GetValue(_observedDataColumn.BaseGrid[1]) - _observedDataColumn[1]);
      }
   }

   public class When_calculating_the_residuals_with_lloq_usage_notrailing_using_log_scaling_and_zero_values : concern_for_ResidualCalculator
   {
      private ResidualsResult _residualResult;
      private OutputResiduals _outputResiduals;

      protected override void Context()
      {
         base.Context();
         sut = new ResidualCalculatorForOnlyObservedData(new TimeGridRestrictor(), _dimensionFactory);
         sut.Initialize(RemoveLLOQModes.NoTrailing);
         _outputMapping.Scaling = Scalings.Log;
      }

      protected override void UpdateObservedDataValues()
      {
         _observedDataColumn.BaseGrid.Values = new[] { 1f, 2f, 3f,4f };
         _observedDataColumn.Values = new[] { 0f, 1.1f, 0.5f, 0.5f };
      }

      protected override void Because()
      {
         _residualResult = sut.Calculate(_simulationRunResultsList, _outputMappings);
         _outputResiduals = _residualResult.AllOutputResidualsFor(_fullOutputPath).First();
      }

      [Observation]
      public void created_residuals_output_should_contain_one_residual_for_each_observed_data_time_point_except_trailing_lloq_and_zero_values()
      {
         _outputResiduals.Residuals.Count.ShouldBeEqualTo(2);
      }

      [Observation]
      public void should_calculate_the_correct_residual_values()
      {
         _outputResiduals.Residuals[0].Value.ShouldBeEqualTo(Math.Log10(ToDouble(_simulationDataColumn.GetValue(_observedDataColumn.BaseGrid[1]))) - Math.Log10(ToDouble(_observedDataColumn[1])));
         _outputResiduals.Residuals[1].Value.ShouldBeEqualTo(Math.Log10(ToDouble(_simulationDataColumn.GetValue(_observedDataColumn.BaseGrid[2]))) - Math.Log10(ToDouble(_observedDataColumn[2])));
      }
   }

   public class When_calculating_the_residuals_with_observed_data_containing_NaN_values : concern_for_ResidualCalculator
   {
      private ResidualsResult _residualResult;
      private OutputResiduals _outputResiduals;

      protected override void Context()
      {
         base.Context();
         sut = new ResidualCalculatorForOnlyObservedData(new TimeGridRestrictor(), _dimensionFactory);
         sut.Initialize(RemoveLLOQModes.Always);
         _outputMapping.Scaling = Scalings.Linear;
      }

      protected override void UpdateObservedDataValues()
      {
         _observedDataColumn.BaseGrid.Values = new[] { 1f, 2f, 3f };
         _observedDataColumn.Values = new[] { 1.1f, float.NaN, float.NaN };
      }

      protected override void Because()
      {
         _residualResult = sut.Calculate(_simulationRunResultsList, _outputMappings);
         _outputResiduals = _residualResult.AllOutputResidualsFor(_fullOutputPath).First();
      }

      [Observation]
      public void created_residuals_output_should_not_contain_entries_for_NaN_values()
      {
         _outputResiduals.Residuals.Count.ShouldBeEqualTo(1);
      }

      [Observation]
      public void should_calculate_the_correct_residual_values()
      {
         _outputResiduals.Residuals[0].Value.ShouldBeEqualTo(_simulationDataColumn.GetValue(_observedDataColumn.BaseGrid[0]) - _observedDataColumn[0]);
      }
   }

   public class When_calculating_the_residuals_with_observed_data_containing_containing_values_outside_of_the_simulation_range : concern_for_ResidualCalculator
   {
      private ResidualsResult _residualResult;
      private OutputResiduals _outputResiduals;

      protected override void Context()
      {
         base.Context();
         sut = new ResidualCalculatorForOnlyObservedData(_timeGridRestrictor, _dimensionFactory);
         _outputMapping.Scaling = Scalings.Linear;

         _simulationDataColumn.BaseGrid.Values = new[] {0f, 1f, 1.5f, 2f, 3f, 4f};
         _simulationDataColumn.Values = new[] {0f, 2.5f, 1.3f, 0.9f, 0.9f, 0.5f};
      }

      protected override void UpdateObservedDataValues()
      {
         _observedDataColumn.BaseGrid.Values = new[] { 1f, 2f, 3f, 10f };
         _observedDataColumn.Values = new[] { 2f, 1.1f, 0.5f, 0.5f };
      }

      protected override void Because()
      {
         _residualResult = sut.Calculate(_simulationRunResultsList, _outputMappings);
         _outputResiduals = _residualResult.AllOutputResidualsFor(_fullOutputPath).First();
      }

      [Observation]
      public void created_residuals_output_should_contain_one_residual_for_each_observed_data_time_point()
      {
         _outputResiduals.Residuals.Count.ShouldBeEqualTo(_observedDataColumn.Values.Count -1);
      }

      [Observation]
      public void should_calculate_the_correct_residual_values_for_observed_data_values()
      {
         for (int i = 0; i < _outputResiduals.Residuals.Count; i++)
         {
            _outputResiduals.Residuals[i].Value.ShouldBeEqualTo(_simulationDataColumn.GetValue(_observedDataColumn.BaseGrid[i]) - _observedDataColumn[i]);
         }
      }
   }
}