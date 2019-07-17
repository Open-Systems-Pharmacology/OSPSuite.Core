using System.Collections.Generic;
using System.Linq;
using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.ParameterIdentifications.Algorithms;
using OSPSuite.Utility.Exceptions;

namespace OSPSuite.Core.Domain
{
   public abstract class concern_for_OptimizationAlgorithm : ContextSpecification<IOptimizationAlgorithm>
   {
      protected OptimizationRunResult _optimizationResult;
      protected double _relTolForComparison = 1e-5;

      protected override void Context()
      {
         sut = CreateOptimizationAlgorithm();
         _optimizationResult = new OptimizationRunResult();
      }

      protected override void Because()
      {
         var constraints = CreateConstraints();
         sut.Optimize(constraints, ObjectiveFunction);
      }

      protected virtual IReadOnlyList<OptimizedParameterConstraint> CreateConstraints()
      {
         // 0 <= x[0] <= 2
         var constraint1 = new OptimizedParameterConstraint("x[0]", 0, 2, 1, Scalings.Linear);

         // 0 <= x[1] <= 10
         var constraint2 = new OptimizedParameterConstraint("x[1]", 0, 10, 1, Scalings.Linear);

         return new List<OptimizedParameterConstraint>() {constraint1, constraint2};
      }

      /// <summary>
      ///    Optimize objective function (x[0]-3)^2+(x[1]-4)^2
      /// </summary>
      /// <param name="values"></param>
      /// <returns></returns>
      protected virtual OptimizationRunResult ObjectiveFunction(IReadOnlyList<OptimizedParameterValue> values)
      {
         var residualResult = new ResidualsResult();
         var outputResiduals = new OutputResiduals("A|B", new DataRepository(), new[]
         {
            new Residual(0, values[0].Value - 3, 1),
            new Residual(0, values[1].Value - 4, 1)
         });
         residualResult.AddOutputResiduals(outputResiduals);

         var optimizationRunResult = new OptimizationRunResult
         {
            ResidualsResult = residualResult,
            Values = values,
         };

         if (optimizationRunResult.TotalError < _optimizationResult.TotalError)
         {
            _optimizationResult = optimizationRunResult;
         }

         return optimizationRunResult;
      }

      protected abstract IOptimizationAlgorithm CreateOptimizationAlgorithm();

      protected void CheckOptimizedValues()
      {
         //optimizing (x[0]-3)^2+(x[1]-4)^2 with constraints 
         //                                     0 <= x[0] <=2
         //                                     0 <= x[1] <=10
         //should return x[0]=2; x[1]=4

         var optimizedValues = _optimizationResult.Values.Select(t => t.Value).ToArray();

         optimizedValues.Length.ShouldBeEqualTo(2);


         optimizedValues[0].ShouldBeEqualTo(2.0, _relTolForComparison);
         optimizedValues[1].ShouldBeEqualTo(4.0, _relTolForComparison);
      }
   }

   public class When_optimizing_using_monte_carlo : concern_for_OptimizationAlgorithm
   {
      protected override IOptimizationAlgorithm CreateOptimizationAlgorithm()
      {
         var optimizer = new MonteCarloOptimizer(1024);
         return optimizer;
      }

      [Observation]
      public void should_return_optimal_values()
      {
         CheckOptimizedValues();
      }
   }

   public class When_optimizing_using_MPFit_LM : concern_for_OptimizationAlgorithm
   {
      protected override IOptimizationAlgorithm CreateOptimizationAlgorithm()
      {
         var optimizer = new MPFitLevenbergMarquardtOptimizer();

         optimizer.Properties["ftol"].ValueAsObject = 1e-6;
         optimizer.Properties["xtol"].ValueAsObject = 1e-6;
         optimizer.Properties["gtol"].ValueAsObject = 1e-10;
         optimizer.Properties["stepfactor"].ValueAsObject = 100;
         optimizer.Properties["maxiter"].ValueAsObject = 200;
         optimizer.Properties["maxfev"].ValueAsObject = 0;

         optimizer.Properties["epsfcn"].ValueAsObject = 1e-9;

         return optimizer;
      }

      [Observation]
      public void should_return_optimal_values()
      {
         CheckOptimizedValues();
      }
   }

