using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using FakeItEasy;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services.ParameterIdentifications;
using OSPSuite.Core.Extensions;
using OSPSuite.Helpers;

namespace OSPSuite.Core
{
   public abstract class concern_for_TimeProfileConfidenceIntervalCalculator : ContextSpecification<ITimeProfileConfidenceIntervalCalculator>
   {
      private IMatrixCalculator _matrixCalculator;
      protected IConfidenceIntervalDataRepositoryCreator _dataRepositoryCreator;
      protected ParameterIdentification _parameterIdentification;
      protected ParameterIdentificationRunResult _runResult;
      private List<OutputMapping> _allOutputMappings;
      private List<IdentificationParameter> _allVariableIdentificationParameters;
      protected OutputMapping _output1;
      private ISimulation _simulation1;
      protected OutputMapping _output2;
      private ISimulation _simulation2;
      private IdentificationParameter _identificationParameter1;
      private IdentificationParameter _identificationParameter2;
      private ParameterSelection _ps1_1;
      private ParameterSelection _ps2_1;
      private ParameterSelection _ps1_2;
      private ParameterSelection _ps2_2;
      protected OutputMapping _output3;
      protected OutputMapping _output4;
      private ResidualsResult _residualResults;
      private DataRepository _observedData1;
      private DataRepository _observedData2;
      private DataRepository _observedData3;
      private DataRepository _observedData4;
      private DataRepository _observedData5;

      protected Cache<string, double[]> _confidenceIntervals;
      protected IReadOnlyList<DataRepository> _result;

