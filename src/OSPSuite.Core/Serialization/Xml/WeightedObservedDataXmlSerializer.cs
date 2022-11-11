using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Serialization.Xml
{
   public class WeightedObservedDataXmlSerializer : OSPSuiteXmlSerializer<WeightedObservedData>
   {
      public override void PerformMapping()
      {
         Map(x => x.Id);
         Map(x => x.Weights);
         MapReference(x => x.ObservedData);
      }
   }
}