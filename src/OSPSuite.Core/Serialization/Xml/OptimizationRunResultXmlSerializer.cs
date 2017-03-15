using System.Xml.Linq;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain.ParameterIdentifications;

namespace OSPSuite.Core.Serialization.Xml
{
   public class OptimizationRunResultXmlSerializer : OSPSuiteXmlSerializer<OptimizationRunResult>
   {
      public override void PerformMapping()
      {
         Map(x => x.ResidualsResult);
         MapEnumerable(x => x.SimulationResults, x => x.AddResult);
         MapEnumerable(x => x.Values, x => x.AddValue);
      }

      protected override void TypedDeserialize(OptimizationRunResult optimizationRunResult, XElement outputToDeserialize, SerializationContext context)
      {
         base.TypedDeserialize(optimizationRunResult, outputToDeserialize, context);
         optimizationRunResult.SimulationResults.Each(context.AddRepository);
      }
   }
}