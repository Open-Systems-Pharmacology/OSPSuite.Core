using System;

namespace OSPSuite.Core.Domain.ParameterIdentifications
{
   public class Residual
   {
      public double Time { get; }
      public double Value { get; }
      public double Weight { get; }

      [Obsolete("For serialization")]
      public Residual()
      {
      }

      public Residual(double time, double value, double weight)
      {
         Time = time;
         Value = value;
         Weight = weight;
      }
   }
}