      protected override void Context()
      {
         _matrixCalculator = A.Fake<IMatrixCalculator>();
         _dataRepositoryCreator = A.Fake<IConfidenceIntervalDataRepositoryCreator>();
         sut = new TimeProfileConfidenceIntervalCalculator(_matrixCalculator, _dataRepositoryCreator);

         _identificationParameter1 = new IdentificationParameter().WithName("IP1");
         _identificationParameter2 = new IdentificationParameter().WithName("IP2");

         var parameterNames = new[] {_identificationParameter1.Name, _identificationParameter2.Name};

         _parameterIdentification = A.Fake<ParameterIdentification>();
         _runResult = A.Fake<ParameterIdentificationRunResult>();
         _runResult.JacobianMatrix = new JacobianMatrix(parameterNames);
         _allOutputMappings = new List<OutputMapping>();
         _allVariableIdentificationParameters = new List<IdentificationParameter>();
         _residualResults = new ResidualsResult();
         _runResult.BestResult.ResidualsResult = _residualResults;

         A.CallTo(() => _parameterIdentification.AllOutputMappings).Returns(_allOutputMappings);
         A.CallTo(() => _parameterIdentification.AllVariableIdentificationParameters).Returns(_allVariableIdentificationParameters);

         _simulation1 = A.Fake<ISimulation>().WithName("S1");
         _simulation2 = A.Fake<ISimulation>().WithName("S2");

         _output1 = createOutput(_simulation1, "C1", Scalings.Log);
         _allOutputMappings.Add(_output1);

         _output2 = createOutput(_simulation2, "C2", Scalings.Linear);
         _allOutputMappings.Add(_output2);

         _output3 = createOutput(_simulation1, "C3", Scalings.Log);
         _allOutputMappings.Add(_output3);

         _output4 = createOutput(_simulation2, "C4", Scalings.Linear);
         _allOutputMappings.Add(_output4);


         _ps1_1 = createParameterSelection(_simulation1, "P1_1");
         _identificationParameter1.AddLinkedParameter(_ps1_1);

         _ps2_1 = createParameterSelection(_simulation1, "P2_1");
         A.CallTo(() => _ps2_1.Dimension).Returns(_ps1_1.Dimension);
         _identificationParameter1.AddLinkedParameter(_ps2_1);

         _ps1_2 = createParameterSelection(_simulation1, "P1_2");
         _identificationParameter2.AddLinkedParameter(_ps1_2);

         _ps2_2 = createParameterSelection(_simulation2, "P2_2");
         A.CallTo(() => _ps2_2.Dimension).Returns(_ps1_2.Dimension);
         _identificationParameter2.AddLinkedParameter(_ps2_2);

         _allVariableIdentificationParameters.Add(_identificationParameter1);
         _allVariableIdentificationParameters.Add(_identificationParameter2);

         _observedData1 = A.Fake<DataRepository>();
         _observedData2 = A.Fake<DataRepository>();
         _observedData3 = A.Fake<DataRepository>();
         _observedData4 = A.Fake<DataRepository>();
         _observedData5 = A.Fake<DataRepository>();

         _residualResults.AddOutputResiduals(_output1.FullOutputPath, _observedData1, new[]
         {
            new Residual(2, 0.10, 1),
            new Residual(4, 0.40, 2),
            new Residual(6, 1.00, 1),
            new Residual(8, 0, 0),
         });

         _residualResults.AddOutputResiduals(_output1.FullOutputPath, _observedData2, new[]
         {
            new Residual(6, 0.20, 2),
            new Residual(8, 0.80, 4),
            new Residual(10, -0.20, 2),
            new Residual(12, 0, 0),
         });

         _residualResults.AddOutputResiduals(_output2.FullOutputPath, _observedData3, new[]
         {
            new Residual(2, 0, 0),
            new Residual(4, 0.20, 2),
            new Residual(6, -0.20, 2),
            new Residual(8, 0, 0),
         });

         _residualResults.AddOutputResiduals(_output2.FullOutputPath, _observedData4, new[]
         {
            new Residual(6, 0.20, 2),
            new Residual(8, 0.80, 4),
            new Residual(10, -0.20, 2),
            new Residual(12, 0, 0),
         });

         _residualResults.AddOutputResiduals(_output2.FullOutputPath, _observedData5, new[]
         {
            new Residual(6, 0, 0),
            new Residual(8, 0, 0),
            new Residual(10, 0, 0),
            new Residual(12, 0, 0),
         });


         var baseGrid = new BaseGrid("Time", DomainHelperForSpecs.TimeDimensionForSpecs()) {Values = new[] {0f, 2f, 4f, 6f, 8f, 10f, 12f, 14f, 16f}};
         var C1 = new DataColumn
         {
            BaseGrid = baseGrid,
            Values = new[] {0f, 0.71f, 2.10f, 3.14f, 3.76f, 4.04f, 4.12f, 4.09f, 4.02f}
         };
         A.CallTo(() => _runResult.BestResult.SimulationResultFor(_output1.FullOutputPath)).Returns(C1);

         var C2 = new DataColumn
         {
            BaseGrid = baseGrid,
            Values = new[] {0f, 0.37f, 0.38f, 0.31f, 0.23f, 0.17f, 0.14f, 0.12f, 0.11f}
         };
         A.CallTo(() => _runResult.BestResult.SimulationResultFor(_output2.FullOutputPath)).Returns(C2);

         var C3 = new DataColumn
         {
            BaseGrid = baseGrid,
            Values = new[] {0f, 0.53f, 0.54f, 0.43f, 0.32f, 0.25f, 0.20f, 0.17f, 0.16f}
         };
         A.CallTo(() => _runResult.BestResult.SimulationResultFor(_output3.FullOutputPath)).Returns(C3);

         var C4 = new DataColumn
         {
            BaseGrid = baseGrid,
            Values = new[] {0f, 0.16f, 0.16f, 0.13f, 0.10f, 0.07f, 0.06f, 0.05f, 0.05f}
         };
         A.CallTo(() => _runResult.BestResult.SimulationResultFor(_output4.FullOutputPath)).Returns(C4);


         var covarianceMatrix = new Matrix(parameterNames, parameterNames);
         A.CallTo(() => _matrixCalculator.CovarianceMatrixFrom(_runResult.JacobianMatrix, _residualResults)).Returns(covarianceMatrix);
         covarianceMatrix.SetRow(0, new[] {5.05, -1.22});
         covarianceMatrix.SetRow(1, new[] {-1.22, 0.51});

         _runResult.JacobianMatrix.AddPartialDerivatives(createPartialDerivatives(_output1.FullOutputPath, parameterNames, new[]
         {
            new[] {0d, 0d},
            new[] {1d, 2d},
            new[] {2d, 3d},
            new[] {1d, 1d},
            new[] {0.5d, 0.7d},
            new[] {0.4d, 0.5d},
            new[] {0.3d, 0.5d},
            new[] {0.3d, 0.2d},
            new[] {0.1d, 0.2d}
         }));

         _runResult.JacobianMatrix.AddPartialDerivatives(createPartialDerivatives(_output2.FullOutputPath, parameterNames, new[]
         {
            new[] {0d, 0d},
            new[] {-1d, -2d},
            new[] {-1d, -2d},
            new[] {-0.5d, -1d},
            new[] {0.1d, 0.1d},
            new[] {-0.1d, -0.1d},
            new[] {0.2d, 0.2d},
            new[] {0.3d, 0.3d},
            new[] {0.2d, 0.2d}
         }));

         _runResult.JacobianMatrix.AddPartialDerivatives(createPartialDerivatives(_output3.FullOutputPath, parameterNames, new[]
         {
            new[] {0d, 0d},
            new[] {1d, 2d},
            new[] {2d, 3d},
            new[] {1d, 1d},
            new[] {0.5d, 0.7d},
            new[] {0.4d, 0.5d},
            new[] {0.3d, 0.5d},
            new[] {0.3d, 0.2d},
            new[] {0.1d, 0.2d},
         }));

         _runResult.JacobianMatrix.AddPartialDerivatives(createPartialDerivatives(_output4.FullOutputPath, parameterNames, new[]
         {
            new[] {0d, 0d},
            new[] {-4d, -4d},
            new[] {-3d, -3d},
            new[] {-2d, -2d},
            new[] {-1d, -1d},
            new[] {-0.5d, -0.5d},
            new[] {-0.4d, -0.4d},
            new[] {-0.3d, -0.3d},
            new[] {-0.2d, -0.2d},
         }));

         _confidenceIntervals = new Cache<string, double[]>();
         A.CallTo(() => _dataRepositoryCreator.CreateFor(A<string>._, A<double[]>._, A<OutputMapping>._, A<OptimizationRunResult>._))
            .Invokes(x => { _confidenceIntervals.Add(x.GetArgument<OutputMapping>(2).FullOutputPath, x.GetArgument<double[]>(1)); })
            .Returns(new DataRepository());
      }

