using System;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Presentation.DTO.ParameterIdentifications
{
   public class ParameterConfidenceIntervalDTO
   {
      public string Name { get; set; }
      public double Value { get; set; }
      public double ConfidenceInterval { get; set; }
      public Unit Unit { get; set; }

      /// <summary>
      ///    Confidence interval expressed in percent of the point estimate. NaN if the point estimate is zero
      /// </summary>
      public double RelativeConfidenceInterval => Value == 0 ? double.NaN : 100 * ConfidenceInterval / Math.Abs(Value);
   }
}