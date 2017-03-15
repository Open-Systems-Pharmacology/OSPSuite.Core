using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Presentation.DTO.ParameterIdentifications
{
   public class ParameterConfidenceIntervalDTO
   {
      private readonly DoubleFormatter _doubleFormatter;

      public ParameterConfidenceIntervalDTO()
      {
         _doubleFormatter = new DoubleFormatter();
      }

      public string Name { get; set; }
      public double Value { get; set; }
      public double ConfidenceInterval { get; set; }
      public Unit Unit { get; set; }

      public string ConfidenceIntervalDisplay
      {
         get
         {
            var intervalDisplay = $"{_doubleFormatter.Format(Value)} +- {_doubleFormatter.Format(ConfidenceInterval)}";
            return Constants.NameWithUnitFor(intervalDisplay, Unit);
         }
      }
   }
}