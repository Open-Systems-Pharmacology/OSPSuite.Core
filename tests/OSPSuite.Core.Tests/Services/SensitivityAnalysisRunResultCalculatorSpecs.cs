using System.Collections.Generic;
using FakeItEasy;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.SensitivityAnalyses;
using OSPSuite.Core.Domain.Services.SensitivityAnalyses;

namespace OSPSuite.Core.Services
{
   public abstract class concern_for_SensitivityAnalysisRunResultCalculator : ContextSpecification<ISensitivityAnalysisRunResultCalculator>
   {
      protected ISensitivityAnalysisPKAnalysesTask _pkAnalysisTask;
      protected SensitivityAnalysis _sensitivityAnalysis;
      protected VariationData _variationData;
      protected SimulationResults _simulationResults;
      protected PopulationSimulationPKAnalyses _pkAnalyses;

      protected override void Context()
      {
         _pkAnalysisTask = A.Fake<ISensitivityAnalysisPKAnalysesTask>();
         sut = new SensitivityAnalysisRunResultCalculator(_pkAnalysisTask);

         _pkAnalyses = new PopulationSimulationPKAnalyses();
         _sensitivityAnalysis = new SensitivityAnalysis();
         _variationData = new VariationData();
         _simulationResults = new SimulationResults();


         A.CallTo(_pkAnalysisTask).WithReturnType<PopulationSimulationPKAnalyses>().Returns(_pkAnalyses);
      }
   }

   public class When_calculating_the_sensitivity_run_results_based_on_a_sensitivity_analysis : concern_for_SensitivityAnalysisRunResultCalculator
   {
      private SensitivityAnalysisRunResult _result;
      private QuantityPKParameter _pkParameter1;
      private QuantityPKParameter _pkParameter2;
      private SensitivityParameter _sensitivityParameter1;
      private SensitivityParameter _sensitivityParameter2;
      private readonly float _defaultPK1Value = 50;
      private readonly float _defaultPK2Value = 70;
      private SensitivityParameter _sensitivityParameter3;

      protected override void Context()
      {
         base.Context();
         _pkParameter1 = new QuantityPKParameter {QuantityPath = "Output1", Name = "AUC"};
         _pkParameter1.SetValue(0, 10);
         _pkParameter1.SetValue(1, 11);
         _pkParameter1.SetValue(2, 12);
         _pkParameter1.SetValue(4, _defaultPK1Value);

         _pkParameter2 = new QuantityPKParameter {QuantityPath = "Output2", Name = "Cmax"};
         _pkParameter2.SetValue(0, 20);
         _pkParameter2.SetValue(1, 21);
         _pkParameter2.SetValue(2, 22);
         _pkParameter2.SetValue(3, 23);
         _pkParameter2.SetValue(4, _defaultPK2Value);

         _pkAnalyses.AddPKAnalysis(_pkParameter1);
         _pkAnalyses.AddPKAnalysis(_pkParameter2);

         _sensitivityParameter1 = A.Fake<SensitivityParameter>().WithName("SP1");
         A.CallTo(() => _sensitivityParameter1.DefaultValue).Returns(10);

         _sensitivityParameter2 = A.Fake<SensitivityParameter>().WithName("SP2");
         A.CallTo(() => _sensitivityParameter2.DefaultValue).Returns(20);

         _sensitivityParameter3 = A.Fake<SensitivityParameter>().WithName("SP3");
         A.CallTo(() => _sensitivityParameter3.DefaultValue).Returns(0);
         A.CallTo(() => _sensitivityParameter3.VariationRangeValue).Returns(0.8);

         _sensitivityAnalysis.AddSensitivityParameter(_sensitivityParameter1);
         _sensitivityAnalysis.AddSensitivityParameter(_sensitivityParameter2);
         _sensitivityAnalysis.AddSensitivityParameter(_sensitivityParameter3);

         var pv11 = new ParameterVariation(parameterName: _sensitivityParameter1.Name, parameterIndex: 0, variationId: 0, variation: new[] {15d, 200d, 300d});
         var pv12 = new ParameterVariation(parameterName: _sensitivityParameter1.Name, parameterIndex: 0, variationId: 1, variation: new[] {20d, 200d, 300d});
         var pv21 = new ParameterVariation(parameterName: _sensitivityParameter2.Name, parameterIndex: 1, variationId: 2, variation: new[] {100d, 21d, 300d});
         var pv22 = new ParameterVariation(parameterName: _sensitivityParameter2.Name, parameterIndex: 1, variationId: 3, variation: new[] {100d, 31d, 300d});

         _variationData.DefaultValues = new[] {100d, 200d, 300d};
         _variationData.AddVariation(pv11);
         _variationData.AddVariation(pv12);
         _variationData.AddVariation(pv21);
         _variationData.AddVariation(pv22);
      }

