using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Chart.ParameterIdentifications
{
   public class ParameterIdentificationResidualHistogram : ObjectBase, ISimulationAnalysis
   {
      public IAnalysable Analysable { get; set; }
   }
}