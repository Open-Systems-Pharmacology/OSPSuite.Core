namespace OSPSuite.Core.Domain.SensitivityAnalyses
{
   public class OutputParameterSensitivity
   {
      /// <summary>
      ///    Path of varied parameter
      /// </summary>
      public string ParameterPath { get; }

      /// <summary>
      ///    Path of the output for which values are stored
      /// </summary>
      public string OutputPath { get; }

      /// <summary>
      ///    Values for the given <see cref="OutputPath" />
      /// </summary>
      public float[] OutputValues { get; }

      /// <summary>
      ///    Simulated time for the given <see cref="OutputPath" />
      /// </summary>
      public float[] TimeValues { get; }


      /// <summary>
      ///    Value of actual parameter for this variation
      /// </summary>
      public double ParameterValue { get; }

      /// <summary>
      ///    Name of the parameter in the sensitivity
      /// </summary>
      public string SensitivityParameterName { get; }

      public OutputParameterSensitivity(string sensitivityParameterName, string parameterPath, double parameterValue, string outputPath, float[] outputValues, float[] timeValues)
      {
         ParameterPath = parameterPath;
         OutputPath = outputPath;
         ParameterValue = parameterValue;
         OutputValues = outputValues;
         SensitivityParameterName = sensitivityParameterName;
         TimeValues = timeValues;
      }
   }
}