      protected override void Because()
      {
         _result = sut.CreateFor(_sensitivityAnalysis, _variationData, _simulationResults, new List<IndividualRunInfo>(), addOutputParameterSensitivitiesToResult: false);
      }

      [Observation]
      public void should_contain_one_entry_for_each_parameter_pk_parameter_output_combination()
      {
         _result.AllPKParameterSensitivities.Count.ShouldBeEqualTo(4);

         _result.AllPKParameterSensitivities[0].ParameterName.ShouldBeEqualTo(_sensitivityParameter1.Name);
         _result.AllPKParameterSensitivities[0].PKParameterName.ShouldBeEqualTo(_pkParameter1.Name);
         _result.AllPKParameterSensitivities[0].QuantityPath.ShouldBeEqualTo(_pkParameter1.QuantityPath);
         _result.AllPKParameterSensitivities[0].Value.ShouldBeEqualTo(((10 - _defaultPK1Value) / (15d - _sensitivityParameter1.DefaultValue) + (11 - _defaultPK1Value) / (20d - _sensitivityParameter1.DefaultValue)) / 2 * _sensitivityParameter1.DefaultValue / _defaultPK1Value);

         _result.AllPKParameterSensitivities[1].ParameterName.ShouldBeEqualTo(_sensitivityParameter2.Name);
         _result.AllPKParameterSensitivities[1].PKParameterName.ShouldBeEqualTo(_pkParameter1.Name);
         _result.AllPKParameterSensitivities[1].QuantityPath.ShouldBeEqualTo(_pkParameter1.QuantityPath);
         double.IsNaN(_result.AllPKParameterSensitivities[1].Value).ShouldBeTrue();

         _result.AllPKParameterSensitivities[2].ParameterName.ShouldBeEqualTo(_sensitivityParameter1.Name);
         _result.AllPKParameterSensitivities[2].PKParameterName.ShouldBeEqualTo(_pkParameter2.Name);
         _result.AllPKParameterSensitivities[2].QuantityPath.ShouldBeEqualTo(_pkParameter2.QuantityPath);
         _result.AllPKParameterSensitivities[2].Value.ShouldBeEqualTo(((20 - _defaultPK2Value) / (15d - _sensitivityParameter1.DefaultValue) + (21 - _defaultPK2Value) / (20d - _sensitivityParameter1.DefaultValue)) / 2 * _sensitivityParameter1.DefaultValue / _defaultPK2Value);

         _result.AllPKParameterSensitivities[3].ParameterName.ShouldBeEqualTo(_sensitivityParameter2.Name);
         _result.AllPKParameterSensitivities[3].PKParameterName.ShouldBeEqualTo(_pkParameter2.Name);
         _result.AllPKParameterSensitivities[3].QuantityPath.ShouldBeEqualTo(_pkParameter2.QuantityPath);
         _result.AllPKParameterSensitivities[3].Value.ShouldBeEqualTo(((22 - _defaultPK2Value) / (21d - _sensitivityParameter2.DefaultValue) + (23 - _defaultPK2Value) / (31d - _sensitivityParameter2.DefaultValue)) / 2 * _sensitivityParameter2.DefaultValue / _defaultPK2Value);
      }
   }

