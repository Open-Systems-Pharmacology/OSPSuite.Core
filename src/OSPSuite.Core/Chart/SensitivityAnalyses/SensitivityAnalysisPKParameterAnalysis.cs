using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Chart.SensitivityAnalyses
{
   public class SensitivityAnalysisPKParameterAnalysis : ObjectBase, ISimulationAnalysis
   {
      public IAnalysable Analysable { get; set; }
      public string PKParameterName { get; set; }
      public string OutputPath { get; set; }
   }
}