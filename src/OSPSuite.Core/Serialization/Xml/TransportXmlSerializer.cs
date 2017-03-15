using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Serialization.Xml
{
   public class TransportXmlSerializer : ProcessXmlSerializer<Transport>
   {
      public override void PerformMapping()
      {
         base.PerformMapping();
         MapReference(x => x.SourceAmount);
         MapReference(x => x.TargetAmount);
      }
   }
}