using System;

namespace OSPSuite.Core.Domain.ParameterIdentifications
{
   public class OptimizedParameterValue
   {
      public string Name { get; }
      public double Value { get; set; }
      public double StartValue { get; }

      [Obsolete("For serialization")]
      public OptimizedParameterValue()
      {
      }

      public OptimizedParameterValue(string name, double value, double startValue)
      {
         Name = name;
         Value = value;
         StartValue = startValue;
      }

      public override string ToString()
      {
         return $"{Name}: {Value}";
      }
   }

   public class OptimizedParameterConstraint : OptimizedParameterValue
   {
      public Scalings Scaling { get; }
      public double Min { get; }
      public double Max { get; }

      public OptimizedParameterConstraint(string name, double min, double max, double startValue, Scalings scaling) : base(name, startValue, startValue)
      {
         Scaling = scaling;
         Min = min;
         Max = max;
         Value = startValue;
      }
   }

   internal class MonteCarloOptimizedParameterConstraint : OptimizedParameterConstraint
   {
      public double Alpha { get; set; }

      public MonteCarloOptimizedParameterConstraint(string name, double min, double max, double startValue, Scalings scaling, double alpha)
         : base(name, min, max, startValue, scaling)
      {
         Alpha = alpha;
      }

      public MonteCarloOptimizedParameterConstraint(OptimizedParameterConstraint baseOptimizedParameterConstraint, double alpha)
         : this(baseOptimizedParameterConstraint.Name, baseOptimizedParameterConstraint.Min, baseOptimizedParameterConstraint.Max, baseOptimizedParameterConstraint.StartValue, baseOptimizedParameterConstraint.Scaling, alpha)
      {
      }
   }
}