using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Exceptions;

namespace OSPSuite.Core.Domain.ParameterIdentifications.Algorithms
{
   public interface IOptimizationAlgorithm : IWithDescription
   {
      string Name { get; }
      string DisplayName { get; }
      OptimizationRunProperties Optimize(IReadOnlyList<OptimizedParameterConstraint> constraints, Func<IReadOnlyList<OptimizedParameterValue>, OptimizationRunResult> objectiveFunc);
      OptimizationAlgorithmProperties Properties { get; }
   }

   public abstract class OptimizationAlgorithm<T> : IOptimizationAlgorithm where T : IOptimizationAlgorithm
   {
      protected ExtendedPropertyStore<T> _extendedPropertyStore;
      protected IReadOnlyList<OptimizedParameterConstraint> _constraints;
      protected Func<IReadOnlyList<OptimizedParameterValue>, OptimizationRunResult> _objectiveFunc;
      protected int _numberOfResiduals;
      public string Name { get; }
      public string Description { get; set; }
      public string DisplayName { get;  }
      public OptimizationAlgorithmProperties Properties { get; }

      protected OptimizationAlgorithm(string name, string displayName)
      {
         Name = name;
         DisplayName = displayName;
         Properties = new OptimizationAlgorithmProperties(Name);
         _extendedPropertyStore = new ExtendedPropertyStore<T>(Properties);
      }

      public OptimizationRunProperties Optimize(IReadOnlyList<OptimizedParameterConstraint> constraints, Func<IReadOnlyList<OptimizedParameterValue>, OptimizationRunResult> objectiveFunc)
      {
         _constraints = constraints;
         _objectiveFunc = objectiveFunc;

         var error = _objectiveFunc(_constraints);
         if(error.ResidualsResult.ExceptionOccured)
            throw new OSPSuiteException(error.ResidualsResult.ExceptionMessage);

         _numberOfResiduals = error.AllResidualValues.Count;

         return RunOptimization();
      }
         
      protected abstract OptimizationRunProperties RunOptimization();

      protected double[] CreateVectorFor(Func<OptimizedParameterConstraint, double> valueFunc)
      {
         return _constraints.Select(x => x.Scaling == Scalings.Linear ? valueFunc(x) : Math.Log10(valueFunc(x))).ToArray();
      }

      protected IReadOnlyList<OptimizedParameterValue> ParameterValuesFrom(double[] vector)
      {
         var values = new OptimizedParameterValue[_constraints.Count];
         for (int i = 0; i < values.Length; i++)
         {
            var value = vector[i];
            if (_constraints[i].Scaling == Scalings.Log)
               value = Math.Pow(10, value);

            values[i] = new OptimizedParameterValue(_constraints[i].Name, value, _constraints[i].StartValue);
         }
         return values;
      }
   }
}