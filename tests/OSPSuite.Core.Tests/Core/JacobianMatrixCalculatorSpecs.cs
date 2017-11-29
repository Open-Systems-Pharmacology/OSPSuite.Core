using System;
using System.Collections.Generic;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Collections;
using FakeItEasy;
using OSPSuite.Utility.Exceptions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Helpers;

namespace OSPSuite.Core
{
   public abstract class concern_for_JacobianMatrixCalculator : ContextSpecification<IJacobianMatrixCalculator>
   {
      protected ParameterIdentification _parameterIdentification;
      protected OptimizationRunResult _runResult;
      protected ICache<ISimulation, ISimModelBatch> _simModelBatches;
      protected JacobianMatrix _result;
      protected List<OutputMapping> _allOutputMappings;
      protected List<IdentificationParameter> _allVariableIdentificationParameters;

      protected OutputMapping _output1;
      protected OutputMapping _output2;
      protected ISimulation _simulation1;
      private ISimulation _simulation2;
      protected ISimModelBatch _simModelBatch1;
      private ISimModelBatch _simModelBatch2;
      protected IdentificationParameter _identificationParameter1;
      protected IdentificationParameter _identificationParameter2;
      protected ParameterSelection _ps1;
      protected ParameterSelection _ps2;
      protected ParameterSelection _ps3;
      private DataColumn _simResults1;
      private DataColumn _simResults2;

      //context shoud represent use case defined in 47-7990 Management of Jacobi Matrix
      protected override void Context()
      {
         _parameterIdentification = A.Fake<ParameterIdentification>();
         _runResult = A.Fake<OptimizationRunResult>();
         _simModelBatches = new Cache<ISimulation, ISimModelBatch>();
         _allOutputMappings = new List<OutputMapping>();
         _allVariableIdentificationParameters = new List<IdentificationParameter>();

         A.CallTo(() => _parameterIdentification.AllOutputMappings).Returns(_allOutputMappings);
         A.CallTo(() => _parameterIdentification.AllVariableIdentificationParameters).Returns(_allVariableIdentificationParameters);
         sut = new JacobianMatrixCalculator();

         _output1 = A.Fake<OutputMapping>();
         _simulation1 = A.Fake<ISimulation>();
         _simModelBatch1 = A.Fake<ISimModelBatch>();
         _simModelBatches.Add(_simulation1, _simModelBatch1);
         A.CallTo(() => _output1.Simulation).Returns(_simulation1);
         A.CallTo(() => _output1.FullOutputPath).Returns("S1|OutputPath1");
         A.CallTo(() => _output1.OutputPath).Returns("OutputPath1");
         _allOutputMappings.Add(_output1);

         _output2 = A.Fake<OutputMapping>();
         _simulation2 = A.Fake<ISimulation>();
         _simModelBatch2 = A.Fake<ISimModelBatch>();
         A.CallTo(() => _output2.Simulation).Returns(_simulation2);
         A.CallTo(() => _output2.FullOutputPath).Returns("S2|OutputPath2");
         A.CallTo(() => _output2.OutputPath).Returns("OutputPath2");
         _simModelBatches.Add(_simulation2, _simModelBatch2);
         _allOutputMappings.Add(_output2);

         _identificationParameter1 = new IdentificationParameter().WithName("IP1");
         _identificationParameter2 = new IdentificationParameter().WithName("IP2");

         _ps1 = A.Fake<ParameterSelection>();
         _ps1.Parameter.Value = 100;
         A.CallTo(() => _ps1.FullQuantityPath).Returns("S1|ParameterPath1");
         A.CallTo(() => _ps1.Path).Returns("ParameterPath1");
         A.CallTo(() => _ps1.Simulation).Returns(_simulation1);
         _identificationParameter1.AddLinkedParameter(_ps1);

         _ps2 = A.Fake<ParameterSelection>();
         _ps2.Parameter.Value = 200;
         A.CallTo(() => _ps2.FullQuantityPath).Returns("S2|ParameterPath2");
         A.CallTo(() => _ps2.Path).Returns("ParameterPath2");
         A.CallTo(() => _ps2.Simulation).Returns(_simulation2);
         A.CallTo(() => _ps2.Dimension).Returns(_ps1.Dimension);
         _identificationParameter1.AddLinkedParameter(_ps2);

         _ps3 = A.Fake<ParameterSelection>();
         _ps3.Parameter.Value = 300;
         A.CallTo(() => _ps3.FullQuantityPath).Returns("S2|ParameterPath3");
         A.CallTo(() => _ps3.Path).Returns("ParameterPath3");
         A.CallTo(() => _ps3.Simulation).Returns(_simulation2);
         _identificationParameter2.AddLinkedParameter(_ps3);

         _allVariableIdentificationParameters.Add(_identificationParameter1);
         _allVariableIdentificationParameters.Add(_identificationParameter2);

         A.CallTo(() => _runResult.AllResidualsFor(_output1.FullOutputPath)).Returns(new[] {new Residual(1, 11, 1), new Residual(2, 21, 2), new Residual(3, 31, 0)});
         A.CallTo(() => _runResult.AllResidualsFor(_output2.FullOutputPath)).Returns(new[] {new Residual(1, 12, 3), new Residual(3, 32, 4), new Residual(4, 42, 5)});

         A.CallTo(() => _simModelBatch1.SensitivityValuesFor(_output1.OutputPath, _ps1.Path)).Returns(new[] {11d, 21d, 31d, 41d, 51d});
         A.CallTo(() => _simModelBatch1.SensitivityValuesFor(_output1.OutputPath, _ps2.Path)).Throws(new OSPSuiteException());
         A.CallTo(() => _simModelBatch1.SensitivityValuesFor(_output1.OutputPath, _ps3.Path)).Throws(new OSPSuiteException());

         A.CallTo(() => _simModelBatch2.SensitivityValuesFor(_output2.OutputPath, _ps1.Path)).Throws(new OSPSuiteException());
         A.CallTo(() => _simModelBatch2.SensitivityValuesFor(_output2.OutputPath, _ps2.Path)).Returns(new[] {12d, 22d, 32d, 42d, 52d, 62d});
         A.CallTo(() => _simModelBatch2.SensitivityValuesFor(_output2.OutputPath, _ps3.Path)).Returns(new[] {13d, 23d, 33d, 43d, 53d, 63d});

         _simResults1 = new DataColumn
         {
            BaseGrid = new BaseGrid("Time", DomainHelperForSpecs.TimeDimensionForSpecs()) {Values = new[] {1f, 2f, 3f, 4f, 5f}},
            Values = new[] {10f, 20f, 30f, 40f, 50f}
         };

         A.CallTo(() => _runResult.SimulationResultFor(_output1.FullOutputPath)).Returns(_simResults1);

         _simResults2 = new DataColumn
         {
            BaseGrid = new BaseGrid("Time", DomainHelperForSpecs.TimeDimensionForSpecs()) {Values = new[] {1f, 2f, 3f, 4f, 5f, 6f}},
            Values = new[] {10f, 20f, 30f, 40f, 50f, 60f}
         };

         A.CallTo(() => _runResult.SimulationResultFor(_output2.FullOutputPath)).Returns(_simResults2);
      }