   public class When_calculating_the_sensitivity_run_results_based_on_a_sensitivity_analysis_and_the_output_should_be_calculated : concern_for_SensitivityAnalysisRunResultCalculator
   {
      private SensitivityAnalysisRunResult _result;
      private QuantityPKParameter _pkParameter1;
      private QuantityPKParameter _pkParameter2;
      private SensitivityParameter _sensitivityParameter1;
      private SensitivityParameter _sensitivityParameter2;
      private readonly float _defaultPK1Value = 50;
      private readonly float _defaultPK2Value = 70;
      private IndividualResults _resV11;
      private IndividualResults _resV12;
      private IndividualResults _resV21;
      private IndividualResults _resV22;
      private QuantityValues _timeValues;

      protected override void Context()
      {
         base.Context();
         _pkParameter1 = new QuantityPKParameter {QuantityPath = "Output1", Name = "AUC"};
         _pkParameter1.SetValue(0, 10);
         _pkParameter1.SetValue(1, 11);
         _pkParameter1.SetValue(2, 12);
         _pkParameter1.SetValue(4, _defaultPK1Value);

         _pkParameter2 = new QuantityPKParameter {QuantityPath = "Output2", Name = "Cmax"};
         _pkParameter2.SetValue(0, 20);
         _pkParameter2.SetValue(1, 21);
         _pkParameter2.SetValue(2, 22);
         _pkParameter2.SetValue(3, 23);
         _pkParameter2.SetValue(4, _defaultPK2Value);

         _pkAnalyses.AddPKAnalysis(_pkParameter1);
         _pkAnalyses.AddPKAnalysis(_pkParameter2);

         _sensitivityParameter1 = A.Fake<SensitivityParameter>().WithName("SP1");
         A.CallTo(() => _sensitivityParameter1.DefaultValue).Returns(10);
         A.CallTo(() => _sensitivityParameter1.ParameterSelection.Path).Returns("SP1-PATH");

         _sensitivityParameter2 = A.Fake<SensitivityParameter>().WithName("SP2");
         A.CallTo(() => _sensitivityParameter2.DefaultValue).Returns(20);
         A.CallTo(() => _sensitivityParameter2.ParameterSelection.Path).Returns("SP2-PATH");

         _sensitivityAnalysis.AddSensitivityParameter(_sensitivityParameter1);
         _sensitivityAnalysis.AddSensitivityParameter(_sensitivityParameter2);

         var pv11 = new ParameterVariation(parameterName: _sensitivityParameter1.Name, parameterIndex: 0, variationId: 0, variation: new[] {15d, 200d, 300d});
         var pv12 = new ParameterVariation(parameterName: _sensitivityParameter1.Name, parameterIndex: 0, variationId: 1, variation: new[] {20d, 200d, 300d});
         var pv21 = new ParameterVariation(parameterName: _sensitivityParameter2.Name, parameterIndex: 1, variationId: 2, variation: new[] {100d, 21d, 300d});
         var pv22 = new ParameterVariation(parameterName: _sensitivityParameter2.Name, parameterIndex: 1, variationId: 3, variation: new[] {100d, 31d, 300d});

         _variationData.DefaultValues = new[] {100d, 200d, 300d};
         _variationData.AddVariation(pv11);
         _variationData.AddVariation(pv12);
         _variationData.AddVariation(pv21);
         _variationData.AddVariation(pv22);

         _timeValues = new QuantityValues {QuantityPath = "Time", Values = new[] {1, 2, 3, 4, 5f}};

         _resV11 = new IndividualResults {IndividualId = 0, Time = _timeValues };
         _resV11.Add(new QuantityValues {QuantityPath = "Output1", Values = new[] {111.1f, 211.1f, 311.1f, 411.1f, 511.1f}});
         _resV11.Add(new QuantityValues {QuantityPath = "Output2", Values = new[] {111.2f, 211.2f, 311.2f, 411.2f, 511.2f}});
         _resV11.UpdateQuantityTimeReference();

         _resV12 = new IndividualResults {IndividualId = 1, Time = _timeValues };
         _resV12.Add(new QuantityValues {QuantityPath = "Output1", Values = new[] {112.1f, 212.1f, 312.1f, 412.1f, 512.1f}});
         _resV12.Add(new QuantityValues {QuantityPath = "Output2", Values = new[] {112.2f, 212.2f, 312.2f, 412.2f, 512.2f}});
         _resV12.UpdateQuantityTimeReference();

         _resV21 = new IndividualResults {IndividualId = 2, Time = _timeValues };
         _resV21.Add(new QuantityValues {QuantityPath = "Output1", Values = new[] {121.1f, 221.1f, 321.1f, 421.1f, 521.1f}});
         _resV21.Add(new QuantityValues {QuantityPath = "Output2", Values = new[] {121.2f, 221.2f, 321.2f, 421.2f, 521.2f}});
         _resV21.UpdateQuantityTimeReference();

         _resV22 = new IndividualResults {IndividualId = 3, Time = _timeValues };
         _resV22.Add(new QuantityValues {QuantityPath = "Output1", Values = new[] {122.1f, 222.1f, 322.1f, 422.1f, 522.1f}});
         _resV22.Add(new QuantityValues {QuantityPath = "Output2", Values = new[] {122.2f, 222.2f, 322.2f, 422.2f, 522.2f}});
         _resV22.UpdateQuantityTimeReference();


         _simulationResults.Add(_resV11);
         _simulationResults.Add(_resV12);
         _simulationResults.Add(_resV21);
         _simulationResults.Add(_resV22);
      }