      private PartialDerivatives createPartialDerivatives(string fullOutputPath, IEnumerable<string> parameterNames, double[][] derivatives)
      {
         var partialDerivatives = new PartialDerivatives(fullOutputPath, parameterNames);
         derivatives.Each(x => partialDerivatives.AddPartialDerivative(x));
         return partialDerivatives;
      }

      private OutputMapping createOutput(ISimulation simulation, string outputPath, Scalings scaling)
      {
         var output = A.Fake<OutputMapping>();
         output.Scaling = scaling;
         A.CallTo(() => output.Simulation).Returns(simulation);
         A.CallTo(() => output.FullOutputPath).Returns(new ObjectPath(outputPath.ToPathArray()).AndAddAtFront(simulation.Name).PathAsString);
         A.CallTo(() => output.OutputPath).Returns(outputPath);
         return output;
      }

      private ParameterSelection createParameterSelection(ISimulation simulation, string parameterPath, double initialValue = 0)
      {
         var parameterSelection = A.Fake<ParameterSelection>();
         parameterSelection.Parameter.Value = initialValue;
         A.CallTo(() => parameterSelection.Simulation).Returns(simulation);
         A.CallTo(() => parameterSelection.FullQuantityPath).Returns(new ObjectPath(parameterPath.ToPathArray()).AndAddAtFront(simulation.Name).PathAsString);
         A.CallTo(() => parameterSelection.Path).Returns(parameterPath);
         return parameterSelection;
      }