      protected override void Because()
      {
         _result = sut.CalculateFor(_parameterIdentification, _runResult, _simModelBatches);
      }
   }

   public class When_calculating_the_jacobi_matrix_according_to_a_predefined_problem_use_case_1 : concern_for_JacobianMatrixCalculator
   {
      protected override void Context()
      {
         base.Context();
         _output1.Scaling = Scalings.Linear;
         _output2.Scaling = Scalings.Linear;
      }

      [Observation]
      public void should_return_one_row_per_residual()
      {
         _result.RowCount.ShouldBeEqualTo(5);
      }

      [Observation]
      public void should_return_one_column_per_parameter_identification_parameter()
      {
         _result.ColumnCount.ShouldBeEqualTo(2);
      }

      [Observation]
      public void should_return_the_expected_value_for_the_jacobian()
      {
         _result[0][_identificationParameter1.Name].ShouldBeEqualTo(11);
         _result[0][_identificationParameter2.Name].ShouldBeEqualTo(0);

         _result[1][_identificationParameter1.Name].ShouldBeEqualTo(21 * 2);
         _result[1][_identificationParameter2.Name].ShouldBeEqualTo(0);

         _result[2][_identificationParameter1.Name].ShouldBeEqualTo(12 * 3);
         _result[2][_identificationParameter2.Name].ShouldBeEqualTo(13 * 3);

         _result[3][_identificationParameter1.Name].ShouldBeEqualTo(32 * 4);
         _result[3][_identificationParameter2.Name].ShouldBeEqualTo(33 * 4);

         _result[4][_identificationParameter1.Name].ShouldBeEqualTo(42 * 5);
         _result[4][_identificationParameter2.Name].ShouldBeEqualTo(43 * 5);
      }
   }

   public class When_calculating_the_jacobi_matrix_according_to_a_predefined_problem_use_case_2 : concern_for_JacobianMatrixCalculator
   {
      protected override void Context()
      {
         base.Context();
         _output1.Scaling = Scalings.Log;
         _output2.Scaling = Scalings.Linear;
      }

      [Observation]
      public void should_return_one_row_per_residual()
      {
         _result.RowCount.ShouldBeEqualTo(5);
      }

      [Observation]
      public void should_return_one_column_per_parameter_identification_parameter()
      {
         _result.ColumnCount.ShouldBeEqualTo(2);
      }

