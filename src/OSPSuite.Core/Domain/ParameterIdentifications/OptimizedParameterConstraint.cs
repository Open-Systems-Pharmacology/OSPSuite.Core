using System;

namespace OSPSuite.Core.Domain.ParameterIdentifications
{
   public class OptimizedParameterValue
   {
      public string Name { get; set; }
      public double Value { get; set; }
      public double StartValue { get; }
      public double Min { get; internal set; }
      public double Max { get; internal set; }
      public Scalings Scaling { get; internal set; }

      [Obsolete("For serialization")]
      public OptimizedParameterValue()
      {
      }

      public OptimizedParameterValue(string name, double value, double startValue, double min, double max, Scalings scaling)
      {
         Name = name;
         Value = value;
         StartValue = startValue;
         Min = min;
         Max = max;
         Scaling = scaling;
      }

      public OptimizedParameterValue Clone() => new OptimizedParameterValue(Name, Value, StartValue, Min, Max, Scaling);

      public override string ToString()
      {
         return $"{Name}: {Value}";
      }
   }

   public class OptimizedParameterConstraint : OptimizedParameterValue
   {
      public OptimizedParameterConstraint(string name, double min, double max, double startValue, Scalings scaling) : base(name, startValue,
         startValue, min, max, scaling)
      {
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
         : this(baseOptimizedParameterConstraint.Name, baseOptimizedParameterConstraint.Min, baseOptimizedParameterConstraint.Max,
            baseOptimizedParameterConstraint.StartValue, baseOptimizedParameterConstraint.Scaling, alpha)
      {
      }
   }
}