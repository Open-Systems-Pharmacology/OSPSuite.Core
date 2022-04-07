namespace OSPSuite.R.Domain
{
   public class SensitivityAnalysisRunOptions : Core.Domain.SensitivityAnalyses.SensitivityAnalysisRunOptions
   {
      /// <summary>
      ///    Specifies whether progress bar should be shown during simulation run. Default is <c>true</c>
      /// </summary>
      public bool ShowProgress { get; set; } = true;
   }
}