      [Observation]
      public void should_return_the_expected_value_for_the_jacobian()
      {
         _result[0][_identificationParameter1.Name].ShouldBeEqualTo(11 * 1 / (10 * Math.Log(10)));
         _result[0][_identificationParameter2.Name].ShouldBeEqualTo(0);

         _result[1][_identificationParameter1.Name].ShouldBeEqualTo(21 * 1 / (20 * Math.Log(10)) * 2);
         _result[1][_identificationParameter2.Name].ShouldBeEqualTo(0);

         _result[2][_identificationParameter1.Name].ShouldBeEqualTo(12 * 3);
         _result[2][_identificationParameter2.Name].ShouldBeEqualTo(13 * 3);

         _result[3][_identificationParameter1.Name].ShouldBeEqualTo(32 * 4);
         _result[3][_identificationParameter2.Name].ShouldBeEqualTo(33 * 4);

         _result[4][_identificationParameter1.Name].ShouldBeEqualTo(42 * 5);
         _result[4][_identificationParameter2.Name].ShouldBeEqualTo(43 * 5);
      }
   }

   public class When_calculating_the_jacobi_matrix_according_to_a_predefined_problem_use_case_3 : concern_for_JacobianMatrixCalculator
   {
      protected override void Context()
      {
         base.Context();
         _output1.Scaling = Scalings.Linear;
         _output2.Scaling = Scalings.Linear;
         _identificationParameter1.UseAsFactor = true;
      }

      [Observation]
      public void should_return_one_row_per_residual()
      {
         _result.RowCount.ShouldBeEqualTo(5);
      }

      [Observation]
      public void should_return_one_column_per_parameter_identification_parameter()
      {
         _result.ColumnCount.ShouldBeEqualTo(2);
      }

      [Observation]
      public void should_return_the_expected_value_for_the_jacobian()
      {
         _result[0][_identificationParameter1.Name].ShouldBeEqualTo(11 * _ps1.Parameter.Value);
         _result[0][_identificationParameter2.Name].ShouldBeEqualTo(0);

         _result[1][_identificationParameter1.Name].ShouldBeEqualTo(21 * _ps1.Parameter.Value * 2);
         _result[1][_identificationParameter2.Name].ShouldBeEqualTo(0);

         _result[2][_identificationParameter1.Name].ShouldBeEqualTo(12 * _ps2.Parameter.Value * 3);
         _result[2][_identificationParameter2.Name].ShouldBeEqualTo(13 * 3);

         _result[3][_identificationParameter1.Name].ShouldBeEqualTo(32 * _ps2.Parameter.Value * 4);
         _result[3][_identificationParameter2.Name].ShouldBeEqualTo(33 * 4);

         _result[4][_identificationParameter1.Name].ShouldBeEqualTo(42 * _ps2.Parameter.Value * 5);
         _result[4][_identificationParameter2.Name].ShouldBeEqualTo(43 * 5);
      }
   }

   public class When_calculating_the_jacobian_matrix_for_a_log_output_where_the_sensitivy_is_zero_or_the_output_value_is_zero : concern_for_JacobianMatrixCalculator
   {
      private DataColumn _simResults1;

      protected override void Context()
      {
         base.Context();
         _simModelBatches.Clear();
         _allOutputMappings.Clear();
         _allVariableIdentificationParameters.Clear();

         _output1.Scaling = Scalings.Log;
         _simModelBatches.Add(_simulation1, _simModelBatch1);
         _allOutputMappings.Add(_output1);

         _allVariableIdentificationParameters.Add(_identificationParameter1);

         A.CallTo(() => _runResult.AllResidualsFor(_output1.FullOutputPath)).Returns(new[] {new Residual(1, 11, 1), new Residual(3, -5, 1),});
         A.CallTo(() => _simModelBatch1.SensitivityValuesFor(_output1.OutputPath, _ps1.Path)).Returns(new[] {0d, 21d, 31d, 41d, 51d});

         _simResults1 = new DataColumn
         {
            BaseGrid = new BaseGrid("Time", DomainHelperForSpecs.TimeDimensionForSpecs()) {Values = new[] {1f, 2f, 3f, 4f, 5f}},
            Values = new[] {10f, 20f, 0, 40f, 50f}
         };

         A.CallTo(() => _runResult.SimulationResultFor(_output1.FullOutputPath)).Returns(_simResults1);
      }

      [Observation]
      public void should_return_zero_when_the_sensitivity_is_zero()
      {
         _result[0][_identificationParameter1.Name].ShouldBeEqualTo(0);
      }

      [Observation]
      public void should_return_infinity_for_an_output_whose_value_is_zero()
      {
         double.IsInfinity(_result[1][_identificationParameter1.Name]).ShouldBeTrue();
      }
   }
}