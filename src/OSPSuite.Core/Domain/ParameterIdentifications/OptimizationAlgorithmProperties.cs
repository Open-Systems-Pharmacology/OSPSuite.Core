using System;

namespace OSPSuite.Core.Domain.ParameterIdentifications
{
   public class OptimizationAlgorithmProperties : ExtendedProperties
   {
      public string Name { get; }

      [Obsolete("For serialization")]
      public OptimizationAlgorithmProperties()
      {
      }

      public OptimizationAlgorithmProperties(string name)
      {
         Name = name;
      }

      public new virtual OptimizationAlgorithmProperties Clone()
      {
         var clone = new OptimizationAlgorithmProperties(Name);
         clone.UpdateFrom(this);
         return clone;
      }
   }
}