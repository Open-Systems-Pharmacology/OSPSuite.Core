using OSPSuite.Core.Domain.Populations;

namespace OSPSuite.R.Domain
{
   public class ParameterValueWithUnit
   {
      private readonly ParameterValue _parameterValue;

      public string Unit { get; }

      public ParameterValueWithUnit(ParameterValue parameterValue, string unit = "")
      {
         _parameterValue = parameterValue;
         Unit = unit;
      }

      public string ParameterPath => _parameterValue.ParameterPath;
      public double Value => _parameterValue.Value;
   }
}