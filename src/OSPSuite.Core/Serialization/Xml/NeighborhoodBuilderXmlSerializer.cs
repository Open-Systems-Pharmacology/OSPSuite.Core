using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Serialization.Xml
{
   public class NeighborhoodBuilderXmlSerializer : ContainerXmlSerializer<NeighborhoodBuilder>
   {
      public override void PerformMapping()
      {
         base.PerformMapping();
         //this is the real mapping
         Map(x => x.FirstNeighborPath);
         Map(x => x.SecondNeighborPath);

         //Compatibility with older projects
         MapReference(x => x.FirstNeighbor);
         MapReference(x => x.SecondNeighbor);
      }
   }
}