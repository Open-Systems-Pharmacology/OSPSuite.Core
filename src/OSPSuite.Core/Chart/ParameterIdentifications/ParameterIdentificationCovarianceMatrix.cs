using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Chart.ParameterIdentifications
{
   public class ParameterIdentificationCovarianceMatrix : ObjectBase, ISimulationAnalysis
   {
      public IAnalysable Analysable { get; set; }
   }
}