using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Chart.ParameterIdentifications
{
   public class ParameterIdentificationCorrelationMatrix : ObjectBase, ISimulationAnalysis
   {
      public IAnalysable Analysable { get; set; }
   }
}