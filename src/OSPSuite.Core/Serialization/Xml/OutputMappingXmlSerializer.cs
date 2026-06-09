using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Serialization.Xml
{
   public class OutputMappingXmlSerializer : OSPSuiteXmlSerializer<OutputMapping>
   {
      public override void PerformMapping()
      {
         Map(x => x.OutputSelection);
         Map(x => x.Scaling);
         Map(x => x.WeightedObservedData);
         Map(x => x.Weight);
      }
   }
}