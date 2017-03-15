using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Extensions;
using PropertyNames = OSPSuite.Assets.Captions.ParameterIdentification.AlgorithmProperties.Names;
using PropertyDescriptions = OSPSuite.Assets.Captions.ParameterIdentification.AlgorithmProperties.Descriptions;

namespace OSPSuite.Core.Domain.ParameterIdentifications.Algorithms
{
   public class MonteCarloOptimizer : OptimizationAlgorithm<MonteCarloOptimizer>
   {
      private readonly Random _random;
      private const double MAX_ALPHA_INCREASE_FACTOR = 10;
      private const double ALPHA_INCREASE_FACTOR = 1.2;
      private const double ALPHA_DECREASE_FACTOR = 0.8;
      private const uint MAX_ITERATIONS = 10000;

      public MonteCarloOptimizer() : this(Environment.TickCount)
      {
      }

      public MonteCarloOptimizer(int seed) : base(Constants.OptimizationAlgorithm.MONTE_CARLO, Captions.ParameterIdentification.Algorithms.MonteCarlo)
      {
         BreakCondition = 1e-3;
         InitialAlpha = 30;
         MaxIterations = MAX_ITERATIONS;
         _random = new Random(seed);
         _extendedPropertyStore.ConfigureProperty(x => x.BreakCondition, PropertyNames.BreakCondition, PropertyDescriptions.BreakCondition);
         _extendedPropertyStore.ConfigureProperty(x => x.InitialAlpha, PropertyNames.InitialAlpha, PropertyDescriptions.InitialAlpha);
         _extendedPropertyStore.ConfigureProperty(x => x.MaxIterations, PropertyNames.MaximumNumberOfIterations, PropertyDescriptions.MaximumNumberOfIterationsMonteCarlo);
      }
      public uint MaxIterations
      {
         get { return _extendedPropertyStore.Get(x => x.MaxIterations); }
         set { _extendedPropertyStore.Set(x => x.MaxIterations, value); }
      }

      public double InitialAlpha
      {
         get { return _extendedPropertyStore.Get(x => x.InitialAlpha); }
         set { _extendedPropertyStore.Set(x => x.InitialAlpha, value); }
      }

      public double BreakCondition
      {
         get { return _extendedPropertyStore.Get(x => x.BreakCondition); }
         set { _extendedPropertyStore.Set(x => x.BreakCondition, value); }
      }

      protected override OptimizationRunProperties RunOptimization()
      {
         var oldError = double.PositiveInfinity;
         var actualError = _objectiveFunc.Invoke(_constraints).TotalError;
         var alphaMin = InitialAlpha;
         var parameterSet = createMonteCarloParametersFrom(_constraints, InitialAlpha);
         int numberOfIterations = 0;

         while ((breakConditionNotSatisfied(oldError, actualError) || alphaInRange(alphaMin)) && maxIterationsNotReached(numberOfIterations))
         {
            oldError = actualError;
            foreach (var parameter in parameterSet.OrderBy(x => _random.Next()).ToList())
            {
               var actualValue = parameter.Value;
               var newValues = variateParameterValue(parameter);

               parameter.Value = newValues.Item1;
               var error1 = _objectiveFunc(parameterSet);
               numberOfIterations++;

               parameter.Value = newValues.Item2;
               var error2 = _objectiveFunc(parameterSet);
               numberOfIterations++;

               OptimizationRunResult error;

               if (error1.TotalError < error2.TotalError)
               {
                  error = error1;
                  parameter.Value = newValues.Item1;
               }
               else
               {
                  error = error2;
               }
               if (error.TotalError < actualError)
               {
                  actualError = error.TotalError;
                  parameter.Alpha = parameter.Alpha * ALPHA_DECREASE_FACTOR;
               }
               else
               {
                  parameter.Value = actualValue;
                  parameter.Alpha = Math.Min(parameter.Alpha * ALPHA_INCREASE_FACTOR + 1, MAX_ALPHA_INCREASE_FACTOR * InitialAlpha);
               }
            }
            alphaMin = parameterSet.Select(p => p.Alpha).Min();
         }
         return new OptimizationRunProperties(numberOfIterations);
      }

      private bool maxIterationsNotReached(int numberOfIterations)
      {
         return numberOfIterations < MaxIterations;
      }

      private Tuple<double, double> variateParameterValue(MonteCarloOptimizedParameterConstraint parameter)
      {
         var a = (parameter.Value - parameter.Min) / (parameter.Max - parameter.Min);
         var z = _random.NextDouble();
         var xi = TransformNumberIntoAlphaDistribution(z, a, parameter.Alpha);
         var xiRev = getReverseValueFromDistribution(xi, a);
         return new Tuple<double, double>(computeValue(parameter, xi), computeValue(parameter, xiRev));
      }

      private static double getReverseValueFromDistribution(double xi, double a)
      {
         var xiRev = (xi <= a) ? a + (1 - xi / a) * (1 - a) : (1 - (xi - a) / (1 - a)) * a;
         if (xiRev < 0)
            return 0;

         if (xiRev > 1)
            return 1;

         return xiRev;
      }

      private static double computeValue(MonteCarloOptimizedParameterConstraint parameter, double xi)
      {
         return parameter.Min + xi * (parameter.Max - parameter.Min);
      }

      /// generate a random number xi with following distribution:
      /// (a ^ (-alpha) * xi ^ (1 / (initialAlpha + 1)), xi &lt;= a 
      /// F ( xi , a ) = { ( 1 - ( ( 1 - a ) ^ alpha * ( 1 - xi ) ) ^ ( 1 / ( initialAlpha + 1 ) ) , xi &gt;= a
      /// 
      /// a, xi and z in [0,1].
      public double TransformNumberIntoAlphaDistribution(double z, double a, double alpha)
      {
         return z <= a
            ? Math.Pow(Math.Pow(a, alpha) * z, 1 / (1 + alpha))
            : 1 - Math.Pow(Math.Pow(1 - a, alpha) * (1 - z), 1 / (1 + alpha));
      }

      private IReadOnlyList<MonteCarloOptimizedParameterConstraint> createMonteCarloParametersFrom(IReadOnlyList<OptimizedParameterConstraint> optimizedParameterConstraints, double initialAlpha)
      {
         return optimizedParameterConstraints.Select(opc => new MonteCarloOptimizedParameterConstraint(opc, initialAlpha)).ToList();
      }

      private bool alphaInRange(double alpha)
      {
         return alpha < 10 * InitialAlpha;
      }

      private bool breakConditionNotSatisfied(double oldError, double actualError)
      {
         return Math.Abs(oldError - actualError) / Math.Min(oldError, actualError) > BreakCondition;
      }
   }
}