   public class When_optimizing_using_MPFit_LM_and_set_the_maxiter_parameter_to_zero : concern_for_OptimizationAlgorithm
   {
      protected override IOptimizationAlgorithm CreateOptimizationAlgorithm()
      {
         var optimizer = new MPFitLevenbergMarquardtOptimizer();

         optimizer.Properties["ftol"].ValueAsObject = 1e-6;
         optimizer.Properties["xtol"].ValueAsObject = 1e-6;
         optimizer.Properties["gtol"].ValueAsObject = 1e-10;
         optimizer.Properties["stepfactor"].ValueAsObject = 100;
         optimizer.Properties["maxiter"].ValueAsObject = 0;
         optimizer.Properties["maxfev"].ValueAsObject = 0;

         optimizer.Properties["epsfcn"].ValueAsObject = 1e-9;

         return optimizer;
      }

      [Observation]
      public void should_return_start_values()
      {
         var optimizedValues = _optimizationResult.Values.Select(t => t.Value).ToArray();

         optimizedValues[0].ShouldBeEqualTo(1.0, 1e-2);
         optimizedValues[1].ShouldBeEqualTo(1.0, 1e-2);
      }
   }


   public class When_optimizing_using_MPFit_LM_with_invalid_start_values : concern_for_OptimizationAlgorithm
   {
      private OSPSuiteException _exception;

      protected override IOptimizationAlgorithm CreateOptimizationAlgorithm()
      {
         return new MPFitLevenbergMarquardtOptimizer();
      }

      protected override IReadOnlyList<OptimizedParameterConstraint> CreateConstraints()
      {
         var constraint1 = new OptimizedParameterConstraint("x[0]", 0, 2, double.PositiveInfinity, Scalings.Linear);
         var constraint2 = new OptimizedParameterConstraint("x[1]", 0, 10, 1, Scalings.Linear);

         return new List<OptimizedParameterConstraint>() {constraint1, constraint2};
      }

      protected override void Because()
      {
         try
         {
            base.Because();
         }
         catch (OSPSuiteException ex)
         {
            _exception = ex;
         }
      }

      [Observation]
      public void should_throw_an_exception()
      {
         _exception.ShouldNotBeNull();
      }
   }

   public class When_optimizing_using_MPFit_LM_with_error_in_objective_function : concern_for_OptimizationAlgorithm
   {
      private OSPSuiteException _exception;
      private readonly string _objectiveFunctionErrorMessage = "Warum ist die Banane krumm?";

      protected override IOptimizationAlgorithm CreateOptimizationAlgorithm()
      {
         return new MPFitLevenbergMarquardtOptimizer();
      }

      protected override OptimizationRunResult ObjectiveFunction(IReadOnlyList<OptimizedParameterValue> values)
      {
         var residualResult = new ResidualsResult()
         {
            ExceptionOccured = true,
            ExceptionMessage = _objectiveFunctionErrorMessage
         };

         var optimizationRunResult = new OptimizationRunResult
         {
            ResidualsResult = residualResult
         };

         return optimizationRunResult;
      }

      protected override void Because()
      {
         try
         {
            base.Because();
         }
         catch (OSPSuiteException ex)
         {
            _exception = ex;
         }
      }

      [Observation]
      public void should_throw_an_exception_containing_the_error_message_from_objective_function()
      {
         _exception.ShouldNotBeNull();
         _exception.Message.Contains(_objectiveFunctionErrorMessage).ShouldBeTrue();
      }
   }

   public class When_the_first_initial_run_with_default_start_values_throws_an_exception : concern_for_OptimizationAlgorithm
   {
      private OSPSuiteException _exception;

      protected override OptimizationRunResult ObjectiveFunction(IReadOnlyList<OptimizedParameterValue> values)
      {
         return new OptimizationRunResult
         {
            ResidualsResult = new ResidualsResult
            {
               ExceptionMessage = "BLAH",
               ExceptionOccured = true
            }
         };
      }

      [Observation]
      public void should_throw_a_propagated_exception()
      {
         _exception.Message.ShouldBeEqualTo("BLAH");
      }

      protected override void Because()
      {
         try
         {
            base.Because();
         }
         catch (OSPSuiteException ex)
         {
            _exception = ex;
         }
      }

      protected override IOptimizationAlgorithm CreateOptimizationAlgorithm()
      {
         return new MPFitLevenbergMarquardtOptimizer();
      }
   }
}