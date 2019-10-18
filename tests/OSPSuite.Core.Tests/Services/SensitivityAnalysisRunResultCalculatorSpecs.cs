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
         _pkAnalysisTask= A.Fake<ISensitivityAnalysisPKAnalysesTask>();
         sut = new SensitivityAnalysisRunResultCalculator(_pkAnalysisTask);

         _pkAnalyses = new PopulationSimulationPKAnalyses();
         _sensitivityAnalysis = new SensitivityAnalysis();
         _variationData =new VariationData();
         _simulationResults = new SimulationResults();


         A.CallTo(_pkAnalysisTask).WithReturnType<PopulationSimulationPKAnalyses>().Returns(_pkAnalyses);
      }
   }

   public class When_calculating_the_sensitivity_run_results_based_on_a_sensitivity_analysis : concern_for_SensitivityAnalysisRunResultCalculator
   {
      private SensitivityAnalysisRunResult _result;
      private QuantityPKParameter _pkParameter1;
      private QuantityPKParameter _pkParameter2;
      private int _numberOfVariations;
      private SensitivityParameter _sensitivityParameter1;
      private SensitivityParameter _sensitivityParameter2;
      private float _defaultPK1Value = 50;
      private float _defaultPK2Value = 70;
      private SensitivityParameter _sensitivityParameter3;

      protected override void Context()
      {  
         base.Context();
         _numberOfVariations = 5;
         _pkParameter1 = new QuantityPKParameter {QuantityPath = "Output1", Name = "AUC"};
         _pkParameter1.SetNumberOfIndividuals(_numberOfVariations);
         _pkParameter1.SetValue(0, 10);
         _pkParameter1.SetValue(1, 11);
         _pkParameter1.SetValue(2, 12);
         _pkParameter1.SetValue(4, _defaultPK1Value);

         _pkParameter2 = new QuantityPKParameter { QuantityPath = "Output2", Name = "Cmax" };
         _pkParameter2.SetNumberOfIndividuals(_numberOfVariations);
         _pkParameter2.SetValue(0, 20);
         _pkParameter2.SetValue(1, 21);
         _pkParameter2.SetValue(2, 22);
         _pkParameter2.SetValue(3, 23);
         _pkParameter2.SetValue(4, _defaultPK2Value);

         _sensitivityParameter1 = A.Fake<SensitivityParameter>().WithName("SP1");
         A.CallTo(() => _sensitivityParameter1.DefaultValue).Returns(10);

         _sensitivityParameter2 = A.Fake<SensitivityParameter>().WithName("SP2");
         A.CallTo(() => _sensitivityParameter2.DefaultValue).Returns(20);

         _sensitivityParameter3 = A.Fake<SensitivityParameter>().WithName("SP3");
         A.CallTo(() => _sensitivityParameter3.DefaultValue).Returns(0);
         A.CallTo(() => _sensitivityParameter3.VariationRangeValue).Returns(0.8);

         _pkAnalyses.AddPKAnalysis(_pkParameter1);
         _pkAnalyses.AddPKAnalysis(_pkParameter2);

         _sensitivityAnalysis.AddSensitivityParameter(_sensitivityParameter1);
         _sensitivityAnalysis.AddSensitivityParameter(_sensitivityParameter2);
         _sensitivityAnalysis.AddSensitivityParameter(_sensitivityParameter3);

         var pv11 = new ParameterVariation {ParameterName = _sensitivityParameter1.Name, VariationId = 0, Variation = new[] {15d, 200d, 300d}};
         var pv12 = new ParameterVariation {ParameterName = _sensitivityParameter1.Name, VariationId = 1, Variation = new[] {20d, 200d, 300d } };
         var pv21 = new ParameterVariation {ParameterName = _sensitivityParameter2.Name, VariationId = 2, Variation = new[] {100d, 21d, 300d } };
         var pv22 = new ParameterVariation {ParameterName = _sensitivityParameter2.Name, VariationId = 3, Variation = new[] {100d, 31d, 300d } };

         _variationData.DefaultValues = new[] {100d, 200d, 300d};
         _variationData.AddVariation(pv11);
         _variationData.AddVariation(pv12);
         _variationData.AddVariation(pv21);
         _variationData.AddVariation(pv22);
      }

      protected override void Because()
      {
         _result = sut.CreateFor(_sensitivityAnalysis, _variationData, _simulationResults);
      }

      [Observation]
      public void should_contain_one_entry_for_each_parameter_pk_parameter_output_combination()
      {
         _result.AllPKParameterSensitivities.Count.ShouldBeEqualTo(4);

         _result.AllPKParameterSensitivities[0].ParameterName.ShouldBeEqualTo(_sensitivityParameter1.Name);
         _result.AllPKParameterSensitivities[0].PKParameterName.ShouldBeEqualTo(_pkParameter1.Name);
         _result.AllPKParameterSensitivities[0].QuantityPath.ShouldBeEqualTo(_pkParameter1.QuantityPath);
         _result.AllPKParameterSensitivities[0].Value.ShouldBeEqualTo(((10- _defaultPK1Value) /(15d- _sensitivityParameter1.DefaultValue) + (11 - _defaultPK1Value) / (20d - _sensitivityParameter1.DefaultValue))/2 * _sensitivityParameter1.DefaultValue / _defaultPK1Value);

         _result.AllPKParameterSensitivities[1].ParameterName.ShouldBeEqualTo(_sensitivityParameter2.Name);
         _result.AllPKParameterSensitivities[1].PKParameterName.ShouldBeEqualTo(_pkParameter1.Name);
         _result.AllPKParameterSensitivities[1].QuantityPath.ShouldBeEqualTo(_pkParameter1.QuantityPath);
         double.IsNaN(_result.AllPKParameterSensitivities[1].Value).ShouldBeTrue();

         _result.AllPKParameterSensitivities[2].ParameterName.ShouldBeEqualTo(_sensitivityParameter1.Name);
         _result.AllPKParameterSensitivities[2].PKParameterName.ShouldBeEqualTo(_pkParameter2.Name);
         _result.AllPKParameterSensitivities[2].QuantityPath.ShouldBeEqualTo(_pkParameter2.QuantityPath);
         _result.AllPKParameterSensitivities[2].Value.ShouldBeEqualTo(((20 - _defaultPK2Value) / (15d - _sensitivityParameter1.DefaultValue) + (21-_defaultPK2Value) / (20d - _sensitivityParameter1.DefaultValue)) / 2 * _sensitivityParameter1.DefaultValue / _defaultPK2Value);

         _result.AllPKParameterSensitivities[3].ParameterName.ShouldBeEqualTo(_sensitivityParameter2.Name);
         _result.AllPKParameterSensitivities[3].PKParameterName.ShouldBeEqualTo(_pkParameter2.Name);
         _result.AllPKParameterSensitivities[3].QuantityPath.ShouldBeEqualTo(_pkParameter2.QuantityPath);
         _result.AllPKParameterSensitivities[3].Value.ShouldBeEqualTo(((22 - _defaultPK2Value) / (21d - _sensitivityParameter2.DefaultValue) + (23 - _defaultPK2Value) / (31d - _sensitivityParameter2.DefaultValue)) / 2 * _sensitivityParameter2.DefaultValue / _defaultPK2Value);
      }
   }
}	