      protected void CompareValues(OutputMapping outputMapping, double[] expectedConfidenceIntervals)
      {
         var calculatedConfidenceIntervals = _confidenceIntervals[outputMapping.FullOutputPath];
         double[] expectedValues = expectedConfidenceIntervals;
         if (outputMapping.Scaling == Scalings.Log)
            expectedValues = expectedValues.Select(x => Math.Pow(10, x)).ToArray();

         calculatedConfidenceIntervals.Length.ShouldBeEqualTo(expectedValues.Length);
         for (int i = 0; i < calculatedConfidenceIntervals.Length; i++)
         {
            calculatedConfidenceIntervals[i].ShouldBeEqualTo(expectedValues[i], 1e-3);
         }
      }
   }

   public class When_calculating_the_time_profile_confidence_interval_of_a_predefined_run_results : concern_for_TimeProfileConfidenceIntervalCalculator
   {
      protected override void Because()
      {
         _result = sut.CalculateConfidenceIntervalFor(_parameterIdentification, _runResult);
      }

      [Observation]
      public void should_return_one_data_repository_per_mapped_output()
      {
         _result.Count.ShouldBeEqualTo(4);
         _confidenceIntervals.Count.ShouldBeEqualTo(4);
         CompareValues(_output1, new[] { 0.00000, 2.05709, 1.49049, 0.55267, 0.21202, 0.16268, 0.11083, 0.13768, 0.03633 });
         CompareValues(_output2, new[] { 0.00000, 3.36300, 3.36300, 1.68150, 0.39958, 0.39958, 0.79917, 1.19875, 0.79917 });
         CompareValues(_output3, new[] { 0.00000, 2.75572, 5.79635, 4.03575, 2.49121, 2.62889, 2.28303, 3.31233, 0.91283 });
         CompareValues(_output4, new[] { 0.00000, 15.98337, 11.98753, 7.99168, 3.99584, 1.99792, 1.59834, 1.19875, 0.79917 });
      }
   }

   public class When_calculating_the_time_profile_prediction_interval_of_a_predefined_run_results : concern_for_TimeProfileConfidenceIntervalCalculator
   {
      protected override void Because()   
      {
         _result = sut.CalculatePredictionIntervalFor(_parameterIdentification, _runResult);
      }

      [Observation]
      public void should_return_one_data_repository_per_mapped_output()
      {
         _confidenceIntervals.Count.ShouldBeEqualTo(4);
         CompareValues(_output1, new[] { 0.00000, 2.14802, 1.61368, 0.82936, 0.65372, 0.63942, 0.62823, 0.63352, 0.61945 });
         CompareValues(_output2, new[] { 0.51532, 3.40225, 3.40225, 1.75869, 0.65209, 0.65209, 0.95091, 1.30482, 0.95091 });
         _confidenceIntervals[_output3.FullOutputPath].ShouldBeNull();
         _confidenceIntervals[_output4.FullOutputPath].ShouldBeNull();
      }
   }

   public class When_calculating_the_time_profile_VPC_interval_of_a_predefined_run_results : concern_for_TimeProfileConfidenceIntervalCalculator
   {
      protected override void Because()
      {
         _result = sut.CalculateVPCIntervalFor(_parameterIdentification, _runResult);
      }

      [Observation]
      public void should_return_one_data_repository_per_mapped_output()
      {
         CompareValues(_output1, new[] { 0, 0.61838, 0.61838, 0.61838, 0.61838, 0.61838, 0.61838, 0.61838, 0.61838 });
        CompareValues(_output2, new[] { 0.51532, 0.51532, 0.51532, 0.51532, 0.51532, 0.51532, 0.51532, 0.51532, 0.51532 });
         _confidenceIntervals[_output3.FullOutputPath].ShouldBeNull();
         _confidenceIntervals[_output4.FullOutputPath].ShouldBeNull();
      }
   }
}