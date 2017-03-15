using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Serialization.Xml
{
   public class NeighborhoodBuilderXmlSerializer : ContainerXmlSerializer<NeighborhoodBuilder>
   {
      public override void PerformMapping()
      {
         base.PerformMapping();
         MapReference(x => x.FirstNeighbor);
         MapReference(x => x.SecondNeighbor);
      }
   }
}