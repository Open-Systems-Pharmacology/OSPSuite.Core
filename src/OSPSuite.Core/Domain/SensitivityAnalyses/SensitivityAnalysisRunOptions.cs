namespace OSPSuite.Core.Domain.SensitivityAnalyses
{
   public class SensitivityAnalysisRunOptions : RunOptions
   {
      /// <summary>
      /// Specifies if simulation results should also be saved for each parameter and output. Default is <c>false</c>
      /// </summary>
      public bool SaveOutputParameterSensitivities { get; set; }
   }
}