      protected override void Because()
      {
         _result = sut.CreateFor(_sensitivityAnalysis, _variationData, _simulationResults, new List<IndividualRunInfo>(), addOutputParameterSensitivitiesToResult: true);
      }

      [Observation]
      public void should_add_one_output_sensitivity_entry_for_each_variation_and_for_each_output()
      {
         //8 = number of output * number of variations = 2*4
         _result.AllOutputParameterSensitivities.Count.ShouldBeEqualTo(8);

         _result.OutputParameterSensitivitiesBySensitivityParameterName("Output1", _sensitivityParameter1.Name).Length.ShouldBeEqualTo(2);
         _result.OutputParameterSensitivitiesBySensitivityParameterName("Output2", _sensitivityParameter1.Name).Length.ShouldBeEqualTo(2);
         _result.OutputParameterSensitivitiesBySensitivityParameterName("Output1", _sensitivityParameter2.Name).Length.ShouldBeEqualTo(2);
         _result.OutputParameterSensitivitiesBySensitivityParameterName("Output2", _sensitivityParameter2.Name).Length.ShouldBeEqualTo(2);

         //test some of the values
         _result.OutputParameterSensitivitiesBySensitivityParameterName("Output1", _sensitivityParameter1.Name)[0].OutputValues.ShouldBeEqualTo(new[] {111.1f, 211.1f, 311.1f, 411.1f, 511.1f});
         _result.OutputParameterSensitivitiesBySensitivityParameterName("Output1", _sensitivityParameter1.Name)[0].TimeValues.ShouldBeEqualTo(new[] { 1, 2, 3, 4, 5f });
         _result.OutputParameterSensitivitiesBySensitivityParameterName("Output1", _sensitivityParameter1.Name)[1].OutputValues.ShouldBeEqualTo(new[] {112.1f, 212.1f, 312.1f, 412.1f, 512.1f});

         //test some of the values
         _result.OutputParameterSensitivitiesBySensitivityParameterName("Output2", _sensitivityParameter2.Name)[0].OutputValues.ShouldBeEqualTo(new[] {121.2f, 221.2f, 321.2f, 421.2f, 521.2f});
         _result.OutputParameterSensitivitiesBySensitivityParameterName("Output2", _sensitivityParameter2.Name)[1].OutputValues.ShouldBeEqualTo(new[] {122.2f, 222.2f, 322.2f, 422.2f, 522.2f});
      }
   }
}