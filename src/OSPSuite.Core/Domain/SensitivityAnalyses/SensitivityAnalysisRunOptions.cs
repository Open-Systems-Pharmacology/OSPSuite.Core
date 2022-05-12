namespace OSPSuite.Core.Domain.SensitivityAnalyses
{
   public class SensitivityAnalysisRunOptions : RunOptions
   {
      /// <summary>
      /// Specifies if simulation results should also be returned for each parameter and output. Default is <c>false</c>
      /// </summary>
      public bool ReturnOutputValues { get; set; }
   }
}