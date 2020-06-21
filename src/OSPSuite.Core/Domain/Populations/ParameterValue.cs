namespace OSPSuite.Core.Domain.Populations
{
   public class ParameterValue : RandomValue
   {
      public string ParameterPath { get; set; }

      public ParameterValue(string parameterPath, double value, double percentile)
      {
         ParameterPath = parameterPath;
         Value = value;
         Percentile = percentile;
      }

      public ParameterValue Clone()
      {
         return new ParameterValue(ParameterPath, Value, Percentile);
      }
   }
}