namespace OSPSuite.Core.Domain.SensitivityAnalyses
{
   public class OutputParameterSensitivity
   {
      /// <summary>
      /// Path of varied parameter
      /// </summary>
      public string ParameterPath { get; }

      /// <summary>
      ///    Path of the quantity for which values are stored
      /// </summary>
      public string QuantityPath { get; set; }

      /// <summary>
      /// Value of actual parameter for this variation
      /// </summary>
      public double ParameterValue { get; }

      /// <summary>
      /// Values for the given <see cref="QuantityPath"/>
      /// </summary>
      public float[] QuantityValues { get; }

      /// <summary>
      /// NAme of the parameter in the sensitivity
      /// </summary>
      public  string SensitivityParameterName { get; }

      public OutputParameterSensitivity(string sensitivityParameterName, string parameterPath, double parameterValue, string quantityPath, float[] quantityValues)
      {
         ParameterPath = parameterPath;
         QuantityPath = quantityPath;
         ParameterValue = parameterValue;
         QuantityValues = quantityValues;
         SensitivityParameterName = sensitivityParameterName;
      }
   }
}