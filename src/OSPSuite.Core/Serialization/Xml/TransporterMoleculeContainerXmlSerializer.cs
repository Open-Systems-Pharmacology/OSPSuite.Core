using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Serialization.Xml
{
   public class TransporterMoleculeContainerXmlSerializer : ContainerXmlSerializer<TransporterMoleculeContainer>
   {
      public override void PerformMapping()
      {
         base.PerformMapping();
         Map(x => x.TransportName);
      } 
   }
}